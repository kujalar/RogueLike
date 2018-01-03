using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creature : MovingObject, ActorObject //, Creature
{
	public GameObject controller;
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
	private StatisticsData statistics;
	private Speedometer speedometer;
	private Actionometer actionometer;

    private bool creaturesTurn = false;
    //when freeMove = true, the player is not in any InitiativeTrack, creature not on init list can move everytime when notBusy
    //TODO freeMove could be checked, if player is on some initiativeTrack, then player should have no freeMove, do isFreeMove() boolean method!!!
    private bool freeMove = true;
    
	//some booleans to determine if ui needs to be refreshed
	public bool movePointsLeftDirty = true;

	public Speedometer GetSpeedometer(){
		return speedometer;
	}
	public Actionometer GetActionometer(){
		return actionometer;
	}
	public StatisticsData GetStatistics(){
		return statistics;
	}

	protected void Awake(){
		speedometer = new Speedometer ();
	}
    // Use this for initialization
    protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        
        foodText.text = "Food:" + food;
        //initialiseFreeMove to true
        freeMove = true;

        base.Start();
        //TODO some kind of statistics loading/factory
        statistics = GetComponent<StatisticsData>();
		//init speedometer
		speedometer.Add (statistics.GetSpeed ());
		//init actionometer
		actionometer = GetComponent<Actionometer>();
		actionometer.init();

		//TODO we should place the player, who is currentSelectedPlayer
		GameManager.instance.setSelectedCreature (this);
	}

    protected void StartFight()
    {
        Debug.Log("Start a Fight");
        //to start a fight, add creature to an initiativetrack
        GameManager.instance.initiativeTrack.Register(this);
        //disable creatures freeMove
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

	public bool IsNoInitiative(){
		if ((!freeMove && !creaturesTurn) || this.isBusy)
			return true;
		return false;
	}

	//this is used to command the Creature to Move one square
	public void MoveCommand(int xDir, int yDir){
		//we have chosen some action, it means, after an action we check if fight begins
		AttemptMove<Wall>(xDir, yDir);

		//check fight conditions, now they are pretty straightforward. we just fight if there are enemies on the board.
		//aim of this is, that unless there is a fight going on, the player can move freely as fast as his speed allows.
		//when the fight is on, we change to a turn based system, where you act turnbased,move with allowances and do actions.
		if (freeMove == true && GameManager.instance.initiativeTrack.Size() > 0)
		{
			StartFight();
		} 
	}


	public Speed getMovePointsLeft(){
		SpeedType currentSpeedType = SpeedType.LAND;
		Speed currentSpeed = speedometer.GetSpeed (currentSpeedType);
		return currentSpeed;
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food:" + food;

        //base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        //speedometer is now used like in turnBased mode... 
        //TODO you should make also freeMove mode where speedometer will not count current move, but instead it will count how fast player moves 1 block.
        //TODO diagonal movement
        //TODO difficult terrain
        if(Move(xDir,yDir,speedometer,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        } else if(hit.transform != null)
        {
            //we cannotMove and hit something with our move
            //with player we are calling this method with T = Wall, so if we hit something with component wall we are gona dig it.
            T hitComponent = hit.transform.GetComponent<T>();

            if (hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }
		if (!freeMove) {
			//simple thing, ask to refresh moves left if we are not doing free move. During free move we should just show max move left.
			movePointsLeftDirty = true;
		}

        bool endMyTurn = false;
        //TODO now we end Players turn when his speedometer turns to 0. this should be changed.
		endMyTurn = speedometer.NoMoveLeft();
        

        CheckIfGameOver();
        
        //here we have moved, we must tell it to initiativeTrack.
        //TODO you might want to place some delay to passing initiative. Now all the objects will get their turn before 
        //turn passing objects smoothMovement script etc is ready. 
        if (!freeMove&&endMyTurn)
        {
			EndTurn ();
        }
    }
	//call this when the player wants to end the turn.
	public void EndTurn(){
		Debug.Log("Creatures turn ends");
		creaturesTurn = false;
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
        Debug.Log("Creatures turn starts");
        speedometer.reset();
		actionometer.Reset ();
		movePointsLeftDirty = true;
		creaturesTurn = true;

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
