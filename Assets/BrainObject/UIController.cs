using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you are using TextMeshPro for better text rendering

public class UIController : MonoBehaviour
{
    public Transform brainModel; // Assign your Brain GameObject here
    public float rotationSpeed = 10f;
    public GameObject detailsPanel; // Assign your details Panel GameObject here
    public TMP_Text detailsText; // Assign the TextMeshPro Text component in the panel
    public AudioSource uiAudioSource; // Assign the Audio Source component
    public AudioClip rotateSound; // Assign the audio clip for rotation sound
    public AudioClip detailsSound; // Assign the audio clip for details sound

    void Start()
    {
        if (brainModel == null || detailsPanel == null || detailsText == null || uiAudioSource == null || rotateSound == null || detailsSound == null)
        {
            Debug.LogError("Brain Model, Details Panel, Text, AudioSource, or Audio Clips not assigned!");
            enabled = false;
        }

        detailsPanel.SetActive(false); // Initially hide the details panel
    }

    public void RotateLeft()
    {
        if (brainModel != null)
        {
            brainModel.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
            uiAudioSource.PlayOneShot(rotateSound);
        }
    }

    public void RotateRight()
    {
        if (brainModel != null)
        {
            brainModel.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            uiAudioSource.PlayOneShot(rotateSound);
        }
    }

    public void DisplayObjectDetails(string details)
    {
        detailsText.text = details;
        detailsPanel.SetActive(true);
        uiAudioSource.PlayOneShot(detailsSound);
    }

    public void HideObjectDetails()
    {
        detailsPanel.SetActive(false);
    }
}