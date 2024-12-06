using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public float speed = 5.0f; // Speed of phone movement
    public float moveInterval = 3.0f; // Time between movements while on-screen
    public float minInterval = 2.0f; // Minimum time before reappearing
    public float maxInterval = 5.0f; // Maximum time before reappearing
    public float firstAppearTime = -1.0f; // Specific time for the first appearance (-1 means disabled)
    public AudioClip[] phoneSounds; // Array of sounds to play when the phone comes on screen

    private Vector3 originalPosition; // Original starting position of the phone
    private Vector3 targetPosition;   // Current target position
    private bool isOnScreen = false;  // Whether the phone is currently on-screen
    private float nextMoveTime;       // Time for the next movement
    private float reappearTime;       // Time for reappearing after resetting to the original position
    private bool hasFirstAppeared = false; // Whether the phone has made its first appearance
    private AudioSource audioSource;  // Audio source to play the sound

    void Start()
    {
        // Store the phone's original position
        originalPosition = transform.position;

        // Add the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;  // The sound will loop while the phone is on screen

        // Start with the phone at its original position
        ResetToOriginalPosition();

        if (firstAppearTime < 0)
        {
            ScheduleReappearance();
        }
    }

    void Update()
    {
        if (isOnScreen && Time.time >= nextMoveTime)
        {
            // Move to a new position on-screen
            MoveToRandomScreenPosition();
        }

        if (!isOnScreen && !hasFirstAppeared && firstAppearTime > 0 && Time.time >= firstAppearTime)
        {
            // Ensure the first appearance happens at the specified time
            MoveToScreen();
            hasFirstAppeared = true;
        }
        else if (!isOnScreen && hasFirstAppeared && Time.time >= reappearTime)
        {
            // Reappear on the screen after the first appearance
            MoveToScreen();
        }
    }

    private void MoveToRandomScreenPosition()
    {
        Camera cam = Camera.main;

        // Generate a random position within the camera's view
        float screenX = Random.Range(0.2f, 0.8f); // Between 20% and 80% of screen width
        float screenY = Random.Range(0.2f, 0.8f); // Between 20% and 80% of screen height

        Vector3 screenPosition = new Vector3(screenX, screenY, cam.nearClipPlane + 1.0f);
        targetPosition = cam.ViewportToWorldPoint(screenPosition);

        // Smoothly move the phone to the new position
        StartCoroutine(MoveTowardsTarget(targetPosition));

        // Schedule the next move
        nextMoveTime = Time.time + moveInterval;
    }

    private void MoveToScreen()
    {
        isOnScreen = true;

        // Play a random sound when the phone comes on screen
        PlayRandomPhoneSound();

        // Start at a random position within the camera's view
        MoveToRandomScreenPosition();
    }

    private void ResetToOriginalPosition()
    {
        isOnScreen = false;

        // Stop the sound when the phone is reset
        audioSource.Stop();

        // Move the phone to its original position
        StartCoroutine(MoveTowardsTarget(originalPosition));

        if (hasFirstAppeared)
        {
            ScheduleReappearance();
        }
    }

    private void ScheduleReappearance()
    {
        reappearTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    private void PlayRandomPhoneSound()
    {
        // Check if there are sounds in the array
        if (phoneSounds.Length > 0)
        {
            // Pick a random sound from the array
            int randomIndex = Random.Range(0, phoneSounds.Length);
            audioSource.clip = phoneSounds[randomIndex];
            audioSource.Play();
        }
    }

    private System.Collections.IEnumerator MoveTowardsTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
    }

    void OnMouseDown()
    {
        // Handle click to reset the phone's position
        if (isOnScreen)
        {
            ResetToOriginalPosition();
        }
    }
}