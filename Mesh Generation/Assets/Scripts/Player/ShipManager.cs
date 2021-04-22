using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private int interactRange = 10;
    [SerializeField]
    private LayerMask shipMask;
    private Camera playerCamera;
    [SerializeField]
    private GameObject playerModel;//later, all player scrips should go on empty and I can just disable and enable the player
    [SerializeField]
    private GameObject playerHitbox;
    [SerializeField]
    private GameObject playerWeapon;
    [SerializeField]
    private Animation shipDoor;
    private bool doorDown;
    private bool usingVehicle;
    private Transform vehicleWheel;
    private SpaceshipMovement vehicle;
    private Transform lastVehicle;
    private OrientateToPlanet orientateToPlanet;

    [SerializeField]
    private Transform player;
    private ConfigurableJoint joint;

    private List<GameObject> warpObjects;
    [SerializeField]
    private int warpOffset = 50;

    void Awake()
    {
        player = GameObject.Find("Poppins").transform;
        joint = player.GetComponent<ConfigurableJoint>();
        playerCamera = player.GetComponentInChildren<Camera>();
        orientateToPlanet = player.GetComponent<OrientateToPlanet>();
        warpObjects = new List<GameObject>();
    }

    void Update() {//All movement should probably be moved to the vehicles controlling script
        RaycastHit hit;
        if (usingVehicle) {
            if (Input.GetKeyDown(KeyCode.E))//stop driving
            {
                vehicle.Land();//maybe have land return a bool and if it fails orientate to the ship instead of the planet
                //stop the script to control the vheicle 
                vehicle.SetDriving(false);
                //teleport player to vehicle
                player.position = lastVehicle.position;
                //enable the camera
                playerCamera.gameObject.SetActive(true);
                //enable the player model
                playerModel.SetActive(true);
                //enable the player hitboxes
                playerHitbox.SetActive(true);
                playerWeapon.SetActive(true);
                //resume the player movment
                usingVehicle = false;
                //unchild the player to the vehicle
                gameObject.transform.parent = null;
                //enable Orientate to planet
                orientateToPlanet.enabled = true;
                return;
            } else if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                WarpForward();
            }
            //set the joint position to the steering wheel for now
            joint.connectedAnchor = vehicleWheel.position;
            return;
        } 
        else if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, interactRange, shipMask)) {
            if (Input.GetKeyDown(KeyCode.E)) {
                string name = hit.collider.gameObject.name;
                if (name.Equals("Rear Door"))
                {
                    if (!shipDoor.isPlaying)
                    {
                        if (doorDown) shipDoor.Play("ShipDoor_FoldUp");
                        else shipDoor.Play("ShipDoor_FoldDown");
                        doorDown = !doorDown;
                    }
                }
                else if (name.Equals("Steering Wheel"))
                {
                    vehicle = GetVehicle(hit.collider.transform.root);
                    lastVehicle = hit.collider.transform;
                    //stop the player movment
                    usingVehicle = true;
                    //disable the camera
                    playerCamera.gameObject.SetActive(false);
                    //disable the player model
                    playerModel.SetActive(false);
                    //enable the player hitboxes
                    playerHitbox.SetActive(false);
                    playerWeapon.SetActive(false);
                    //activate the script to control the vheicle 
                    vehicle.SetDriving(true);
                    //get the steering wheel
                    vehicleWheel = hit.collider.transform;
                    //child the player to the vehicle
                    gameObject.transform.parent = vehicle.transform;
                    //disable Orientate to planet
                    orientateToPlanet.enabled = false;
                }
            }
        } 
        else if(Input.GetKeyDown(KeyCode.R))
        {
            player.position = lastVehicle.position;
        }
    }

    private SpaceshipMovement GetVehicle(Transform root) {
        if(!root.name.Equals("Spaceship")) root = root.Find("Spaceship");
        return root.GetComponent<SpaceshipMovement>();
    }

    private void WarpForward()//need to child the ship to the planet in some script
    {
        GameObject closeObject = GetCloseObject();
        if (closeObject == null) return;
        Vector3 Dir = (closeObject.transform.position - vehicle.transform.position).normalized;
        RaycastHit hit;
        Ray ray = new Ray(vehicle.transform.position, Dir);
        Vector3 newPosition = Vector3.zero;
        if(closeObject.GetComponent<MeshCollider>().Raycast(ray, out hit, Mathf.Infinity))
        {
            newPosition = hit.point + (-Dir * warpOffset);
        }
        vehicle.transform.position = newPosition;
        transform.position = newPosition;
    }

    private GameObject GetCloseObject()
    {
        GameObject closeObject = null;
        float closeDist = float.MaxValue;
        for (int i = 0; i < warpObjects.Count; i++)
        {
            Vector3 Dir = (warpObjects[i].transform.position - vehicle.transform.position).normalized;
            float dot = Vector3.Dot(Dir, vehicle.transform.forward);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            if (angle < 10)
            {
                float Dist = Vector3.Distance(vehicle.transform.position, warpObjects[i].transform.position);
                if (Dist < closeDist)
                {
                    closeDist = Dist;
                    closeObject = warpObjects[i];
                }
            }
        }
        return closeObject;
    }

    public void AddWarpObjects(GameObject[] newObjects)
    {
        warpObjects.AddRange(newObjects);
    }
}
