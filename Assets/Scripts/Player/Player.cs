using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ControlSelectedCreatureMovement ();
	}
    
	private void ControlSelectedCreatureMovement(){
		//we should control selectedCreature if it exists and is our players Creature and has active turn
		Creature selectedCreature = GameManager.instance.getSelectedCreature ();
		if (selectedCreature == null || gameObject != selectedCreature.controller
			|| selectedCreature.IsNoInitiative() ) {
			return;
		}
			
		//checks are clear and we can take input.
		int horizontal = 0;
		int vertical = 0;
		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical = (int)Input.GetAxisRaw("Vertical");
		if(horizontal != 0)
		{
			//this prevents diagonical move
			//TODO make diagonicalMove
			vertical = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			Debug.Log ("sending move command");
			selectedCreature.MoveCommand (horizontal, vertical);
		}
	}
}


