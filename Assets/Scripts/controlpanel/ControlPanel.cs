using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour {
	public Text movePoints;

	// Use this for initialization
	void Start () {
		
	}
	void Update(){
		Player selectedPlayer = GameManager.instance.getSelectedPlayer (); 
		if (selectedPlayer == null) {
			return;
		}
		if (selectedPlayer.movePointsLeftDirty) {
			selectedPlayer.movePointsLeftDirty = false;
			Speed speed = selectedPlayer.getMovePointsLeft ();
			Debug.Log ("Setting mp left="+speed.currentValue);
			movePoints.text = ""+speed.currentValue;
		}
	}
}
