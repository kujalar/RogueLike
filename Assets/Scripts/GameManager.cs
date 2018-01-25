using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public GameObject levelImage;

    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;

    private Creature selectedCreature;
  //  [HideInInspector] public bool playersTurn = false;

    private Text levelText;

    private int level = 3;
 //   private List<Enemy> enemies;
    private bool enemiesMoving;

    private bool doingSetup;

    [HideInInspector] public InitiativeTrack initiativeTrack;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            //SceneManager.sceneLoaded += OnSceneLoaded;
        } else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
     //   enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        initiativeTrack = GetComponent<InitiativeTrack>();
        
	}
    private void Start()
    {
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }
	public void setSelectedCreature(Creature creature){
        //some experimenting with the EventSystem
        GameObject esGO = GameObject.Find("EventSystem");
        EventSystem es = esGO.GetComponent<EventSystem>();
        //this will call OnSelected from the Creature that was selected and OnDeSelect from the one who was not... currently does nothing special
        es.SetSelectedGameObject(creature.gameObject);
        //this works for now on
		selectedCreature = creature;
        //ask the display to show whole new guy
        GameEvents.instance.FireRefreshSelectedCreature();
    }
	public Creature getSelectedCreature(){
		return selectedCreature;
	}
    //here we are going to setup the level
    void InitGame()
    {
        doingSetup = true;

        //we find images like this, because they are instantiated on level load. cant put in editor.
        //levelImage = GameObject.Find("LevelImage");
        levelImage.SetActive(true);
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        
        
        //wait the delay and invoke method after that.
        Invoke("HideLevelImage", levelStartDelay);
        initiativeTrack.Clear();
       // enemies.Clear();
        //TODO we must get number of enemies, so that they can be added to the initiativeTrack
        boardScript.SetupScene(level);

        //Experimenting with this system... looks like I'm going to use ObjectData to store my stats that I wish to follow on UI Canvas
        ObjectData<int> intData = new ObjectData<int>();
        intData.setData(1);
        Debug.Log("objectData = "+intData.data);
    }
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

	// Update is called once per frame
	void Update () {
        /*
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
        */
	}

    //Enemies used to call this from Enemy.cs when Start

    /*
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    //Here we will move all our enemies one at a time
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
    */

}
