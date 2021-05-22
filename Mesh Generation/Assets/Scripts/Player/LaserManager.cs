using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private float maxDistance = 100f;
    [SerializeField]
    private LayerMask interactMask;
    [SerializeField]
    private LineRenderer lr;
    private ResourceInventory inventory;

    void Start() {
        inventory = GetComponent<ResourceInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) EnableLaser();
        if (Input.GetMouseButton(0)) UpdateLaser();
        if (Input.GetMouseButtonUp(0)) DisableLaser();
    }

    private void EnableLaser()
    {
        laser.SetActive(true);
    }

    private void DisableLaser()
    {
        laser.SetActive(false);
    }

    private void UpdateLaser()
    {
        lr.SetPosition(0, laser.transform.position);
        RaycastHit hit;
        if(Physics.Raycast(laser.transform.position, laser.transform.forward, out hit, maxDistance))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
            if (interactMask == (interactMask | (1 << hit.transform.gameObject.layer)))
            {
                hit.transform.parent.GetComponent<ResourceManager>().Interact(inventory);
            }
        } else
        {
            lr.SetPosition(1, laser.transform.position + (laser.transform.forward * maxDistance));
        }
    }
}
