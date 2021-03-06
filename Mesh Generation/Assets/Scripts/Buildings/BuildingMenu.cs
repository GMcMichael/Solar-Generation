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
    private int buildDistance = 30;
    [SerializeField]
    private GameObject buildMenu;
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private GameObject[] displayPrefabs;
    [SerializeField]
    private Sprite[] prefabSprites;
    [SerializeField]
    private Image[] menuImages = new Image[7];
    private int currPrefab;
    private GameObject previewObject;
    [SerializeField]
    private OrientateToPlanet orientateToPlanet;
    [SerializeField]
    private int RotateSpeed = 100;

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

        int ScrollAmount = 0;
        if (Input.GetKeyDown(KeyCode.X) || (Input.GetAxis("Mouse ScrollWheel") > 0f)) ScrollAmount--;
        else if (Input.GetKeyDown(KeyCode.C) || (Input.GetAxis("Mouse ScrollWheel") < 0f)) ScrollAmount++;

        if(Input.GetKey(KeyCode.R)) {
            previewObject.transform.Rotate(previewObject.transform.up, ScrollAmount*RotateSpeed);
        } else {
            currPrefab += ScrollAmount;
            if (currPrefab < 0) currPrefab = prefabs.Length - 1;
            else if (currPrefab > prefabs.Length - 1) currPrefab = 0;
        }

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

    private void DisplayPreview() {
        if (displayed && !previewObject.name.Equals(prefabs[currPrefab].name + " Display(Clone)")) StopPreview();
        if (!displayed) {
            displayed = true;
            previewObject = Instantiate(displayPrefabs[currPrefab]);
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

    private void BuildPrefab()
    {
        //check requirements for building here
        if(!false) {//can build
            ToggleMenu();
            Instantiate(prefabs[currPrefab], previewObject.transform.position, previewObject.transform.rotation, orientateToPlanet.getCurrPlanet());
        }

    }
}
