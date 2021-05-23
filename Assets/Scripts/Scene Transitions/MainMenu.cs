using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Puebito");
    }
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
