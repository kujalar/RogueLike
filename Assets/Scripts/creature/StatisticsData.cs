using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsData : MonoBehaviour {




	public List<ActionOption> actionOptions = new List<ActionOption>();

	private Speed speed;
	void Awake(){
		//TODO factory to make different kind of statisticsdatas for different creature types
		//TODO factory to build Speed object
		speed = new Speed();
		speed.type = SpeedType.LAND;
		speed.maxValue = 30;
		speed.currentValue = 30;

		ActionOptionBuilder actionOptionBuilder = ActionOptionBuilder.GetInstance ().withAction (Actions.ATTACK);
		actionOptions.Add (actionOptionBuilder.GetActionOption());

		actionOptions.Add (actionOptionBuilder.startNew().withAction(Actions.DASH).GetActionOption());

		actionOptions.Add (actionOptionBuilder.startNew().withAction(Actions.DODGE).GetActionOption());
	}
    void Start()
    {
        
    }

	public Speed GetSpeed()
    {
        return speed;
    }
}

