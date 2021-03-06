using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currCameraRotationX = 0;
    private float jetpackForce = 0;

    [SerializeField]
    private float cameraRotationLimit = 65f;

    private Rigidbody playerRigidbody;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    //sets the velocity vector
    public void SetVelocity(Vector3 _velocity) {
        velocity = _velocity;
    }

    //sets the rotational vector
    public void SetRotation(Vector3 _rotation) {
        rotation = _rotation;
    }

    //sets the rotational vector
    public void SetCameraRotation(float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }

    //sets the jetpack force vector
    public void SetJetpackForce(float _jetpackForce) {
        jetpackForce = _jetpackForce;
    }

    //Runs every physics update
    void FixedUpdate() {//maybe use AddForce instead so it still used the rigidbody
        Move();
        Rotate();
    }

    //Move based on velocity
    void Move() {
        if(velocity != Vector3.zero) {
            //moves the rigidbody while checking for physics interactions
            //playerRigidbody.MovePosition(playerRigidbody.position + velocity * Time.fixedDeltaTime);
            transform.Translate(velocity/8, Space.World);
        }
        if(jetpackForce != 0) {
            playerRigidbody.AddForce(transform.up * jetpackForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    //rotate the player body and camera
    void Rotate() {
        //Right/Left Movement
        //playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(rotation));
        transform.Rotate(rotation);
        //Up/Down Movement
        if(playerCamera != null) {
            //set camera rotation and clamp it within the limits
            currCameraRotationX -= cameraRotationX;
            currCameraRotationX = Mathf.Clamp(currCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            //apply to transform of the camera
            playerCamera.transform.localEulerAngles = new Vector3(currCameraRotationX, 0f, 0f);
        }
    }

}
