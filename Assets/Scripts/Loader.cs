using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public GameObject levelImage;

	void Awake () {
		if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        
        GameManager.instance.levelImage = levelImage;
        
	}

}
