using System;
using Unity.VisualScripting;
using UnityEngine;



public enum SoundType
{
    InGame,
    SelectLevel,
    FXWin,
    FXLose,
    FXAttack,
    FXHit,
    FXSpawnEnemy,
    FXMove,
    FXConnect,
    FXShowStar,
    FXButtonClick
}
public class AudioManager : DesignPattern.Singleton<AudioManager>
{
    [Header("-----Audio Source-----")]
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioSource m_SFXSource;

    [Header("-----Audio Clip-----")] [SerializeField]
    private SoundSource[] soundList;

  
  
    
    [Header("----------Data----------")] 
    [SerializeField] private float m_MusicVolumeRate = 1;
    [SerializeField] private float m_SFXVolumeRate = 1;

    public float MusicVolumeRate
    {
        get => m_MusicVolumeRate;
        set
        {
            if (Math.Abs(m_SFXVolumeRate - value) > 0.0001f)
            {
                m_MusicSource.volume = value;
                m_MusicVolumeRate = value;
            }
            
        }
    }

    public float SfxVolumeRate
    {
        get => m_SFXVolumeRate;
        set => m_SFXVolumeRate = value;
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadComponent();
    }

    private void Reset()
    {
        LoadComponent();
    }

    private void LoadComponent()
    {
        if (m_MusicSource == null)
        {
            Transform musicTrf = transform.Find("Music");
            if (musicTrf == null)
            {
                musicTrf = new GameObject("Music").transform;
                musicTrf.parent = transform;
            }

            m_MusicSource = musicTrf.GetComponent<AudioSource>();
            if (m_MusicSource == null) m_MusicSource = musicTrf.AddComponent<AudioSource>();
        }

        if (m_SFXSource == null)
        {
            Transform sfxTrf = transform.Find("SFX");
            if (sfxTrf == null)
            {
                sfxTrf = new GameObject("SFX").transform;
                sfxTrf.parent = transform;
            }

            m_SFXSource = sfxTrf.GetComponent<AudioSource>();
            if (m_SFXSource == null) m_SFXSource = sfxTrf.AddComponent<AudioSource>();
        }
    }
    

 

    public static void PlaySFX(SoundType sound, float volume = 1)
    {
        
        volume = Mathf.Clamp(volume * Instance.m_SFXVolumeRate, 0, 1);
        Instance.m_SFXSource.PlayOneShot(Instance.soundList[(int)sound].Sound, volume);
    }

    public static void PlayBackGroundMusic(SoundType sound)
    {
       
        Instance.m_MusicSource.Stop();
        Instance.m_MusicSource.clip = Instance.soundList[(int)sound].Sound;

        Instance.m_MusicSource.volume = Instance.m_MusicVolumeRate;
        Instance.m_MusicSource.Play();
    }

    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
    
        if (soundList == null || soundList.Length != names.Length)
        {
            Array.Resize(ref soundList, names.Length);
        }

        for (int i = 0; i < names.Length; ++i)
        {
            if (soundList[i].name == null) 
            {
                soundList[i] = new SoundSource();
            }
            soundList[i].name = names[i];
        }
    }
    public static void StopAudioMusic()
    {
        Instance.m_MusicSource.Stop();
    }
    public static void PlayContinueSound()
    {
        Instance.m_MusicSource.Play();
    }

}

[Serializable]
public struct SoundSource
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip sound;
    public AudioClip Sound
    {
        get => sound;
    }
}

