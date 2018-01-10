using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionometer: MonoBehaviour {
    public bool IsDirty = true;

	public int actionsLeft = 1;
	public int maxBonusActions = 0;
	public int bonusActionsLeft = 0;
	public int maxLegendaryActions = 0;//Players have these usually 0
	public int legendaryActionsLeft = 0;
	public int reactionsLeft = 1;
	public int minorActionsLeft = 1;
    

    private Creature creature;

	//these simple params hold the action that is used. Works because you can do only one action of each type.
	//these are like a log to be displayed what was chosen on a current round.
	public ObjectData<Action> usedAction = new ObjectData<Action>();
	public ObjectData<Action> usedBonusAction = new ObjectData<Action>();
	public ObjectData<Action> usedReaction= new ObjectData<Action>();
	//TODO usedLegendaryActions.

	void Start () {
		creature = GetComponent<Creature> ();
	}


	//TODO we might need builder, now we have a fast implementation here
	public void init(){
		//turha
	}

	// call this method when players turn starts. This will reset the action counters.
	public void Reset(){
		actionsLeft = 1;
		bonusActionsLeft = maxBonusActions;
		legendaryActionsLeft = maxLegendaryActions;
		reactionsLeft = 1;
		minorActionsLeft = 1;
		usedAction.setData (Actions.EMPTY);
		usedBonusAction.setData (Actions.EMPTY);
		usedReaction.setData (Actions.EMPTY);
	
	}
	
	//call this method when action is chosen.
	public void DoAction(ActionOption actionOption){
		if (actionsLeft < 1 || actionOption.isUsed) {
			Debug.LogWarning ("No action left warning! actionsLeft="+actionsLeft+" actionOption.isUsed="+actionOption.isUsed);
			return;
		}

		actionOption.action.Execute (creature);
		actionsLeft--;
		//this is set so that we may show what action was chosen
		usedAction.setData(actionOption.action);
		//TODO different kind of actions might have different used option. Some are 3 / day and some might be still active and so on.
		actionOption.isUsed = true;
        GameEvents.instance.FireRefreshActionDisplay(creature.gameObject);
	}
	//call this method when bonus action is chosen.
	public void DoBonusAction(ActionOption actionOption){
		if (bonusActionsLeft < 1 || actionOption.isUsed) {
			Debug.LogWarning ("No bonusAction left warning!");
			return;
		}
		actionOption.action.Execute (creature);
		bonusActionsLeft--;
		//this is set so that we may show what bonus action was chosen
		usedBonusAction.setData(actionOption.action);
		//TODO different kind of bonusActions might have different used option. Some are 3 / day and some might be still active and so on.
		actionOption.isUsed = true;
        GameEvents.instance.FireRefreshActionDisplay(creature.gameObject);
    }
	//TODO minorActions and Legendary actions and reactions which should have some trigger and implemented somewhere.
}
//this is a creatures option for an action. It might be used already in turn
public class ActionOption {
	public bool isUsed = false;
	public bool isAction = false;
	public bool isBonusAction = false;
	public bool isReaction = false;
	public Action action;
}
//this is an interface for the action business logic
public interface Action {
	ActionType GetActionType();//actions type with which it can be identified
	string GetName();
	void Execute(Creature source);
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
	public void Execute(Creature source){
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
	public void Execute(Creature source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Creature is dodging.");
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
	public void Execute(Creature source){
        source.GetSpeedometer().DoDash();
        //TODO make this into event that signals dirtyness - perhaps.
        GameEvents.instance.FireRefreshMovePointDisplay(source.gameObject);
		Debug.Log (source.name+" is dashing.");
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
	public void Execute(Creature source){
		//TODO make the logicx
		Debug.Log ("UnderConstruction. Creature may attack.");
	}
}
