using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.selectedPlayerAnimator)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }    
}
