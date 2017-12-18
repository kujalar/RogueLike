using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsData : MonoBehaviour, CreatureStatistics {


    private Speedometer speedometer;

    StatisticsData()
    {
        //TODO factory to build Speed object
        Speed speed = new Speed();
        speed.type = SpeedType.LAND;
        speed.maxValue = 30;
        speed.currentValue = 30;
        speedometer = new Speedometer();
        speedometer.Add(speed);
    }

	public Speedometer GetSpeedometer()
    {
        return speedometer;
    }
}
public class Speedometer
{
    bool noMoveLeft = false;
    Speed[] speedArray = new Speed[4];
    public void Add(Speed speed)
    {
        speedArray[(int)speed.type] = speed;
    }
    public bool PayMovementAllowance(int cost, SpeedType type)
    {
        Speed speed = speedArray[(int)type];
        if (cost > speed.currentValue && speed.currentValue != speed.maxValue)
        {
            noMoveLeft = true;
            speed.currentValue = 0;
            //this means we cannot move because we cannot pay the price anymore!
            return false;
        }
        //TODO , as we pay the price we should reduce all movement types 
        speed.currentValue = speed.currentValue - cost;
        if (speed.currentValue <= 0)
        {
            speed.currentValue = 0;
            noMoveLeft = true;
        }
        return true;
    }
    public bool NoMoveLeft()
    {
        return noMoveLeft;
    }
    public void reset()
    {
        noMoveLeft = false;
        for(int i = 0; i < 4; i++)
        {
            Speed speed = speedArray[i];
            if (speed != null)
            {
                speed.currentValue = speed.maxValue;
            }
        }
    }
}
public class Speed
{
    public SpeedType type;
    public int maxValue;
    public int currentValue;
}

public enum SpeedType
{
    LAND,
    FLY,
    SWIM,
    HOVER
}
