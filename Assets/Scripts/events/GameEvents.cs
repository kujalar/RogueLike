using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour {

    public delegate void RefreshSelectedCreatureDisplay();
    public event RefreshSelectedCreatureDisplay OnRefreshSelectedCreatureDisplay;
    public delegate void RefreshMovePointDisplay();
    public event RefreshMovePointDisplay OnRefreshMovePointDisplay;
    public delegate void RefreshActionDisplay();
    public event RefreshActionDisplay OnRefreshActionDisplay;

    public static GameEvents instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void FireRefreshSelectedCreature(GameObject source)
    {
        if (GameManager.instance.getSelectedCreature().gameObject != source)
        {
            return;
        }
        FireRefreshSelectedCreature();
    }
    public void FireRefreshSelectedCreature()
    {
        if(OnRefreshSelectedCreatureDisplay != null)
        {
            OnRefreshSelectedCreatureDisplay();
        }
    }

    public void FireRefreshActionDisplay(GameObject source)
    {
        if (GameManager.instance.getSelectedCreature().gameObject != source)
        {
            return;
        }

        if (OnRefreshActionDisplay != null)
        {
            OnRefreshActionDisplay();
        }
    }

    public void FireRefreshMovePointDisplay(GameObject source)
    {
        //TODO this is a bit complicated...
        if (GameManager.instance.getSelectedCreature().gameObject != source)
        {
            return;
        }
        if (OnRefreshMovePointDisplay != null)
        {
            OnRefreshMovePointDisplay();
        }
    }
}
