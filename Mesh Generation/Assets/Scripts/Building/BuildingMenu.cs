using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    private bool open = false;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private LayerMask buildMask;
    private bool displayed;
    [SerializeField]
    private int buildDistance = 10;
    [SerializeField]
    private GameObject buildMenu;
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private Sprite[] prefabSprites;
    [SerializeField]
    private Image[] menuImages = new Image[7];
    private int currPrefab;
    private GameObject previewObject;

    // Start is called before the first frame update
    void Start()
    {
        currPrefab = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) ToggleMenu();
        if (!open) {
            if (displayed) StopPreview();
            return;
        }

        if (Input.GetKeyDown(KeyCode.X) || (Input.GetAxis("Mouse ScrollWheel") > 0f)) currPrefab--;
        else if (Input.GetKeyDown(KeyCode.C) || (Input.GetAxis("Mouse ScrollWheel") < 0f)) currPrefab++;
        
        if (currPrefab < 0) currPrefab = prefabs.Length - 1;
        else if (currPrefab > prefabs.Length - 1) currPrefab = 0;

        UpdateMenu();

        DisplayPreview();

        if (Input.GetMouseButtonDown(0)) BuildPrefab();
        else if (Input.GetMouseButtonDown(1)) ToggleMenu();
    }

    private void ToggleMenu()
    {
        open = !open;
        buildMenu.SetActive(open);
    }

    private void UpdateMenu()
    {
        int sidePrefab = currPrefab;
        for (int i = 0; i < 3; i++)
        {
            sidePrefab--;
            if (sidePrefab < 0) sidePrefab = prefabs.Length - 1;
            else if (sidePrefab > prefabs.Length - 1) sidePrefab = 0;
        }
        for(int i = 0; i < menuImages.Length-1; i++)
        {
            menuImages[i].sprite = prefabSprites[sidePrefab];
            sidePrefab++;
            if (sidePrefab < 0) sidePrefab = prefabs.Length - 1;
            else if (sidePrefab > prefabs.Length - 1) sidePrefab = 0;
        }
    }

    private void DisplayPreview()//to build object get the rotation of the preview and then make a real one.
    {//need to rotate the object somehow here
        if (displayed && !previewObject.name.Equals(prefabs[currPrefab].name + "(Clone)")) StopPreview();
        if (!displayed) {
            displayed = true;
            previewObject = Instantiate(prefabs[currPrefab]);
            previewObject.GetComponent<Collider>().enabled = false;
        }
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, buildDistance, buildMask)) previewObject.transform.position = hit.point;
        else if(displayed) StopPreview();
    }

    private void StopPreview()
    {
        Destroy(previewObject);
        displayed = false;
    }

    private void BuildPrefab()//check requirements for building here
    {
        
    }
}
