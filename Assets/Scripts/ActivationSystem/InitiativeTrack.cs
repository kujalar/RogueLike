using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeTrack : MonoBehaviour {
    public float turnDelay = .1f;

    //  private List<ActorsListItem> actors = new List<ActorsListItem>();
    private LinkedList<ActorsListItem> actors = new LinkedList<ActorsListItem>();
    LinkedListNode<ActorsListItem> currentNode = null;

    public void Clear()
    {
        currentNode = null;
        actors.Clear();
    }

    public void Register(ActorObject actor)
    {
        ActorsListItem listItem = new ActorsListItem();
        listItem.initiativeCount = actor.RollInitiative();
        listItem.actor = actor;

        int actorsCount = InsertListItem(listItem);

        Debug.Log("Registered actorsCount = "+actorsCount );
    }
    //returns the index of a new listItem at the actors list.
    protected int InsertListItem(ActorsListItem listItem)
    {
        LinkedListNode<ActorsListItem> node = actors.First;
        while (node != null)
        {
            int currentInitCount = node.Value.initiativeCount;
            if (listItem.initiativeCount > currentInitCount)
            {
                actors.AddBefore(node, listItem);
                return actors.Count;
            }
            node = node.Next;
        }
        
        actors.AddLast(listItem);
        return actors.Count;
    }

    public void StartRound()
    {
        Debug.Log("Start round");
        //TODO we could do some turn advancement logic here
        currentNode = actors.First;
        //currentNode.Value.actor.StartTurn();
        StartCoroutine(SignalStartTurn(currentNode.Value.actor));
    }

    public void NextTurn(ActorObject lastActor)
    {
        //lastActor can be used as a errorCheck value, if all goes well we do not need it.
        currentNode = currentNode.Next;
        //null means everyone has made action on their turn and we can start a new turn.
        if (currentNode == null)
        {
            //everyone has moved, so start whole next round
            StartRound();
        } else
        {
            //we can advance with next turn in initiative track
            //currentNode.Value.actor.StartTurn();
            StartCoroutine(SignalStartTurn(currentNode.Value.actor));
        }
    }
    protected IEnumerator SignalStartTurn(ActorObject actor)
    {
        yield return new WaitForSeconds(turnDelay);
        actor.StartTurn();
    }
    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/
    protected class ActorsListItem
    {
        public int initiativeCount = 0;
        public ActorObject actor = null;
        public bool temporial = false;//with this we can signal if the item is removed from the list.
    }
}



public interface ActorObject
{
    void StartTurn();
    int RollInitiative();
}

