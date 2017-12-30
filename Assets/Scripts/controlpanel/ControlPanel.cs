using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour {
	public GameObject fab_actionIcon;

	private float firstActionCol = 8f;
	private float actionColWidth = 56f;
	List<ActionIcon> actionIcons = new List<ActionIcon> ();

	private Player player;
	// Use this for initialization
	void Start () {
		//instantiate one row of icons, with empty icons. do 5 icons
		ActionOption emptyOption = new ActionOption();
		emptyOption.action = new EmptyAction ();
		for (int i = 0; i < 5; i++) {
			instantiateNewActionIcon ();
		}
	}

	private void instantiateNewActionIcon(){
		GameObject instance = Instantiate (fab_actionIcon) as GameObject;
		instance.transform.SetParent (gameObject.transform);
		float colNumber = actionIcons.Count;
		float x = firstActionCol + (colNumber * actionColWidth);
		instance.transform.localPosition = new Vector3 (x, 0f, 0f);
		ActionIcon actionIcon = instance.GetComponent<ActionIcon> ();
		actionIcons.Add (actionIcon);
	}

	void Update(){
		Player selectedPlayer = GameManager.instance.getSelectedPlayer ();
		if (player != selectedPlayer) {
			player = selectedPlayer;
			InitActionIconsWithOptions ();
		}
	}
	private void InitActionIconsWithOptions(){
		//TODO when player changes, init ActionIcons with players actionometer

	}

}
