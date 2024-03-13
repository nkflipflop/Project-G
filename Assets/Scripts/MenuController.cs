using General;
using UnityEngine;
using Utilities;

public class MenuController : MonoBehaviour
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
        
        Signals.Common.Signal.SetInputState?.Invoke(true);
    }

    public void PlayGame()
    {
        LoadingManager.instance.LoadScene(Constants.DUNGEON);
    }

    public void ExitGame()
    {
        Log.Debug("Exiting...", color: Color.red);
        Application.Quit();
    }

    public void WeaponSelect(int weapon)
    {
        DataManager.instance.WeaponID = weapon;
    }
}