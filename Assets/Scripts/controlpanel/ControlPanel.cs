using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour {
	public GameObject fab_actionIcon;

	private float firstActionCol = 8f;
	private float actionColWidth = 56f;
	List<ActionIcon> actionIcons = new List<ActionIcon> ();

	private Creature creature;
	// Use this for initialization
	void Start () {
		//instantiate one row of icons, with empty icons. do 5 icons
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
		ActionOption emptyOption = ActionOptionBuilder.GetInstance ().withEmptyAction ().GetActionOption ();
		actionIcon.Init (emptyOption,null);
		actionIcons.Add (actionIcon);
	}

	void Update(){
		Creature selectedCreature = GameManager.instance.getSelectedCreature ();
		if (creature != selectedCreature) {
			creature = selectedCreature;
			InitActionIconsWithOptions ();
		}
	}
	private void InitActionIconsWithOptions(){
		//TODO when player changes, init ActionIcons with players actionometer

		List<ActionOption> options = creature.GetStatistics ().actionOptions;
		Debug.Log("InitActionIconsWithOptions options.Count="+options.Count);
		Actionometer actionometer = creature.GetActionometer ();
		for (int i = 0; i < options.Count; i++) {
			ActionOption option = options [i];
			if (i >= actionIcons.Count) {
				Debug.LogError ("Creature has too many options, you have not implemented paging for actionIcons. actionIcons.Count="+actionIcons.Count+" i="+i);
			} else {
				actionIcons [i].Init (option, actionometer);
			}
		}
	}

}
