using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BettingManager : MonoBehaviour
{
    public TMP_InputField betAmountInput;
    public TextMeshProUGUI balanceText;
    private Player currentPlayer;

    private void Start()
    {
        {
            // TODO: specify name and balance
            currentPlayer = new Player("Player1", 1000);
            UpdateBalanceUI();
        }
    }

    public void PlaceBet(int number)
    {
        if (int.TryParse(betAmountInput.text, out int betAmount) && betAmount > 0)
        {
            currentPlayer.PlaceBet(number, betAmount);
            UpdateBalanceUI();
        }
        else
            Debug.Log("Enter correct bet amount.");
    }

    void UpdateBalanceUI()
    {
        balanceText.text = "Balance: " + currentPlayer.balance.ToString();
    }
}
