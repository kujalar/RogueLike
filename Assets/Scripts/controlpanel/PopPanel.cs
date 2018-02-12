using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopPanel : MonoBehaviour {
    bool listenMouse = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!listenMouse)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            PopPanel popPanel = GetComponent<PopPanel>();
            Vector3 mousepos = Input.mousePosition;

            popPanel.transform.position = mousepos;
        }
    }
    public void StartListenMouse()
    {
        //Debug.Log("Start Listen Mouse");
        listenMouse = true;
    }
    public void StopListeningMouse()
    {
        //Debug.Log("Stop Listen Mouse");
        listenMouse = false;
    }
}
