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

    //Called from external scripts and returns an array with the amount and type
    public int[] GetResource(int slot, int maxAmount) {
        int[] data = new int[2];
        int amount = storage[slot];
        if(amount <= maxAmount) {
            storage[slot] = 0;
            data[0] = amount;
        } else {
            storage[slot] -= maxAmount;
            data[0] = maxAmount;
        }
            data[1] = slotTypes[slot];
        return data;
    }

    //Called from external scripts and gets sent the type an amount, return the amount that can't be added
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
                    slotTypes[i] = type;
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

    private bool ContainsType(int type) {
        foreach(int slotType in slotTypes) {
            if(type == slotType) return true;
        }
        return false;
    }
}
