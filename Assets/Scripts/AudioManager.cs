// =============================================================================
// AudioManager.cs
// Manages background music playback and exposes volume control to the UI.
// =============================================================================

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Constants
    // -------------------------------------------------------------------------

    private const bool LoopMusic = true;

    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("Music")]
    public AudioClip gameMusic;

    [Header("Volume")]
    public float volume = 1f;

    // -------------------------------------------------------------------------
    // Private State
    // -------------------------------------------------------------------------

    private AudioSource audioSource;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Initialize and play background music
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.loop = LoopMusic;
        audioSource.volume = volume;
        audioSource.Play();
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    // Update the audio source volume from an external caller
    public void SetVolume(float newVolume)
    {
        volume = newVolume;

        if (audioSource != null)
        {
            audioSource.volume = newVolume;
        }
    }
}