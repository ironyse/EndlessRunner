using UnityEngine;
using UnityEngine.UI;

public class UICurrencyContoller : MonoBehaviour
{
    public Text diamondAmount;
    
    void Update(){
        diamondAmount.text = CurrencyData.diamondAmount.ToString();        
    }
}
