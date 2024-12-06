using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioClip gameAudioClip;
    public AudioClip spaceSound;
    public AudioClip gameStartSound;
    public float gameStartSoundDelay = 2.0f;
    [Range(0f, 1f)] public float gameAudioVolume = 1.0f; // Volume control for game audio clip

    private AudioSource musicAudioSource; // Separate AudioSource for gameAudioClip
    private AudioSource sfxAudioSource;   // Separate AudioSource for sound effects
    private bool musicPlaying = false;
    private bool spaceSoundPlayed = false;
    private bool gameStartSoundPlayed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (gameAudioClip != null)
        {
            musicAudioSource.clip = gameAudioClip;
            musicAudioSource.loop = true;
            musicAudioSource.volume = gameAudioVolume; // Set initial volume for gameAudioClip
            musicAudioSource.Play();
            musicPlaying = true;
        }

        if (gameStartSound != null)
        {
            StartCoroutine(PlayGameStartSound());
        }
    }

    private IEnumerator PlayGameStartSound()
    {
        yield return new WaitForSeconds(gameStartSoundDelay);

        if (gameStartSound != null && !gameStartSoundPlayed)
        {
            sfxAudioSource.PlayOneShot(gameStartSound); // Play using sfxAudioSource
            gameStartSoundPlayed = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene" && gameAudioClip != null)
        {
            if (!musicPlaying)
            {
                musicAudioSource.volume = gameAudioVolume; // Ensure volume is set correctly
                musicAudioSource.Play();
                musicPlaying = true;
            }
        }
        else if (scene.name == "StartScene" && gameAudioClip != null)
        {
            musicAudioSource.Stop();
            musicAudioSource.volume = gameAudioVolume; // Ensure volume is set correctly
            musicAudioSource.Play();
            musicPlaying = true;

            if (!spaceSoundPlayed && spaceSound != null)
            {
                sfxAudioSource.PlayOneShot(spaceSound); // Play using sfxAudioSource
                spaceSoundPlayed = true;
            }

            gameStartSoundPlayed = false;

            if (gameStartSound != null && !gameStartSoundPlayed)
            {
                sfxAudioSource.PlayOneShot(gameStartSound); // Play using sfxAudioSource
                gameStartSoundPlayed = true;
            }
        }
    }

    void Update()
    {
        // Dynamically update the volume for the gameAudioClip
        if (musicAudioSource != null && musicAudioSource.clip == gameAudioClip)
        {
            musicAudioSource.volume = gameAudioVolume;
        }

        if (Input.GetKeyDown(KeyCode.Space) && spaceSound != null)
        {
            sfxAudioSource.PlayOneShot(spaceSound); // Play using sfxAudioSource
        }
    }
}
