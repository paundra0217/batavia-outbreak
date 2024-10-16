using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void ChangeScene(string scene)
    {
        UILoadingScreen.LoadScene(scene);
    }
}
