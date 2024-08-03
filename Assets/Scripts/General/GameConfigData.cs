using UnityEngine;

public class GameConfigData : MonoBehaviour
{
    private static GameConfigData instance;

    public static GameConfigData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ((GameObject)Instantiate(Resources.Load("GameConfigData"))).GetComponent<GameConfigData>();
            }

            return instance;
        }
    }

    public enum CollectibleType
    {
        Medkit,
        Shield,
        Snack
    };

    public GameObject Consumable;

    public SoundAudioClip[] Sounds;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
    }
}