using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {
	public Text moveText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Player selectedPlayer = GameManager.instance.getSelectedPlayer (); 
		if (selectedPlayer == null) {
			return;
		}
		if (selectedPlayer.movePointsLeftDirty) {
			selectedPlayer.movePointsLeftDirty = false;
			Speed speed = selectedPlayer.getMovePointsLeft ();
			moveText.text = "Move "+speed.currentValue+"("+speed.maxValue+")";
		}
	}
}
