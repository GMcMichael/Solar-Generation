using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    private int speed;
    [SerializeField]
    private int distFromCenter;
    [SerializeField]
    private float currAngle;//maybe have a vertical angle as well so they arent all on the same horizontal plane

    void Awake()
    {
        currAngle = Random.Range(0, 360);
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }

    public void setDist(int dist)
    {
        distFromCenter = dist;
    }

    public int getDist()
    {
        return distFromCenter;
    }

    public float getAngle()
    {
        return currAngle;
    }

    public void addAngle(float add)
    {
        currAngle += add*speed;
        currAngle = currAngle % 360;
    }
}
