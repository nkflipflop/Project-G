using General;
using UnityEngine;
using Utilities;

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
        LoadingManager.instance.LoadScene(Constants.DUNGEON);
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