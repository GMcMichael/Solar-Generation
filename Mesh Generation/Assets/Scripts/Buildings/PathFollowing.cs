using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFollowing : MonoBehaviour
{
    private enum Move {
        Step,
        Lerp
    }

    [SerializeField]
    private Move move = Move.Step;
    [SerializeField]
    private ConveyorPath path;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float MaxDistToNode = .1f;
    private IEnumerator<Transform> nodeInPath;
    private int currNode;
    private bool onConveyor;
    public bool RemoveFromBelt;

    void Start() {
        SetUpPath();
    }

    void FixedUpdate() {
        if(RemoveFromBelt) {
            LeaveConveyor();
            RemoveFromBelt = false;
        }
        if(!onConveyor) return;
        if(nodeInPath == null || nodeInPath.Current == null)
            if(!SetUpPath()) return;
        
        if(move == Move.Step) {
            transform.position = Vector3.MoveTowards(transform.position,
                                                    nodeInPath.Current.position,
                                                    Time.fixedDeltaTime * speed);
        } else if(move == Move.Lerp) {
            transform.position = Vector3.Lerp(transform.position,
                                            nodeInPath.Current.position,
                                            Time.fixedDeltaTime * speed);
        }
        
        if(Vector3.Distance(transform.position, nodeInPath.Current.position) < MaxDistToNode)
            nodeInPath.MoveNext();
    }

    private bool SetUpPath() {
        if(path == null) {
            Debug.LogError("Path is null");
            return false;
        }
        speed = path.GetSpeed();
        //reference to coroutine
        nodeInPath = path.GetNextNode(this);

        //Gets next node
        nodeInPath.MoveNext();

        if(nodeInPath.Current == null) {
            Debug.LogError("Path has no nodes to follow", gameObject);
            return false;
        }
        
        transform.position = nodeInPath.Current.position;
        onConveyor = true;
        return true;
    }

    public void SetCurrNode(int nodeIdx) {
        currNode = nodeIdx;
    }

    private void LeaveConveyor() {
        path.Move(currNode, true);//leave path
        nodeInPath.Dispose();//end IEnumerator
        onConveyor = false;
    }
}
