using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // UI Elements
    // Starting screen
    public GameObject welcomeCanvas;
    public TMP_Dropdown numberOfPlayersDropdown;
    public List<GameObject> playerNamesText;
    public List<GameObject> playerNameInputs;
    public TMP_InputField startBalanceInput;
    public TMP_InputField minBetAmountInput;
    public TMP_InputField maxBetAmountInput;
    // Game screen
    public GameObject bettingCanvas;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI casinoBalanceText;
    public TextMeshProUGUI selectedNumberText;
    public TMP_InputField betAmountInput;
    public GameObject betButton;
    public GameObject nextPlayerButton;
    public GameObject spinButton;
    // End screen
    public GameObject restartCanvas;

    // Initializing dropdown with number of players selection
    void Start()
    {
        // Clearing existing options
        numberOfPlayersDropdown.ClearOptions();
        // Creating new options list
        List<string> options = new List<string> { "1", "2", "3", "4", "5" };
        // Adding options
        numberOfPlayersDropdown.AddOptions(options);

        // Adding a listener for when the dropdown value changes
        numberOfPlayersDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        // Ensure we start with the correct elements visible (based on default dropdown value)
        OnDropdownValueChanged(numberOfPlayersDropdown.value);
    }

    // Method is called when selected value of dropdown changes
    private void OnDropdownValueChanged(int selectedValue)
    {
        // Hide all dropdown elements
        for (int i = 0; i < playerNameInputs.Count; ++i)
        {
            playerNameInputs[i].SetActive(false);
            playerNamesText[i].SetActive(false);
        }

        // Option 0 shows 2 elements, Option 1 shows 4, etc. UI elements constist of text and input field
        // Show the appropriate number of elements
        for (int i = 0; i < selectedValue + 1 && i < playerNameInputs.Count; ++i)
        {
            playerNameInputs[i].SetActive(true);
            playerNamesText[i].SetActive(true);
        }
    }

    // Method for starting the game when all values have been initialized
    public void StartGame()
    {
        // Validating information
        if (!int.TryParse(minBetAmountInput.text, out int minBetAmount) || minBetAmount < 1)
        {
            Debug.Log("Enter correct minimal bet amount.");
            return;
        }
        if (!int.TryParse(maxBetAmountInput.text, out int maxBetAmount) || maxBetAmount < 1 || maxBetAmount < minBetAmount)
        {
            Debug.Log("Enter correct maximal bet amount.");
            return;
        }
        if (!int.TryParse(startBalanceInput.text, out int balance) || balance < 0 || balance < minBetAmount)
        {
            Debug.Log("Enter correct starting balance greater than minimal bet amount.");
            return;
        }
        // Switching UI elements from starting screen to game screen
        welcomeCanvas.SetActive(false);
        bettingCanvas.SetActive(true);

        // Assigning input values to objects
        int numberOfPlayers = numberOfPlayersDropdown.value + 1;
        int startingBalance = int.Parse(startBalanceInput.text);
        List<string> playerNames = new List<string>();
        // Filling list of player names
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            string name = playerNameInputs[i].GetComponent<TMP_InputField>().text;
            // If name is not specified - give player a generic one
            if (name == "")
                name = $"Player {i + 1}";
            playerNames.Add(name);
        }

        // Passing the values to GameManager
        FindObjectOfType<GameManager>().StartGame(numberOfPlayers, startingBalance, playerNames, minBetAmount, maxBetAmount);
    }

    // Methods for updating text information
    public void UpdateSelectedNumberUI(int selectedNumber)
    {
        if (selectedNumber > -1)
            selectedNumberText.text = $"Selected number: {selectedNumber.ToString()}";
        else
            selectedNumberText.text = "Selected number: ";
    }

    public void UpdatePlayerInfoUI(string name, int balance)
    {
        playerNameText.text = name;
        balanceText.text = $"Balance: ${balance}";
        betAmountInput.text = "";
    }

    public void UpdateCasinoBalanceUI(int balance)
    {
        casinoBalanceText.text = $"Casino balance: ${balance}";
    }

    // Methods for showing and hiding buttons
    public void ToggleBetButtonUI()
    {
        betButton.SetActive(!betButton.activeSelf);
    }

    public void ShowNextPlayerButtonUI()
    {
        nextPlayerButton.SetActive(true);
    }

    public void HideNextPlayerButtonUI()
    {
        nextPlayerButton.SetActive(false);
    }

    public void ShowSpinButtonUI()
    {
        spinButton.SetActive(true);
    }

    public void HideSpinButtonUI()
    {
        spinButton.SetActive(false);
    }

    // Methods for ending and restarting the game
    public void EndGame()
    {
        Debug.Log("All players lost their money. The game ends.");
        bettingCanvas.SetActive(false);
        restartCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game.");
        restartCanvas.SetActive(false);
        welcomeCanvas.SetActive(true);
    }
}
