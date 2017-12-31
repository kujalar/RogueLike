using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour {

	public static IconManager instance = null;

	//TODO add a list of sprites for use as a different kind of icons, for example attack, dash, dodge ... etc option icons

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		} else if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
}
