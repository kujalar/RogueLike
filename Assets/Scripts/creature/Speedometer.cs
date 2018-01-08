using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer
{
	bool noMoveLeft = false;
	Speed[] speedArray = new Speed[4];
	public void Add(Speed speed)
	{
		speedArray[(int)speed.type] = speed;
	}
	public Speed GetSpeed(SpeedType speedType){
		return speedArray [(int)speedType];
	}
	public bool PayMovementAllowance(int cost, SpeedType type)
	{
		Speed speed = speedArray[(int)type];
		// if (cost > speed.currentValue && speed.currentValue != speed.baseSpeed)
        if(cost > speed.GetMoveLeft() && speed.HasSpentMovePoints())
        {
			//noMoveLeft = true;
			//this means we cannot move because we cannot pay the price!
			return false;
		}
        //TODO , as we pay the price we should reduce all movement types 
        speed.PaySpeed(cost);
        /*
		if (speed.currentValue <= 0)
		{
			speed.currentValue = 0;
			//noMoveLeft = true;
		}*/
		return true;
	}
    public void DoDash()
    {
        for(int i = 0; i < 4; i++)
        {
            Speed speed = speedArray[i];
            if (speed != null)
            {
                //TODO dirtyness
                speed.dashValue = speed.baseSpeed;
                speed.currentDashValue = speed.baseSpeed;
            }
        }
    }

    //obsolete I might think, not sure though.
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
				speed.currentValue = speed.baseSpeed;//this shows how much baseSpeed is unspent.
                speed.dashValue = 0; //this shows how much dash speed has been added by an action.
                speed.currentDashValue = 0;//this shows how much dash speed is unspent.
			}
		}
	}
}
public class Speed
{
	public SpeedType type;
	public int baseSpeed;
	public int currentValue;
    public int dashValue;
    public int currentDashValue;

    //tell's us how much move is left when summing baseSpeed and optional dashValue (dashing)
    public int GetMoveLeft()
    {
        return currentValue + currentDashValue;
    }
    public bool HasSpentMovePoints()
    {
        return currentValue != baseSpeed;
    }
    public void PaySpeed(int amountSpent)
    {
        currentValue = currentValue - amountSpent;
        if (currentValue < 0)
        {
            currentDashValue = currentDashValue + currentValue;
            currentValue = 0;
        }
        
    }
}

public enum SpeedType
{
	LAND,
	FLY,
	SWIM,
	HOVER
}
