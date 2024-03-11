using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class UIController : MonoBehaviour
{
    public CursorController CursorUI;

    public LevelGUI LevelUI;
    public MiscGUI MiscUI;
    public WeaponGUI WeaponUI;

    public GameObject PauseMenuUI;
    public GameObject GameOverUI;

    public PlayerManager PlayerManager;
    public GameManager GameManager;

    public Color existColor;
    public Color absentColor;

    private WeaponBase _weapon = null; // Current Weapon
    private int _ammo = -1; // Current Ammo
    private int _health = -1; // Current Health
    private int _medkit = -1; // Current Medkit
    private int _shield = -1; // Current Shield
    private bool _hasKey = false; // Having Key
    private int _level = -1;

    private float _reloadTimer = 0f;

    private void Start()
    {
        MiscUI.KeyUI.SetActive(false);
    }

    private void Update()
    {
        // Weapon Sprite and Name Update
        if (_weapon != PlayerManager.PlayerHandController.CurrentWeapon)
        {
            _weapon = PlayerManager.PlayerHandController.CurrentWeapon;
            WeaponUI.WeaponImage.sprite = _weapon.WeaponRenderer.sprite; // Weapon Info
            WeaponUI.WeaponImage.SetNativeSize();
            WeaponUI.WeaponText.text = _weapon.Weapon.Name;
            WeaponUI.AmmoImage.sprite = _weapon.Weapon.Bullet.Renderer.sprite; // Ammo Info
            WeaponUI.AmmoImage.SetNativeSize();
        }

        // Ammo Count Update
        if (_ammo != PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo)
        {
            _ammo = PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo;
            WeaponUI.AmmoText.text = _ammo.ToString();
            if (_ammo == 0)
            {
                WeaponUI.SetColor(absentColor); // Setting color of ammo bar
                if (_reloadTimer <= 0)
                {
                    _reloadTimer = _weapon.Weapon.ReloadTime;       // Starts reloading
                }
            }
            else
            {
                WeaponUI.SetColor(existColor);
            }
        }

        // Setting Reload Slider
        if (_reloadTimer > 0)
        {
            _reloadTimer -= Time.deltaTime;
            WeaponUI.SetReloadSlider(_reloadTimer / _weapon.Weapon.ReloadTime);
            if (_reloadTimer <= 0)
            {
                WeaponUI.SetReloadSlider(1);
            }
        }

        // Health Count Update
        if (_health != PlayerManager.HealthController.Health)
        {
            _health = PlayerManager.HealthController.Health;
            MiscUI.HealthText.text = _health.ToString();
            MiscUI.SetHealthColor((float)_health / 100);
        }

        // MedKit Count Update
        if (_medkit != PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Medkit].Count)
        {
            _medkit = PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Medkit].Count;
            MiscUI.MedkitText.text = _medkit.ToString();
            MiscUI.SetMedKitColor(_medkit == 0 ? absentColor : existColor);     // Setting color of medkit bar
        }

        // Shield Count Update
        if (_shield != PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Shield].Count)
        {
            _shield = PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Shield].Count;
            MiscUI.ShieldText.text = _shield.ToString();
            MiscUI.SetShieldColor(_shield == 0 ? absentColor : existColor); // Setting color of shield bar
        }

        // Key Owning Update
        if (_hasKey != PlayerManager.HasKey)
        {
            _hasKey = PlayerManager.HasKey;
            MiscUI.KeyUI.SetActive(true);
        }

        // Level Value Update
        if (_level != GameManager.DungeonLevel)
        {
            _level = GameManager.DungeonLevel;
            LevelUI.LevelText.text = Extensions.ConcatenateString("Level ", _level + 1);
        }
    }

    /// <summary> Pauses/Resumes the game by toggling the current situation </summary>
    public void TogglePause()
    {
        Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
        PauseMenuUI.SetActive(Time.timeScale != 1); // activate/deactivate the Pause Menu
        CursorUI.ToggleShowMouse(PauseMenuUI.activeSelf);
    }

    // Pause Menu Operations
    public void NextLevel()
    {
        TogglePause();
        GameManager.LoadNextLevel();
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void ReturnMainMenu()
    {
        GameManager.ResetLevelData(); // before going back to the main menu, reset al data
        if (Time.timeScale == 0) // if the game is paused, resume it and then, go to main menu
        {
            TogglePause();
        }
        SceneManager.LoadScene(0); // load main menu
    }

    public void ExitGame()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public async UniTaskVoid ActivateGameOverScreen()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: this.GetCancellationTokenOnDestroy());
        GameManager.GameCamera.GetComponent<AudioListener>().enabled = true;
        GameOverUI.SetActive(true);
    }
}