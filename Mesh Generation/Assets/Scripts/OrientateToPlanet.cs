﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OrientateToPlanet : MonoBehaviour//maybe need to change how the orientations works (maybe just make the object perpendicular to where it hits the mesh)
{
    private static List<Transform> planets;
    private Transform currPlanet;
    public static int distance = 1000;//the distance is to small at 100 if the planet radius is 500
    public static int buffer = 10;
    [SerializeField]
    private bool player = false;
    private PlayerController playerController;
    private Rigidbody rb;
    private bool disableRotation = false;

    private void Start()
    {
        planets = new List<Transform>();
        rb = GetComponent<Rigidbody>();
        if (player) playerController = GetComponent<PlayerController>();
        StartCoroutine(LateStart(0.01f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        planets = new List<Transform>();
        GameObject[] planetsArray = GameObject.Find("SolarSystemManager").GetComponent<SolarSystemGenerator>().getPlanets();
        for (int i = 0; i < planetsArray.Length; i++)
        {
            planets.Add(planetsArray[i].transform);
        }
    }
    
    void Update()
    {
        if (currPlanet != null)
        {
            float dist = Mathf.Sqrt(Mathf.Pow((currPlanet.position.x - transform.position.x), 2) + Mathf.Pow((currPlanet.position.y - transform.position.y), 2) + Mathf.Pow((currPlanet.position.z - transform.position.z), 2));
            if (dist > (distance + buffer))
            {
                currPlanet = null;
                //transform.parent = null;
                if (player) playerController.SetPlanet(null);
            }
            else
            {
                //transform.parent = currPlanet;
                Orientate();
                return;
            }
        }
        for (int i = 0; i < planets.Count; i++) {
            float dist = Mathf.Sqrt(Mathf.Pow((planets[i].position.x - transform.position.x), 2) + Mathf.Pow((planets[i].position.y - transform.position.y), 2) + Mathf.Pow((planets[i].position.z - transform.position.z), 2));
            if(dist <= distance)
            {
                currPlanet = planets[i];
                Orientate();
                if (player) playerController.SetPlanet(currPlanet);
                return;
            }
        }
    }

    void Orientate()
    {
        if (disableRotation) return;
        Vector3 planetDir = currPlanet.position - transform.position;
        rb.rotation = Quaternion.FromToRotation(transform.up, -planetDir) * transform.rotation;
    }

    public void setDisableRotation(bool x)
    {
        disableRotation = x;
    }

    public Transform getCurrPlanet() {
        return currPlanet;
    }
}