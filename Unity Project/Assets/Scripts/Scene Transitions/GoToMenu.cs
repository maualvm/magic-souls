using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    public void Menu()
    {
        AudioManager.PlaySound(AudioManager.Sound.Confirm);
        StartCoroutine(ChangeScene("MainMenu"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ChangeScene(string scene)
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(scene);
    }
}
