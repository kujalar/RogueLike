using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject, ActorObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private int food;

    private bool playersTurn = false;
    //when freeMove = true, the player is not in any InitiativeTrack, creature not on init list can move everytime when notBusy
    //TODO freeMove could be checked, if player is on some initiativeTrack, then player should have no freeMove, do isFreeMove() boolean method!!!
    private bool freeMove = true;
    
    // Use this for initialization
    protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        
        foodText.text = "Food:" + food;
        //initialiseFreeMove to true
        freeMove = true;

        base.Start();
        
    }

    protected void StartFight()
    {
        Debug.Log("Start a Fight");
        //to start a fight, add player to an initiativetrack
        GameManager.instance.initiativeTrack.Register(this);
        //disable players freeMove
        freeMove = false;
        //now there is only one way to start initiativeSystem.
        GameManager.instance.initiativeTrack.StartRound();
    }
    protected void LeaveFight()
    {
        //TODO UnderConstruction.
        //here we should switch back to freeMove and remove ourselves from the initiativeTrack.
    }
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update () {
        if ((!freeMove && !playersTurn)||this.isBusy) return;
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if(horizontal != 0)
        {
            //this prevents diagonical move
            vertical = 0;
        }


        if(horizontal != 0 || vertical != 0)
        {   
            //we have chosen some action, it means, after an action we check if fight begins
            AttemptMove<Wall>(horizontal, vertical);

            //check fight conditions, now they are pretty straightforward. we just fight if there are enemies on the board.
            //aim of this is, that unless there is a fight going on, the player can move freely as fast as his speed allows.
            //when the fight is on, we change to a turn based system, where you act turnbased,move with allowances and do actions.
            if (freeMove == true && GameManager.instance.initiativeTrack.Size() > 0)
            {
                StartFight();
            } 
        }
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food:" + food;

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        //now because we call Move Again it is actually called twice... FIX it when you have time
        if(Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }


        CheckIfGameOver();
        //this means player control is taken off - TODO this should be made local... now it is global and applies only to one player
        playersTurn = false;
        //here we have moved, we must tell it to initiativeTrack.
        //TODO you might want to place some delay to passing initiative. Now all the objects will get their turn before 
        //turn passing objects smoothMovement script etc is ready. 
        if (!freeMove)
        {
            GameManager.instance.initiativeTrack.NextTurn(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } else if(other.tag == "Food")
        {
            food += pointsPerFood;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            foodText.text = "+" + pointsPerFood + " Food:" + food;
            other.gameObject.SetActive(false);
        } else if(other.tag == "Soda")
        {
            food += pointsPerSoda;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            foodText.text = "+" + pointsPerSoda + " Food:" + food;
            other.gameObject.SetActive(false);
        }
    }


    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }
    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
        //and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public int RollInitiative()
    {
        //this should be made random, but now i can test like this.
        return 10;
    }
    public void StartTurn()
    {
        Debug.Log("Players turn start");
        //TODO this is global, this should be changed
        playersTurn = true;
    }


    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food:" + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if(food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    public void OnMouseOver()
    {
        CanvasController.instance.setMouseEnterText(name);
    }
}
