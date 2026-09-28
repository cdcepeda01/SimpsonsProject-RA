using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource hoverSource;

    [Header("Música por escena")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip tutorialMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip finalMusic;

    [Header("Sonidos UI")]
    [SerializeField] private AudioClip buttonSelectSound;
    [SerializeField] private AudioClip buttonHoverSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(
            SceneManager.GetActiveScene().name
        );
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        StopButtonHover();

        PlayMusicForScene(scene.name);
    }

    // =====================================================
    // MÚSICA
    // =====================================================

    private void PlayMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "00_MenuSimpsons":
                PlayMusic(menuMusic);
                break;

            case "01_TutorialSimpsons":
                PlayMusic(tutorialMusic);
                break;

            case "02_JuegoSimpsons":
                PlayMusic(gameMusic);
                break;

            case "03_FinalSimpsons":
                PlayMusic(finalMusic);
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
            return;

        if (
            musicSource.clip == clip &&
            musicSource.isPlaying
        )
        {
            return;
        }

        musicSource.Stop();

        musicSource.clip = clip;
        musicSource.loop = true;

        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // =====================================================
    // SONIDO AL SELECCIONAR BOTÓN
    // =====================================================

    public void PlayButtonSelect()
    {
        if (
            sfxSource == null ||
            buttonSelectSound == null
        )
        {
            return;
        }

        sfxSource.PlayOneShot(
            buttonSelectSound
        );
    }

    // =====================================================
    // HOVER
    // =====================================================

    public void StartButtonHover()
    {
        if (
            hoverSource == null ||
            buttonHoverSound == null
        )
        {
            return;
        }

        // Evita reiniciar el sonido continuamente.
        if (hoverSource.isPlaying)
            return;

        hoverSource.clip = buttonHoverSound;
        hoverSource.Play();
    }

    public void StopButtonHover()
    {
        if (hoverSource == null)
            return;

        hoverSource.Stop();
        hoverSource.clip = null;
    }

    // =====================================================
    // SFX GENÉRICOS
    // =====================================================

    public void PlaySFX(AudioClip clip)
    {
        if (
            clip == null ||
            sfxSource == null
        )
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    // =====================================================
    // VOLUMEN
    // =====================================================

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume =
                Mathf.Clamp01(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        float value =
            Mathf.Clamp01(volume);

        if (sfxSource != null)
        {
            sfxSource.volume = value;
        }

        if (hoverSource != null)
        {
            hoverSource.volume = value;
        }
    }
}