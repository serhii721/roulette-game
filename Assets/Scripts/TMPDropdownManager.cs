using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPDropdownManager : MonoBehaviour
{
    // UI Elements
    public TMP_Dropdown playerCountTMPDropdown;
    public GameObject[] playerNameInputs;

    // Start is called before the first frame update
    void Start()
    {
        // Adding options to dropdown
        AddOptionsToTMPDropDown();

        // Adding a listener for when the dropdown value changes
        playerCountTMPDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        // Ensure we start with the correct elements visible (based on default dropdown value)
        OnDropdownValueChanged(playerCountTMPDropdown.value);
    }

    void AddOptionsToTMPDropDown()
    {
        // Clearing existing options
        playerCountTMPDropdown.ClearOptions();

        // Creating new options list
        List<string> options = new List<string> { "1", "2", "3", "4", "5" };

        // Adding options
        playerCountTMPDropdown.AddOptions(options);
    }

    // Method is called when selected value changes
    void OnDropdownValueChanged(int selectedValue)
    {
        // Hide all UI elements by default
        HideAllElements();

        // Determine how many elements to show based on selected dropdown option
        // Option 0 shows 2 elements, Option 1 shows 4, etc. UI elements constist of text and input field
        int elementsToShow = (selectedValue + 1) * 2;

        // Show the appropriate number of elements
        ShowElements(elementsToShow);
    }

    // Method to hide all UI elements
    void HideAllElements()
    {
        foreach (GameObject element in playerNameInputs)
            element.SetActive(false);
    }

    // Method to show a specific number of elements
    void ShowElements(int n)
    {
        for (int i = 0; i < n && i < playerNameInputs.Length; ++i)
            playerNameInputs[i].SetActive(true);
    }
}
