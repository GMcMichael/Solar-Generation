using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(OrientateToPlanet))]
public class PlayerController : MonoBehaviour
{
    
    [SerializeField]
    private float speed = 10f;
    private float baseSpeed;
    [SerializeField]
    private float sprintingMultiplier = 2f;
    [SerializeField]
    private float mouseSensitivity = 10f;

    [SerializeField]
    private float jetpackForce = 1000f;

    [Header("Joint Settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 50f;
    private Transform planet;
    [SerializeField]
    private LayerMask jointConnectMask;
    [SerializeField]
    private int floatHeight = 2;

    private PlayerMovement movement;
    private ConfigurableJoint joint;

    private bool cursorLocked = true;

    void Start() {
        movement = GetComponent<PlayerMovement>();
        joint = GetComponent<ConfigurableJoint>();
        baseSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;

        setJointSettings(jointSpring);
    }

    void Update() {
        //Toggle lock of cursor
        if(Input.GetKeyDown(KeyCode.L)) {
            if(cursorLocked) {
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
            cursorLocked = !cursorLocked;
        } else if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();//close the game

        //raycast for joint height and interacting
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, jointConnectMask))
        {
            //set joint connectedAnchor to just above hit position
            joint.connectedAnchor = hit.point + ((transform.position - hit.point).normalized) * floatHeight;
        }

        //calculate movment velocity as 3D vector
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        //check for sprinting
        if(Input.GetKey(KeyCode.LeftShift)) {
            speed = baseSpeed * sprintingMultiplier;
        } else {
            speed = baseSpeed;
        }

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;
        //movement vector
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;//normilized causes the result to always be 1 so the actual speed is only dictated by the speed variable

        //send movement to movement script
        movement.SetVelocity(velocity);

        //get horizontal rotation as a 3d vector
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * mouseSensitivity;

        //send rotation to movement script
        movement.SetRotation(rotation);

        //get vertical rotation as a 3d vector
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * mouseSensitivity;

        //send camera rotation to movement script
        movement.SetCameraRotation(cameraRotationX);

        //apply jetpack force
        Vector3 _jetpackForce = Vector3.zero;
        //check for jumping
        if(Input.GetButton("Jump")) {//change jetpack key later so you can jump and fly

            _jetpackForce = joint.connectedAnchor.normalized * jetpackForce;
            setJointSettings(0);
        } else {
            setJointSettings(jointSpring);
        }
        movement.SetJetpackForce(_jetpackForce);
    }

    private void setJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
        joint.xDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
        joint.zDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

    public void SetPlanet(Transform _planet)
    {
        planet = _planet;
    }

}
