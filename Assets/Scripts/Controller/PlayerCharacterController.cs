using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour
{
    public Button UnlockButton;
    public Button SelectButton;
    public Image CharacterImage;
    public Text UnlockCostText;
    public Text SelectText;

    private bool IsUnlocked { get; set; }
    private bool IsSelected { get; set; }    
    private int UnlockCost { get; set; }

    private Character _character;    

    public void SetCharacter(Character chr){
        _character = chr;
        UnlockCost = _character.UnlockCost;
        UnlockCostText.text = _character.UnlockCost.ToString("0");
        CharacterImage.sprite = _character.sprite;
        IsUnlocked = _character.Unlocked;
    }

    private void SelectCharacter() {
        GameManager.Instance.SetSelectedChar(_character);
    }

    private void Update()
    {
        if (IsUnlocked)
        {
            IsSelected = (GameManager.Instance.SelectedCharacter() == _character);
            SelectText.text = IsSelected ? "Selected" : "Select";
            SelectButton.enabled = !IsSelected;
        }
    }

    private void SetButtonUnlocked(){
        UnlockButton.gameObject.SetActive(false);
        SelectButton.gameObject.SetActive(true);
        SelectButton.onClick.AddListener(() => {
            SelectCharacter();
        });
    }

    private void Start(){
        if (IsUnlocked){
            SetButtonUnlocked();
        } else {
            UnlockButton.onClick.AddListener(() =>{                
                if (CurrencyData.diamondAmount >= UnlockCost){
                    CurrencyData.diamondAmount -= UnlockCost;                 
                    GameManager.Instance.UnlockCharacter(_character.Name);
                    GameManager.Instance.Save();
                    SetButtonUnlocked();
                }
            });
        }
    }
}
