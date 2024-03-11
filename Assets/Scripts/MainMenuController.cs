using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#else
        Application.targetFrameRate = Utilities.Constants.DEFAULT_TARGET_FPS;
        Debug.unityLogger.filterLogType = LogType.Exception;
#endif
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public void WeaponSelect(int weapon)
    {
        DataManager.instance.WeaponID = weapon;
    }
}