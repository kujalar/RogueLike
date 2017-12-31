using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOptionBuilder {
	ActionOption actionOption;

	public static ActionOptionBuilder GetInstance(){
		ActionOptionBuilder builder = new ActionOptionBuilder ();

		return builder.startNew();
	}
	public ActionOptionBuilder startNew(){
		this.actionOption = new ActionOption ();
		return this;
	}
	//Empty action is newer action or bonus action. It is used to declare ActionIcon has no action.
	public ActionOptionBuilder withEmptyAction(){
		//TODO when you have a correct icons, uset them from IconManager, IconManager is put inside the ColorManager gameObject
		this.actionOption.action = Actions.EMPTY;
		this.actionOption.isAction = false;
		this.actionOption.isBonusAction = false;
		return this;
	}
	//this sets action and enables actionOption.isAction
	public ActionOptionBuilder withAction(Action action){
		this.actionOption.action = action;
		this.actionOption.isAction = true;
		return this;
	}
	public ActionOption GetActionOption(){
		return actionOption;
	}
}
