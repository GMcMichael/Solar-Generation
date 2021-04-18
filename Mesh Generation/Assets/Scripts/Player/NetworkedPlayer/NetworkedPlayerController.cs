//using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ConfigurableJoint))]
public class NetworkedPlayerController : MonoBehaviour
{
    /*[SerializeField]
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


    private PlayerMovement movement;
    private ConfigurableJoint joint;

    private bool cursorLocked = true;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        joint = GetComponent<ConfigurableJoint>();
        baseSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;

        setJointSettings(jointSpring);
    }

    void Update()
    {
        //Toggle lock of cursor
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            cursorLocked = !cursorLocked;
        }

        //calculate movment velocity as 3D vector
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        //check for sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed * sprintingMultiplier;
        }
        else
        {
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
        if (Input.GetButton("Jump"))
        {//change jetpack key later so you can jump and fly

            _jetpackForce = Vector3.up * jetpackForce;
            setJointSettings(0);
        }
        else
        {
            setJointSettings(jointSpring);
        }
        movement.SetJetpackForce(_jetpackForce);
    }

    private void setJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }*/
}
