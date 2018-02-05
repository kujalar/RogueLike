using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour {

    public static GridScript instance;

    public DataTilemap dataMap;
  
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public DataEntry GetDataFromWorld(Vector3 worldPoint)
    {
        return dataMap.GetDataEntryFromWorld(worldPoint);
    }
}
