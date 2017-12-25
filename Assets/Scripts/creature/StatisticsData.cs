using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsData : MonoBehaviour {


	private Speed speed;

    StatisticsData()
    {
        //TODO factory to build Speed object
        speed = new Speed();
        speed.type = SpeedType.LAND;
        speed.maxValue = 30;
        speed.currentValue = 30;
    }

	public Speed GetSpeed()
    {
        return speed;
    }
}

