using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public GameObject levelImage;
    public BoardManager boardManager;

	void Awake () {
        Debug.Log(this.name + " on herännyt ruususen unesta!");
		if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        Debug.Log("Assign level image "+levelImage);
        GameManager.instance.levelImage = levelImage;
	}

}
