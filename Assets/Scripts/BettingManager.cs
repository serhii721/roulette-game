using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BettingManager : MonoBehaviour
{
    public TMP_InputField betAmountInput;
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI selectedNumberText;
    private Player currentPlayer;
    private int selectedNumber = -1;

    private void StartBetting(Player player)
    {
        currentPlayer = player;
        UpdateBalanceUI();
        UpdateSelectedNumberUI();
    }

    public void SelectBettingNumber(int n)
    {
        selectedNumber = n;
        UpdateSelectedNumberUI();
    }

    public void PlaceBet()
    {
        if (selectedNumber < 0)
        {
            Debug.Log("Select number");
            return;
        }

        if (int.TryParse(betAmountInput.text, out int betAmount) && betAmount > 0)
        {
            if (currentPlayer.balance >= betAmount)
            {
                currentPlayer.PlaceBet(selectedNumber, betAmount);
                FindObjectOfType<GameManager>().AddBet(currentPlayer, selectedNumber, betAmount);
                UpdateBalanceUI();
            }
            else
                Debug.Log("Insufficient balance to place bet");
        }
        else
            Debug.Log("Enter correct bet amount");
    }

    private void UpdateSelectedNumberUI()
    {
        if (selectedNumber > -1)
            selectedNumberText.text = $"Selected number: {selectedNumber.ToString()}";
        else
            selectedNumberText.text = "Selected number: ";
    }

    private void UpdateBalanceUI()
    {
        balanceText.text = $"Balance: {currentPlayer.balance.ToString()}";
    }
}
