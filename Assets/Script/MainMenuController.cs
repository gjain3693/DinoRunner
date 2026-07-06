using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private const string LEVEL01 = "Level01";

    public void EnterGame()
    {
        if (Application.CanStreamedLevelBeLoaded(LEVEL01))
        {
            SceneManager.LoadScene(LEVEL01);
        }
        else
        {
            Debug.LogError(
                "Scene not found in Build Settings: " +
                LEVEL01
            );
        }
    }
}