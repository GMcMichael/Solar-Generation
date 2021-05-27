using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBuilding : MonoBehaviour
{
    private GameObject[] nodes;
    [SerializeField]
    private int SearchRange = 100;
    [SerializeField]
    private ResourceManager[] selectedNodes;
    private int currNodes = 0;
    private bool active = true;
    private int currMiners = 0;
    [SerializeField]
    private Miner[] Miners;
    [SerializeField]
    private int inventory;//need to change this to handle multiple types
    [SerializeField]
    private LayerMask planetMask;
    [SerializeField]
    private int minerOffset;

    void Awake() {
        nodes = transform.root.GetComponent<ObjectGeneration>().GetResources();
        selectedNodes = new ResourceManager[0];
        Transform closeNode = null;
        float nodeDist = float.MaxValue;
        foreach (var node in nodes)
        {
            float Dist = Vector3.Distance(node.transform.position, transform.position);
            if(Dist < SearchRange && Dist < nodeDist) {
                if(!node.GetComponent<ResourceManager>().isMining())
                    closeNode = node.transform;
            }
        }
        if(closeNode != null) {
            ResourceManager nodeManager = closeNode.GetComponent<ResourceManager>();
            selectedNodes = nodeManager.GetNodeGroup();
            nodeManager.SetMining(true);
        } else {
            //drop resources in temp box and destory building
            Debug.Log("No available node group in range (" + SearchRange + ")");
        }
    }

    void Update()
    {
        if(!active || Miners.Length <= 0) return;
        if(currNodes < selectedNodes.Length && currMiners < Miners.Length) {//if not all nodes are being mined and there are miners available
            SendOutMiner(GetFreeMiner());
        }
    }

    private void SendOutMiner(Miner miner) {
        if(miner == null) {
            Debug.LogError("Miner building attempting to send out null miner.");
            return;
        }
        currMiners++;
        currNodes++;
        //drop miner to planet
        //raycast down from miner pos to planet pos
        RaycastHit hit;
        if(Physics.Raycast(miner.transform.position, -transform.up, out hit, planetMask)) {
            miner.transform.position = hit.point + (transform.up* minerOffset);
        }
        ResourceManager node = GetFreeNode();
        if (node != null) miner.Activate(node);
    }

    public void ReciveMiner(Miner miner, int inventoryAmount) {
        currMiners--;
        currNodes--;
        inventory += inventoryAmount;
        miner.transform.position = miner.transform.parent.position;
        miner.transform.rotation = Quaternion.identity;
    }

    private void Activate() {
        if(selectedNodes == null || selectedNodes.Length == 0) return;
        active = true;
    }

    private Miner GetFreeMiner() {
        foreach (Miner miner in Miners)
        {
            if(!miner.isActive()) return miner;
        }
        return null;
    }

    private ResourceManager GetFreeNode() {
        foreach (ResourceManager node in selectedNodes)
        {
            if(!node.isMining()) return node;
        }
        return null;
    }
}
