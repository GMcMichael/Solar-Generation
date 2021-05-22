using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private enum ResourceType {iron, copper, stone, coal};
    [SerializeField]
    private ResourceType resourceType;
    [SerializeField]
    private int amountAdded = 1;
    private Transform[] nodeGroup;
    private bool mining;

    public void Interact(ResourceInventory inventory) {//need to add a timer for player mining
        inventory.ChangeResource((int)resourceType, amountAdded);
    }

    public void SetNodeGroup(Transform[] nodes) {
        nodeGroup = nodes;
    }

    public ResourceManager[] GetNodeGroup() {
        List<ResourceManager> nodes = new List<ResourceManager>();
        foreach (Transform node in nodeGroup)
        {
            nodes.Add(node.GetComponent<ResourceManager>());
        }
        return nodes.ToArray();
    }
    public void SetMining(bool x) {
        mining = x;
    }
    public bool isMining() {
        return mining;
    }

    public int GetResourceType() {
        return (int)resourceType;
    }
}
