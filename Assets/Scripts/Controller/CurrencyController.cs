using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    private int currentAmount;

    void Start(){
        currentAmount = 0;
    }

    public float GetCurrentAmount() {
        return currentAmount;
    }

    public void IncreaseCurrentAmount() {
        currentAmount++;
    }

    public void SaveCurrency() {
        CurrencyData.diamondAmount += currentAmount;
    }
}
