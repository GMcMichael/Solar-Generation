using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    private string[] resourceTypes = {"iron", "copper", "stone", "coal"};
    private int[] resourceMax = {999, 999, 999, 999};
    [SerializeField]
    private int[] resourceHolder;
    
    void Start() {
        resourceHolder = new int[resourceTypes.Length];
    }

    public void ChangeResource(int type, int amount) {
        resourceHolder[type] += amount;
        if(resourceHolder[type] > resourceMax[type]) resourceHolder[type] = resourceMax[type];
    }
}
