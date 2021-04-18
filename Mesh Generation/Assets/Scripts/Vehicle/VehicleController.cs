using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OrientateToPlanet))]
public class VehicleController : MonoBehaviour
{
    [SerializeField]
    private bool driving = false;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float turnSpeed = 10f;
    [SerializeField]
    private int landDist = 5;
    private Vector3 velocity;
    private Vector3 rotation;
    private Rigidbody rb;
    [SerializeField]
    private LayerMask landingMask;
    [SerializeField]
    private float landingOffset;
    private OrientateToPlanet orientateToPlanet;
    [SerializeField]
    private GameObject vehicleCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientateToPlanet = GetComponent<OrientateToPlanet>();
    }

    void Update()
    {
        if (!driving) return;

        //calculate movment velocity as 3D vector
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 moveVertical = transform.forward * zMove;
        //movement vector
        velocity = moveVertical.normalized * speed;

        //get horizontal and vertical rotation as a 3d vector
        float zRotation = Input.GetAxisRaw("Horizontal");
        float yRotation = Input.GetAxisRaw("Mouse X");
        float xRotation = Input.GetAxisRaw("Mouse Y");
        rotation = new Vector3(xRotation, yRotation, -zRotation) * turnSpeed;//xRotation in here might be wrong
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (velocity != Vector3.zero)
        {
            //moves the rigidbody while checking for physics interactions
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    private void Rotate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    public void SetDriving(bool x)//maybe disable the player when they are driving, or replace the joint
    {
        driving = x;
        orientateToPlanet.setDisableRotation(x);
        vehicleCamera.SetActive(x);
    }

    public void Land()
    {
        //raycast down, if it hits a planet within x# meters, clip to the planet and orientate
        RaycastHit hit;
        /*Transform currPlanet = orientateToPlanet.getCurrPlanet();
        Vector3 dir = (currPlanet.position - transform.position).normalized;
        Vector3 origin = currPlanet.position - (dir * 500);
        if (Physics.Raycast(origin, -dir, out hit, Mathf.Infinity, landingMask))*/
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, landDist, landingMask))
        {
            /*Debug.Log("Distance: " + Mathf.Abs(Vector3.Distance(hit.point, transform.position)));
            if(Mathf.Abs(Vector3.Distance(hit.point, transform.position)) > landDist) return;*/
            //snap to the planet offset for the landing legs
            transform.position = hit.point + ((transform.position - hit.point).normalized) * landingOffset;
        }
    }

}
