using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField]
    private float thrustForce = 10;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float speedLimit = 200;
    private bool driving;
    [SerializeField]
    private GameObject vehicleCamera;
    private OrientateToPlanet orientateToPlanet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //orientateToPlanet = GetComponent<OrientateToPlanet>();
    }

    void Update() {
        if(!driving) return;
        float thrust = Input.GetAxisRaw("Vertical");//ws is thrust
        float xRotation = Input.GetAxisRaw("Mouse Y");//mouse up/down should be nose up and down
        float yRotation = Input.GetAxisRaw("Mouse X");//mouse left/right should be nose left/right
        float zRotation = Input.GetAxisRaw("Horizontal");//ad is roll
        AddForce(thrust);
        Rotate(xRotation, yRotation, -zRotation);
    }

    private void LimitVelocity() {
        float x = Mathf.Clamp(rb.velocity.x, -speedLimit, speedLimit);
        float y = Mathf.Clamp(rb.velocity.y, -speedLimit, speedLimit);
        float z = Mathf.Clamp(rb.velocity.z, -speedLimit, speedLimit);

        rb.velocity = new Vector3(x, y, z);
    }

    private void AddForce(float force) {
        Vector3 velocity = transform.forward * force * thrustForce;
        rb.AddForce(velocity);
    }

    private void Rotate(float xAmount, float yAmount, float zAmount) {
        xAmount *= rotationSpeed;
        yAmount *= rotationSpeed;
        zAmount *= rotationSpeed;
        transform.Rotate(xAmount, yAmount, zAmount);
    }

    public void SetDriving(bool x) {
        driving = x;
        vehicleCamera.SetActive(x);
        //orientateToPlanet.setDisableRotation(x);
    }

    public void Land() {//Should change this to be handled from the vehicle here instead of the player

    }
}
