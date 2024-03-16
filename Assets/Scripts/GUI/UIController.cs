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

    private WeaponBase weapon; // Current Weapon
    private int ammo = -1; // Current Ammo
    private int health = -1; // Current Health
    private int medKit = -1; // Current MedKit
    private int shield = -1; // Current Shield
    private bool hasKey = false; // Having Key
    private int level = -1;

    private float reloadTimer = 0f;

    private void Start()
    {
        MiscUI.KeyUI.SetActive(false);
    }

    private void Update()
    {
        // Weapon Sprite and Name Update
        if (weapon != PlayerManager.PlayerHandController.CurrentWeapon)
        {
            weapon = PlayerManager.PlayerHandController.CurrentWeapon;
            WeaponUI.WeaponImage.sprite = weapon.WeaponRenderer.sprite; // Weapon Info
            WeaponUI.WeaponImage.SetNativeSize();
            WeaponUI.WeaponText.text = weapon.Name;
            WeaponUI.AmmoImage.sprite = weapon.BulletIcon; // Ammo Info
            WeaponUI.AmmoImage.SetNativeSize();
        }

        // Ammo Count Update
        if (ammo != PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo)
        {
            ammo = PlayerManager.PlayerHandController.CurrentWeapon.CurrentAmmo;
            WeaponUI.AmmoText.text = ammo.ToString();
            if (ammo == 0)
            {
                WeaponUI.SetColor(absentColor); // Setting color of ammo bar
                if (reloadTimer <= 0)
                {
                    reloadTimer = weapon.ReloadTime;       // Starts reloading
                }
            }
            else
            {
                WeaponUI.SetColor(existColor);
            }
        }

        // Setting Reload Slider
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            WeaponUI.SetReloadSlider(reloadTimer / weapon.ReloadTime);
            if (reloadTimer <= 0)
            {
                WeaponUI.SetReloadSlider(1);
            }
        }

        // Health Count Update
        if (health != PlayerManager.HealthController.Health)
        {
            health = PlayerManager.HealthController.Health;
            MiscUI.HealthText.text = health.ToString();
            MiscUI.SetHealthColor((float)health / 100);
        }

        // MedKit Count Update
        if (medKit != PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Medkit].Count)
        {
            medKit = PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Medkit].Count;
            MiscUI.MedkitText.text = medKit.ToString();
            MiscUI.SetMedKitColor(medKit == 0 ? absentColor : existColor);     // Setting color of medkit bar
        }

        // Shield Count Update
        if (shield != PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Shield].Count)
        {
            shield = PlayerManager.Inventory[(int)GameConfigData.CollectibleType.Shield].Count;
            MiscUI.ShieldText.text = shield.ToString();
            MiscUI.SetShieldColor(shield == 0 ? absentColor : existColor); // Setting color of shield bar
        }

        // Key Owning Update
        if (hasKey != PlayerManager.HasKey)
        {
            hasKey = PlayerManager.HasKey;
            MiscUI.KeyUI.SetActive(true);
        }

        // Level Value Update
        if (level != GameManager.DungeonLevel)
        {
            level = GameManager.DungeonLevel;
            LevelUI.LevelText.text = Extensions.ConcatenateString("Level ", level + 1);
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
        GameManager.cam.GetComponent<AudioListener>().enabled = true;
        GameOverUI.SetActive(true);
    }
}