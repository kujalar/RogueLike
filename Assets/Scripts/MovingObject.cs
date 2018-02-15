using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {
    public float moveTime = 1f; //one is a pretty good default with current logix
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;


    //this parameter tells if we are doing smoothMovement animation
    protected bool isBusy = false;
	// Use this for initialization
	protected virtual void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}
	
    protected bool Move (int xDir, int yDir, Speedometer speedometer, out RaycastHit2D hit, bool freeMoveMode)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        float distance = Vector2.Distance(start, end);

        bool weHaveEnoughSpeed = true;
        //calculate move cost
        //check the target wallmap
        //TODO here we could change what kind of systems work, now I make just a statement to delete what is not implemented in scene.
        //RogueLikeTile roguetile = null;
        DataEntry data = null;
        if (GridScript.instance != null)
        {
            //roguetile = GridScript.instance.wallMap.getTileFromWorld(new Vector3(end.x, end.y, 0F));
            data = GridScript.instance.GetDataFromWorld(new Vector3(end.x, end.y, 0F));
        }
        int moveCost = TerrainChart.instance.GetCostToEnter(data,xDir,yDir,distance,SpeedType.LAND);

        //some default
        float effectiveMoveSpeed = 1;

        

        //if we have speedometer, we should take the terrain and reduce the speed as we now have no terrain
        //and we always move only 1 move we reduce the distance of one square and it means speed of 5
        if (speedometer != null && moveCost >= 0)
        {
            //in freeMoveMode we do not pay movement allowance in movepoints but in time, and for that we need effective speed compared to terrain
            effectiveMoveSpeed = speedometer.GetSpeed(SpeedType.LAND).GetCurrentMaxSpeed() / (moveTime * moveCost);
            Debug.Log("effectiveMoveSpeed = " + effectiveMoveSpeed);

            //we pay the cost in movementpoints if we are not in freeMoveMode
            if (!freeMoveMode)
            {
                weHaveEnoughSpeed = speedometer.PayMovementAllowance(moveCost, SpeedType.LAND);
            }
        } else
        {
            weHaveEnoughSpeed = false;
        }
        //Debug.Log("Heipparallaa");

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null && weHaveEnoughSpeed)
        {
            StartCoroutine(SmoothMovement(end, effectiveMoveSpeed));
            return true;
        }
        return false;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit,bool freeMove)
    {
        //this is a default move where we do not have a speedometer
        return Move(xDir, yDir, null, out hit,freeMove);
        /*
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        Debug.Log("Heipparallaa");

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;*/
    }

    protected IEnumerator SmoothMovement(Vector3 end,float stepSpeed)
    {
        isBusy = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, stepSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        isBusy = false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir,bool freeMove)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit,freeMove);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
