using UnityEngine;
using UnityEngine.UI;

public class UICollectibleContoller : MonoBehaviour
{
    public Text diamondAmount;
    public CurrencyController currencyController;

    void Update(){
        diamondAmount.text = currencyController.GetCurrentAmount().ToString();
    }
}
