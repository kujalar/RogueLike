using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance = null;

    public  Text mouseEnterText;

	// Use this for initialization
	void Awake () {
        instance = this;
    }
	
	public void setMouseEnterText(string text)
    {
        mouseEnterText.text = text;
    }

    
}
