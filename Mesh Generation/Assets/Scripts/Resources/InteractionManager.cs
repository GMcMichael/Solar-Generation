using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private enum ResourceType {iron, copper, stone, coal};
    [SerializeField]
    private ResourceType resourceType;
    [SerializeField]
    private int amountAdded = 1;

    public void Interact(ResourceInventory inventory) {//need to add a timer for mining
        inventory.ChangeResource((int)resourceType, amountAdded);
    }
}
