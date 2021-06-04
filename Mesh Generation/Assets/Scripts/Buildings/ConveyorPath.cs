using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class ConveyorPath : MonoBehaviour//spliters and mergers should have an array of paths that can merge to one or choose to send an item down
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private List<Transform> pathNodes;//should be added to when conveyors are made (when making a conveyor belt, add a node every so often(always same distance) to travel to)
    private List<bool> nodeFull;
    public bool setpath = false;

    void Awake() {//maybe make this use vector3s instead
        SetPath();
    }

    void FixedUpdate() {
        if(setpath) {
            SetPath();
            setpath = false;
        }
    }

    public IEnumerator<Transform> GetNextNode(PathFollowing obj) {
        int currNode = 0;
        nodeFull[currNode] = true;
        //make sure path has nodes
        if(pathNodes == null || pathNodes.Count <= 1) yield break;

        while(true) {
            //returns curr point in pathNodes
            yield return pathNodes[currNode];

            //moves path point to next path
            if(currNode < pathNodes.Count - 1) 
                if(!nodeFull[currNode+1]) {
                    Move(currNode++);//currNode--; for reverse
                    obj.SetCurrNode(currNode);
                }
        }
    }

    public void Move(int nodePos, bool removed = false) {
        nodeFull[nodePos] = false;
        if(removed) return;//removed form conveyor
        nodeFull[nodePos+1] = true;
    }

    public float GetSpeed() {
        return speed;
    }

    public void SetPath() {
        List<Transform> newPath = new List<Transform>(GetComponentsInChildren<Transform>());
        newPath.RemoveAt(0);
        pathNodes = newPath;
        nodeFull = new List<bool>();
        for (int i = 0; i < newPath.Count; i++)
        {
            nodeFull.Add(false);
        }
    }

    public void AddPathNodes(List<Transform> newNodes) {//called when new conveyors are made
        pathNodes.AddRange(newNodes);
        for (int i = 0; i < newNodes.Count; i++)
        {
            nodeFull.Add(false);
        }
    }

    public void MergePaths(ConveyorPath _path) {
        //add the nodes to this one and remove the other script/gameobject
    }

    public void RemovePathNodes(List<Transform> oldNodes) {
        //need to split into two paths
        //create new ConveyorPath object
        //add other nodes to conveyorPath
    }
}