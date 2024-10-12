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
    private int number = -1;

    private void Start()
    {
        {
            // TODO: specify name and balance
            currentPlayer = new Player("Player1", 1000);
            UpdateBalanceUI();
            UpdateSelectedNumberUI();
        }
    }

    public void SelectBettingNumber(int n)
    {
        number = n;
        UpdateSelectedNumberUI();
    }

    public void PlaceBet()
    {
        if (number < 0)
        {
            Debug.Log("Select number");
            return;
        }

        if (int.TryParse(betAmountInput.text, out int betAmount) && betAmount > 0)
        {
            if (currentPlayer.balance >= betAmount)
            {
                currentPlayer.PlaceBet(number, betAmount);
                FindObjectOfType<GameManager>().AddBet(currentPlayer, number, betAmount);
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
        if (number > -1)
            selectedNumberText.text = "Selected number: " + number.ToString();
        else
            selectedNumberText.text = "Selected number: ";
    }

    private void UpdateBalanceUI()
    {
        balanceText.text = "Balance: " + currentPlayer.balance.ToString();
    }
}
