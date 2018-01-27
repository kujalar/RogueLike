using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallMap : MonoBehaviour {
    int counter = 0;
    private void Awake()
    {
        
    }

    public RogueLikeTile getTileFromWorld(Vector3 worldPoint)
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        Vector3Int cell = tilemap.WorldToCell(worldPoint);

        TileBase tilebase = tilemap.GetTile(cell);
        RogueLikeTile roguetile = null;

        if (tilebase != null)
        {
            counter++;
            roguetile = tilebase as RogueLikeTile;
            Debug.Log("Worldpoint(" + worldPoint + ") code="+roguetile.GetCode()+" nameFound=" + tilebase.name + " " + counter);

        }
        return roguetile;
    }
}
