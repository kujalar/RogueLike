using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChart : MonoBehaviour {

    public static TerrainChart instance;

    private void Awake()
    {
        instance = this;
    }

    public int GetCostToEnter(RogueLikeTile targetTile,float distance, SpeedType speedType)
    {
        //TODO tänne voisi laittaa monta vaihtehtoista speedtypeä ja niistä sitten valitaan se mikä on halvin liikkujalle, ja käytetään sitä.
        string code;
        if (targetTile != null)
        {
            code = targetTile.GetCode();
        } else
        {
            code = "_";//this means clear
        }

        Debug.Log("distance="+distance+" and movecode="+code);

        //now if we got code S then it means solid wall, no pass. This might be made better too. S is also used in drawing the maps
        if (code == "S")
        {
            //negative number means we cannot pass.
            return -1;
        }
        //easy way, but maybe we could change it a little if it gives strange numbers.
        float cost = 5F * distance;
        return (int)cost;
    }
}
