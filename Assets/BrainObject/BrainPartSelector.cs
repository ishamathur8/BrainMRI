using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // If you are using TextMeshPro

public class BrainPartSelector : MonoBehaviour
{
    public TMP_Dropdown regionDropdown;
    public TMP_Dropdown highlightDropdown;
    //public UIController uiController;

    private Dictionary<string, Transform> brainParts = new Dictionary<string, Transform>();
    public GameObject brainPrefab; // Assign the brain prefab in the inspector

    void Start()
    {
        if (regionDropdown == null || highlightDropdown == null || brainPrefab == null)
        {
            Debug.LogError("Dropdowns, Brain Prefab, UI Controller not assigned!");
            return;
        }

        // Find the brain prefab in the scene
        GameObject brainInstance = GameObject.Find(brainPrefab.name);

        if (brainInstance == null)
        {
            Debug.LogError("Brain Prefab not found in the scene! Make sure it's instantiated before this script runs.");
            return;
        }

        PopulateUI(brainInstance);
    }

    void PopulateUI(GameObject brainInstance)
    {
        PopulateRegionDropdown(brainInstance);
        PopulateHighlightDropdown();
    }

    void PopulateRegionDropdown(GameObject brainInstance)
    {
        regionDropdown.ClearOptions();
        List<string> dropdownOptions = new List<string>();

        // Get the brain parts from brainInstance
        foreach (Transform child in brainInstance.transform)
        {
            brainParts.Add(child.name, child);
            dropdownOptions.Add(child.name);
        }

        regionDropdown.AddOptions(dropdownOptions);

        // Set up a listener for when the dropdown value changes
        regionDropdown.onValueChanged.AddListener(delegate {
            RegionDropdownValueChanged(regionDropdown);
        });
    }

    void PopulateHighlightDropdown()
    {
        highlightDropdown.ClearOptions();
        List<string> dropdownOptions = new List<string>();
        dropdownOptions.Add("Show Only Selected");
        dropdownOptions.Add("Show with Context");
        dropdownOptions.Add("Normal View");

        highlightDropdown.AddOptions(dropdownOptions);

        // Set up a listener for when the dropdown value changes
        highlightDropdown.onValueChanged.AddListener(delegate {
            HighlightDropdownValueChanged(highlightDropdown);
        });
    }

    void RegionDropdownValueChanged(TMP_Dropdown dropdown)
    {
        string selectedBrainPartName = dropdown.options[dropdown.value].text;

        if (brainParts.ContainsKey(selectedBrainPartName))
        {
            Transform selectedPart = brainParts[selectedBrainPartName];
            // Extrusion Logic Here
            // Information Display Logic Here
        }
    }

    void HighlightDropdownValueChanged(TMP_Dropdown dropdown)
    {
        string selectedOption = dropdown.options[dropdown.value].text;
        // Highlighting Logic Here
    }

    // Placeholder function to get details for a brain part
    string GetBrainPartDetails(Transform part)
    {
        return part.name;
    }
}