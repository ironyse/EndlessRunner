using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOverController : MonoBehaviour
{
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
