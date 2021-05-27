using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : MonoBehaviour
{
    [SerializeField]
    private int[] storage;
    [SerializeField]
    private int[] slotTypes;
    private int slotMaximum = 999;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AddResource(int type, int amount) {
        if(ContainsType(type)) {
            for(int i = 0; i < slotTypes.Length; i++) {
                if(slotTypes[i] == type) amount = DepositResource(i, type, amount);
                if(amount == 0) break;
            }
        } else {
            for(int i = 0; i < slotTypes.Length; i++) {
                if(storage[i] == 0) {
                    amount = DepositResource(i, type, amount);
                    slotType[i] = type;
                }
                if(amount == 0) break;
            }
        }
        return amount;
    }

    private int DepositResource(int index, int type, int amount) {
        storage[index] += amount;
        int remaning = storage[index]%slotMaximum;
        if(remaning != 0) {
            storage[index] = slotMaximum;
            return remaning;
        }
        return 0;
    }

    public int GetResource(int type = -1, int slot = -1, int maxAmount) {
        if(slot >= 0) {
            //get resources from slot
        } else if(type >= 0) {
            //get resource type
        }
        return 0;
    }

    private bool ContainsType(int type) {
        foreach(int slotType in slotTypes) {
            if(type == slotType) return true;
        }
        return false;
    }
}
