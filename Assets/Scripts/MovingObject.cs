using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {
    public float moveTime = 0.1f;
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
	
    protected bool Move (int xDir, int yDir, Speedometer speedometer, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        //if we have speedometer, we should take the terrain and reduce the speed as we now have no terrain
        //and we always move only 1 move we reduce the distance of one square and it means speed of 5
        bool weHaveEnoughSpeed = true;
        if (speedometer != null)
        {
            weHaveEnoughSpeed = speedometer.PayMovementAllowance(5, SpeedType.LAND);
        }
        //Debug.Log("Heipparallaa");

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null && weHaveEnoughSpeed)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        
        return Move(xDir, yDir, null, out hit);
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

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isBusy = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while(sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        isBusy = false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

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
