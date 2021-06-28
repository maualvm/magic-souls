using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.PlaySound(AudioManager.Sound.Confirm);
        StartCoroutine(ChangeScene("PuebloFulir"));
    }
    public void Instructions()
    {
        AudioManager.PlaySound(AudioManager.Sound.Confirm);
        StartCoroutine(ChangeScene("Instructions"));
    }
    public void Credits()
    {
        AudioManager.PlaySound(AudioManager.Sound.Confirm);
        StartCoroutine(ChangeScene("Credits"));
    }

    private IEnumerator ChangeScene(string scene)
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(scene);
    }
}
