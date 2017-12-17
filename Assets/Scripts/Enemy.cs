using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject, ActorObject
{

    public String name;
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    protected void Awake()
    {
        //add our object to initiativetrack
        GameManager.instance.initiativeTrack.Register(this);
    }
	// Use this for initialization
	protected override void Start () {
        //GameManager.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    public int RollInitiative()
    {
        //this should be made random, but now i can test like this.
        return 6;
    }
    public void StartTurn()
    { 
        MoveEnemy();
        //TODO you might want to place some delay to passing initiative. Now all the objects will get their turn before 
        //turn passing objects smoothMovement script etc is ready.
        GameManager.instance.initiativeTrack.NextTurn(this);
    }


    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        //Check if we are on same X column, and then move up or down.
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        } else
        { //otherwise move right or left
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
    private void OnMouseEnter()
    {
        CanvasController.instance.setMouseEnterText(name);
    }
}
