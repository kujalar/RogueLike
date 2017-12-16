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
	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food:" + food;

        base.Start();

        //add our object to initiativetrack
        GameManager.instance.initiativeTrack.Register(this);
        //after our one and only player has entered.... start the round... all enemies should be on the scene already
        GameManager.instance.initiativeTrack.StartRound();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update () {
        if (!GameManager.instance.playersTurn) return;
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
            AttemptMove<Wall>(horizontal, vertical);
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
        GameManager.instance.playersTurn = false;
        //here we have moved, we must tell it to initiativeTrack TODO here is some bugs still, and remember there are bugs with the enemy script too.
        GameManager.instance.initiativeTrack.NextTurn(this);
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
        GameManager.instance.playersTurn = true;
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
