using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {
	private Text moveText;
	public GameObject moveTextObject;
	public GameObject fab_ActionHolder;

	private float firstActionRow = -22f;
	private float actionRowHeight = -14f;
	private GameObject actionHolder = null;
	private GameObject bonusActionHolder = null;
	private GameObject reactionHolder = null;

	private List<GameObject> panelRows = new List<GameObject>();
	private bool componentPositioningDirty=false;
	// Use this for initialization
	void Start () {
		/*actionHolder = Instantiate (fab_ActionHolder) as GameObject; //, new Vector3(100f,100f, 0f), Quaternion.identity) as GameObject;
		instance.transform.SetParent(gameObject.transform);
		instance.transform.localPosition = new Vector3 (6f, firstActionRow, 0f);*/
		ColorManager colorManager = ColorManager.instance;
		actionHolder = initActionHolder (colorManager.actionColor);
		bonusActionHolder = initActionHolder (colorManager.bonusActionColor);
		reactionHolder = initActionHolder (colorManager.reactionColor);

		moveText = moveTextObject.GetComponent<Text> ();
		panelRows.Add (moveTextObject);

		Debug.Log ("Start StatsPanel");
		componentPositioningDirty = true;

        GameEvents gameEvents = GameEvents.instance;
        //join to listen events that tell us what part of UI should be redrawn
        gameEvents.OnRefreshMovePointDisplay += UpdateMovePointDisplay;
        gameEvents.OnRefreshSelectedCreatureDisplay += UpdateStatsPanel;
        gameEvents.OnRefreshActionDisplay += UpdateActionDisplay;
	}
	private GameObject initActionHolder(Color color){
		GameObject instance = Instantiate (fab_ActionHolder) as GameObject;
		instance.transform.SetParent (gameObject.transform);
		instance.transform.localPosition = new Vector3 (6f, firstActionRow, 0f);
		ActionHolder actionHolder = instance.GetComponent<ActionHolder> ();
		actionHolder.SetSymbolColor (color);
		panelRows.Add (instance);
		instance.SetActive (false);
		return instance;
	}
	


	void RemoveActions(){
		actionHolder.SetActive (false);
		bonusActionHolder.SetActive (false);
		reactionHolder.SetActive (false);
	}
	void AddAction(){
		//action holder is always the first row
		actionHolder.SetActive (true);
	}
	void AddBonusAction(){
		bonusActionHolder.SetActive (true);
	}
	void AddReaction(){
		reactionHolder.SetActive (true);
	}

    //UpdateAll
    void UpdateStatsPanel()
    {
        UpdateMovePointDisplay();
        UpdateActionDisplay();
    }
    void UpdateMovePointDisplay()
    {
        Creature selectedCreature = GameManager.instance.getSelectedCreature();
        if (selectedCreature == null)
        {
            return;
        }
        
        Debug.Log("draw moveText");
        selectedCreature.movePointsLeftDirty = false;
        Speed speed = selectedCreature.getMovePointsLeft();
        string dashText = "";
        if (speed.dashValue > 0)
        {
            dashText = "+" + speed.dashValue;
        }
        moveText.text = "Move " + speed.GetMoveLeft() + "(" + speed.baseSpeed + dashText + ")";
    }



	// Update is called once per frame
	void Update () {
		//Creature selectedCreature = GameManager.instance.getSelectedCreature (); 

		//UpdateMovePointsLeft (selectedCreature);
		//UpdateActions (selectedCreature);
		//UpdateComponentPositioning ();
	}

	private void UpdateActionDisplay(){
        Creature selectedCreature = GameManager.instance.getSelectedCreature();
        if (selectedCreature == null) {
			return;
		}
		Actionometer actionometer = selectedCreature.GetActionometer ();
		
		updateActionHolder (actionometer.usedAction.data,actionHolder);
		updateActionHolder (actionometer.usedBonusAction.data,bonusActionHolder);

        UpdateComponentPositioning();
	}
	void updateActionHolder(Action action,GameObject holder){
		if (action == null || ActionType.EMPTY.Equals (action.GetActionType ())) {
			holder.SetActive (false);
		} else {
			ActionHolder ah = holder.GetComponent<ActionHolder> ();
			ah.SetText (action.GetName());
			holder.SetActive (true);
		}
		componentPositioningDirty = true;
	}

	void UpdateComponentPositioning(){

		Debug.Log ("UpdateComponentPositioning");
		float count = 0f;
		for (int i = 0; i < panelRows.Count; i++) {
			GameObject holder = panelRows [i];
			if (holder.activeSelf) {
				float y = firstActionRow + (count * actionRowHeight);
				Debug.Log ("y="+y);
				holder.transform.localPosition= new Vector3 (6f,y,0f);
				count += 1f;
			}
		}
	}
}
