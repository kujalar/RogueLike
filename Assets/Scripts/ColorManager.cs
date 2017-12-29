using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

	public Color actionColor;
	public Color bonusActionColor;
	public Color reactionColor;
	public Color legendaryColor;

	public static ColorManager instance = null;

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
