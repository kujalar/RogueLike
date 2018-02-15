using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToRandomDungeon : ScenePortal
{
    public int level;
    
    public override void ActivatePortal(Creature activator)
    {
        GameManager.instance.SetLevel(level);
        base.ActivatePortal(activator);
    }

}

public abstract class ScenePortal : MonoBehaviour {
    public string toScene;
    public Vector2Int spawnPoint;

    //this method is called when configured ScenePortal is activated
    public virtual void ActivatePortal(Creature activator)
    {
        GameManager.instance.playerSpawnPoint.SetLocation(spawnPoint);
        //how to change scene? now just make it static
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        SceneManager.LoadScene(toScene);
        //SceneManager.LoadScene(target.buildIndex, LoadSceneMode.Single);
        
    }
}


