using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{

    private bool active = false;
    private ResourceManager node;
    private int inventoryResource;//make a resource class with the nodes resourceType enum so this can use it as well
    private int inventory;
    [SerializeField]
    private int inventoryMax = 100;//may need to update with testing
    private bool mining = false;
    private int miningSpeed;
    [SerializeField]
    private int speed;
    private Transform building;
    private MinerBuilding buildingScript;
    private Vector3 target;
    [SerializeField]
    private int maxDistToTarget = 3;
    
    void Update()
    {
        if(!active) return;
        if(Vector3.Distance(transform.position, target) > maxDistToTarget) {//not at target
            Move();
        } else {//at target
            if(mining) {
                //mine
                inventory++;//this is what mines, change later
                if(inventory >= inventoryMax) {
                    inventory = inventoryMax;
                    mining = false;
                    SetTarget();
                }
            } else {
                //back at building
                active = false;
                buildingScript.ReciveMiner(this, inventory);
                inventory = 0;
            }
        }
    }

    private void Move() {//attach orientate to planet script
        //look at target
        transform.LookAt(target);
        //move with transform.forward to target
        transform.Translate(transform.forward * speed, Space.Self);//may be wrong
    }

    private void SetTarget() {
        if(mining) {
            node.SetMining(true);
            target = node.transform.position;
            inventoryResource = node.GetResourceType();
        }
        else {
            node.SetMining(false);
            target = building.position;
        }
    }
    public bool isActive() {
        return active;
    }

    public void Activate(ResourceManager nextNode) {
        if(nextNode == null) {
            Debug.LogError("Miner is reciving orders for null resource node.");
            return;
        }
        node = nextNode;
        active = true;
        mining = true;
        SetTarget();
    }
}
