using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public AudioClip spaceSound;
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if in the "StartScene" and Space is pressed, then play the sound
        if (SceneManager.GetActiveScene().name == "StartScene" && Input.GetKeyDown(KeyCode.Space) && spaceSound != null)
        {
            audioSource.PlayOneShot(spaceSound);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Additional logic for when a scene is loaded can go here
    }
}
