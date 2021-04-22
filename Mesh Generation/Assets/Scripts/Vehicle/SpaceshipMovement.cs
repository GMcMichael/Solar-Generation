using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float thrustForce = 10;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float speedSpace = 200;
    [SerializeField]
    private float speedPlanet= 50;
    [SerializeField]
    private float speedLimit;
    private bool driving;
    [SerializeField]
    private GameObject vehicleCamera;
    private OrientateToPlanet orientateToPlanet;
    [SerializeField]
    private LayerMask landingMask;
    [SerializeField]
    private float landingOffset;
    [SerializeField]
    private int landDist = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientateToPlanet = GetComponent<OrientateToPlanet>();
        speedLimit = speedSpace;
    }

    void Update() {//Should maybe be in fixed update?
        if(!driving) return;
        float thrust = Input.GetAxisRaw("Vertical");//ws is thrust
        float xRotation = Input.GetAxisRaw("Mouse Y");//mouse up/down should be nose up and down
        float yRotation = Input.GetAxisRaw("Mouse X");//mouse left/right should be nose left/right
        float zRotation = Input.GetAxisRaw("Horizontal");//ad is roll
        AddForce(thrust);
        Rotate(xRotation, yRotation, -zRotation);
        LimitVelocity(speedLimit);
        if(Input.GetKeyDown(KeyCode.F)) LimitVelocity(0);//Change key later
    }

    void FixedUpdate() {
        speed = rb.velocity.magnitude;
        velocity = rb.velocity;
    }

    private void LimitVelocity(float limit) {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, limit);
    }

    private void AddForce(float force) {
        Vector3 velocity = transform.forward * force * thrustForce;
        rb.AddForce(velocity);
    }

    private void Rotate(float xAmount, float yAmount, float zAmount) {
        xAmount *= rotationSpeed;
        yAmount *= rotationSpeed;
        zAmount *= rotationSpeed;
        transform.Rotate(xAmount, yAmount, zAmount);//make this addTorque to the rigidbody
    }

    public void SetDriving(bool x) {
        driving = x;
        vehicleCamera.SetActive(x);
        orientateToPlanet.setDisableRotation(x);
        rb.isKinematic = !x;
    }

    public void Land() {//Should change this to be handled from the vehicle here instead of the player
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
        LimitVelocity(0);
    }

    public void OnPlanet(Transform planet) {
        if(planet == null) speedLimit = speedSpace;
        else {
            speedLimit = speedPlanet;
        }
    }
}
