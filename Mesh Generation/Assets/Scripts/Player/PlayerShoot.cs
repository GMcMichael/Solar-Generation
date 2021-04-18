using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
    private float fireTimer;
    //private static float deltaTime = 0.02f;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private LayerMask mask;

    void Start() {
        if(playerCamera == null) {
            Debug.Log("PlayerShoot: No Camera");
            this.enabled = false;
        }
    }

    void Update() {
        if(!weapon.getCanShoot()) {
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0) weapon.Cooldown();
        } else {
            if(Input.GetButtonDown("Fire1")) {
                weapon.Shot();
                fireTimer = weapon.getFireRate();
                Shoot();
            }
        }
    }

    void Shoot() {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weapon.getRange(), mask)) {
            if(hit.collider.tag == PLAYER_TAG) {
                //CmdPlayerHit(hit.transform.root.gameObject.transform.name, weapon);
            }
        }
    }

}
