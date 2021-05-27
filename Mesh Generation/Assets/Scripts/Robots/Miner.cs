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
    private float speed;
    [SerializeField]
    private Transform building;
    private MinerBuilding buildingScript;
    private Transform target;
    [SerializeField]
    private int maxDistToTarget = 3;
    [SerializeField]
    private LayerMask planetMask;
    [SerializeField]
    private float MovingOffset;
    
    void Awake() {
        buildingScript = building.GetComponent<MinerBuilding>();
    }

    void FixedUpdate()
    {
        if(!active) return;
        if(Vector3.Distance(transform.position, target.position) > maxDistToTarget) {//not at target
            Move();
        } else {//at target
            if(mining) {
                //mine
                inventory++;//this is what mines, change later to use the the nodes own script. Need timer to mine only so fast
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
        //move to planet surface
        RaycastHit hit;
        if(Physics.Raycast(transform.position - (transform.up*2), -transform.up, out hit, planetMask)) {
            transform.position = hit.point + (transform.up*MovingOffset);
        }
        //look at target
        transform.LookAt(target);//may be wrong
        //move with transform.forward to target
        transform.Translate(transform.forward * speed, Space.World);//may be wrong
    }

    private void SetTarget() {
        if(mining) {
            node.SetMining(true);
            target = node.transform;
            inventoryResource = node.GetResourceType();
        }
        else {
            node.SetMining(false);
            target = building;
        }
    }
    public bool isActive() {
        return active;
    }

    public void Activate(ResourceManager nextNode) {
        if(nextNode == null) {
            Debug.LogError("Miner is reciving orders for null node ResourceManager.");
            return;
        }
        node = nextNode;
        active = true;
        mining = true;
        SetTarget();
    }
}
