//Stable v1
using UnityEngine;
using TMPro; // For TextMesh Pro components
using UnityEngine.UI; // For Button components
using UnityEngine.XR.ARFoundation; // For AR Foundation
using UnityEngine.XR.ARSubsystems; // For AR Subsystems

public class BrainARController : MonoBehaviour
{
    // UI Elements
    public TMP_Dropdown regionDropdown; // TextMesh Pro Dropdown for selecting brain regions
    public Button rotateButton;         // Button for rotation
    public Button audioButton;          // Button for audio
    public Slider transparencySlider;   // Optional: Slider to control transparency (if added to UI)

    // Brain Object and AR
    public GameObject brainPrefab;      // Prefab to spawn when image is detected
    private GameObject spawnedBrain;    // Reference to the spawned brain instance
    public ARTrackedImageManager trackedImageManager; // Reference to AR Tracked Image Manager

    // Audio
    public AudioSource audioSource;     // Audio source component
    public AudioClip WhiteMatterTelencephalon_r_001;
    public AudioClip WhiteMatterTelencephalon_r_002;
    public AudioClip TemporalPlane_r_001;// Array of audio clips for each region

    // Highlight and Opacity Materials
    public Material highlightMaterial;  // Material for the selected region (fully opaque)
    public Material opaqueMaterial;     // Material for non-selected regions (semi-transparent)
    private Material[] originalMaterials; // Array to store original materials of all children
    private GameObject selectedRegion;  // Currently selected child object

    // Transparency Control
    private float transparencyLevel = 0.3f; // Default transparency level (0 = fully transparent, 1 = fully opaque)

    void Awake()
    {
        // Subscribe to the tracked image event
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void Start()
    {
        // Initialize original materials array
        originalMaterials = new Material[brainPrefab.transform.childCount];

        // Populate Dropdown with child names (using prefab data)
        PopulateDropdown();

        // Add listeners to buttons
        rotateButton.onClick.AddListener(RotateSelectedRegion);
        audioButton.onClick.AddListener(PlayAudio);

        // Add listener to transparency slider (if used)
        if (transparencySlider != null)
        {
            transparencySlider.onValueChanged.AddListener(delegate { UpdateTransparency(transparencySlider.value); });
            transparencySlider.value = transparencyLevel; // Set default value
        }

        // Ensure audio source is set up
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Apply initial transparency to opaque material
        UpdateMaterialTransparency(opaqueMaterial, transparencyLevel);
    }

    void PopulateDropdown()
    {
        // Clear existing options
        regionDropdown.options.Clear();

        // Add each child name as an option and store original materials
        int index = 0;
        foreach (Transform child in brainPrefab.transform)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(child.name);
            regionDropdown.options.Add(option);

            // Store the original material from the prefab
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                originalMaterials[index] = renderer.material;
            }
            index++;
        }

        // Select the first option by default
        regionDropdown.value = 0;
        regionDropdown.onValueChanged.AddListener(delegate { UpdateSelection(); });
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // Spawn the brain prefab when a new image is detected
            if (spawnedBrain == null)
            {
                spawnedBrain = Instantiate(brainPrefab, trackedImage.transform.position, trackedImage.transform.rotation, trackedImage.transform);
                UpdateSelection(); // Initialize the selection
            }
        }

        // Update the brain position if the image is tracked
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (spawnedBrain != null && trackedImage.trackingState == TrackingState.Tracking)
            {
                spawnedBrain.transform.position = trackedImage.transform.position;
                spawnedBrain.transform.rotation = trackedImage.transform.rotation;
            }
        }

        // Clean up if the image is lost
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (spawnedBrain != null)
            {
                Destroy(spawnedBrain);
                spawnedBrain = null;
            }
        }
    }


    void UpdateSelection()
    {
        if (spawnedBrain == null) return; // Exit if no brain is spawned

        // Reset all regions to opaque material with current transparency
        for (int i = 0; i < spawnedBrain.transform.childCount; i++)
        {
            Transform child = spawnedBrain.transform.GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null && i != regionDropdown.value)
            {
                renderer.material = opaqueMaterial; // Apply semi-transparent material
            }
        }

        // Highlight the selected region
        int selectedIndex = regionDropdown.value;
        if (selectedIndex >= 0 && selectedIndex < spawnedBrain.transform.childCount)
        {
            selectedRegion = spawnedBrain.transform.GetChild(selectedIndex).gameObject;

            // Apply highlight material to the selected region
            Renderer renderer = selectedRegion.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = highlightMaterial; // Fully opaque highlight
            }
        }
    }

    void RotateSelectedRegion()
    {
        if (selectedRegion != null)
        {
            // Rotate the selected region by 90 degrees around Y-axis
            selectedRegion.transform.Rotate(0, 90, 0);
        }
    }

    void PlayAudio()
    {
        if (audioSource == null) return;

        string selected = regionDropdown.options[regionDropdown.value].text;
        AudioClip clipToPlay = null;

        switch (selected)
        {
            case "White matter of telencephalon.r.001":
                clipToPlay = WhiteMatterTelencephalon_r_001;
                break;
            case "White matter of telencephalon.r.002":
                clipToPlay = WhiteMatterTelencephalon_r_002;
                break;
            case "Temporal plane.r.001":
                clipToPlay = TemporalPlane_r_001;
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }

    // Method to update material transparency
    void UpdateMaterialTransparency(Material material, float alpha)
    {
        if (material != null && material.HasProperty("_Color"))
        {
            Color color = material.color;
            color.a = alpha;
            material.SetColor("_Color", color);
        }
    }

    // Method to update transparency level
    public void UpdateTransparency(float value)
    {
        transparencyLevel = value; // Update the transparency level (0 to 1)
        UpdateMaterialTransparency(opaqueMaterial, transparencyLevel); // Apply to opaque material

        // Reapply materials to ensure all non-selected regions reflect the new transparency
        if (spawnedBrain != null)
        {
            UpdateSelection(); // Refresh the selection to apply new transparency
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        // Restore original materials if the brain exists
        if (spawnedBrain != null)
        {
            for (int i = 0; i < spawnedBrain.transform.childCount; i++)
            {
                Transform child = spawnedBrain.transform.GetChild(i);
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null && originalMaterials[i] != null)
                {
                    renderer.material = originalMaterials[i];
                }
            }
            Destroy(spawnedBrain);
        }
    }
   

}

