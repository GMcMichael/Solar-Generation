//using Mirror;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class NetworkedPlayerSetup : MonoBehaviour
{
    /*[SerializeField]
    Behaviour[] disabledComponents;

    [SerializeField]
    private string remoteLayer = "RemotePlayer";
    [SerializeField]
    private GameObject[] relayer;

    private Camera sceneCamera;

    private void Start() 
    {
        if(!isLocalPlayer) {
            DisableComponents();
            AssignLayer();
        } else {
            sceneCamera = Camera.main;
            if(sceneCamera != null) sceneCamera.gameObject.SetActive(false);
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netID, player);
    }

    void AssignLayer() {
        foreach (GameObject obj in relayer)
        {
            obj.layer = LayerMask.NameToLayer(remoteLayer);
        }
    }

    void DisableComponents() {
        for(int i = 0; i < disabledComponents.Length; i++) {
                disabledComponents[i].enabled = false;
            }
    }

    void OnDisable() {//Is called when objectis destroyed
        Cursor.lockState = CursorLockMode.None;
        if(sceneCamera != null) sceneCamera.gameObject.SetActive(true);
        GameManager.UnRegisterPlayer(transform.name);
    }*/
}
