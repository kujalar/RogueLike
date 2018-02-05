using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChart : MonoBehaviour {

    public static TerrainChart instance;

    private int[,] conversionMatrix = { {7,6,5}, {4,9,3}, {2,1,0} };
    private void Awake()
    {
        instance = this;
    }

    private int ConvertToNumber(int dirX,int dirY)
    {
        return conversionMatrix[dirX+1,dirY+1];
    }

    public int GetCostToEnter(DataEntry targetData,int dirX, int dirY,float distance, SpeedType speedType)
    {
        //TODO tänne voisi laittaa monta vaihtehtoista speedtypeä ja niistä sitten valitaan se mikä on halvin liikkujalle, ja käytetään sitä.
        char code;
        int index = ConvertToNumber(dirX, dirY);
        Debug.Log(dirX + "," + dirY + "=>" + index);

        if (targetData != null && targetData.code != null && index < targetData.code.Length)
        {
            code = targetData.code[index];
        } else
        {
            code = '_';//this means clear
        }

        Debug.Log("distance="+distance+" and movecode="+code);

        //now if we got code S then it means solid wall, no pass. This might be made better too. S is also used in drawing the maps
        if (code == 'X')
        {
            //negative number means we cannot pass.
            return -1;
        }
        //easy way, but maybe we could change it a little if it gives strange numbers.
        float cost = 5F * distance;
        return (int)cost;
    }
}
