using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsData : MonoBehaviour {




	public List<ActionOption> actionOptions = new List<ActionOption>();

	private Speed speed;

    void Start()
    {
        //TODO factory to make different kind of statisticsdatas for different creature types
		//TODO factory to build Speed object
        speed = new Speed();
        speed.type = SpeedType.LAND;
        speed.maxValue = 30;
        speed.currentValue = 30;

		ActionOption attackOption = new ActionOption ();
		attackOption.action = new AttackAction ();
		actionOptions.Add (attackOption);

		ActionOption dashOption = new ActionOption ();
		attackOption.action = new DashAction ();
		actionOptions.Add (dashOption);

		ActionOption dodgeOption = new ActionOption ();
		attackOption.action = new DodgeAction ();
		actionOptions.Add (dodgeOption);
    }

	public Speed GetSpeed()
    {
        return speed;
    }
}

