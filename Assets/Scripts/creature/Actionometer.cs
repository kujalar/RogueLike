using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionometer: MonoBehaviour {
	public int actionsLeft = 1;
	public int maxBonusActions = 0;
	public int bonusActionsLeft = 0;
	public int maxLegendaryActions = 0;//Players have these usually 0
	public int legendaryActionsLeft = 0;
	public int reactionsLeft = 1;
	public int minorActionsLeft = 1;

	public List<ActionOption> actions = new List<ActionOption>();
	public List<ActionOption> bonusActions = new List<ActionOption>();
	public List<ActionOption> legendaryActions = new List<ActionOption>();
	public List<ActionOption> minorActions = new List<ActionOption>();

	private Player player;

	void Start () {
		player = GetComponent<Player> ();
	}


	//TODO we might need builder, now we have a fast implementation here
	public void init(){
		ActionOption dodgeOption = new ActionOption ();
		//TODO we should not make whole object for every player. Actions should be easy to build singleton. this is now quick and fast first sketch.
		dodgeOption.action = new DodgeAction ();
		actions.Add (dodgeOption);
	}

	// call this method when players turn starts. This will reset the action counters.
	public void Reset(){
		actionsLeft = 1;
		bonusActionsLeft = maxBonusActions;
		legendaryActionsLeft = maxLegendaryActions;
		reactionsLeft = 1;
		minorActionsLeft = 1;
	}
	//call this method when action is chosen.
	public void DoAction(ActionOption actionOption){
		if (actionsLeft < 1 || actionOption.isUsed) {
			Debug.LogWarning ("No action left warning!");
			return;
		}
		actionsLeft--;
		//TODO different kind of actions might have different used option. Some are 3 / day and some might be still active and so on.
		actionOption.isUsed = true;
		actionOption.action.Execute (player);
	}
	//call this method when bonus action is chosen.
	public void DoBonusAction(ActionOption actionOption){
		//TODO bonus actions still under ćonstruction
	}
	//TODO minorActions and Legendary actions and reactions which should have some trigger and implemented somewhere.
}
//this is a creatures option for an action. It might be used already in turn
public class ActionOption {
	public bool isUsed = false;
	public bool isAction = true;
	public bool isBonusAction = false;
	public bool isReaction = false;
	public Action action;
}
//this is an interface for the action business logic
public interface Action {
	ActionType GetActionType();//actions type with which it can be identified
	string GetName();
	void Execute(Player source);
}

public enum ActionType {
	EMPTY,
	DODGE,
	DASH,
	ATTACK
}
public class EmptyAction : Action {
	private ActionType type = ActionType.EMPTY;

	public string GetName(){
		return "";
	}

	public ActionType GetActionType(){
		return type;
	}
	public void Execute(Player source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Empty action.");
	}
}


public class DodgeAction : Action {
	private ActionType type = ActionType.DODGE;

	public string GetName(){
		return "Dodge";
	}

	public ActionType GetActionType(){
		return type;
	}
	public void Execute(Player source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Player is dodging.");
	}
}

public class DashAction : Action {
	private ActionType type = ActionType.DASH;

	public string GetName(){
		return "Dash";
	}

	public ActionType GetActionType(){
		return type;
	}
	public void Execute(Player source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Player is dashing.");
	}
}

public class AttackAction : Action {
	private ActionType type = ActionType.ATTACK;

	public string GetName(){
		return "Attack";
	}

	public ActionType GetActionType(){
		return type;
	}
	public void Execute(Player source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Player may attack.");
	}
}
