using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBet
{
    public Player player;
    public int amount;
}

public class GameManager : MonoBehaviour
{
    public UIManager IM; // Interface Manager used to interact with user inputs and change text on UI elements
    private List<Player> players;
    private int minBetAmount;
    private int maxBetAmount;
    private int currentPlayerIndex;
    private int selectedNumber;
    private int casinoBalance;

    private string buffer; // Used for log information

    public void StartGame(int numberOfPlayers, int startingBalance, List<string> playerNames, int minBet, int maxBet)
    {
        // Initializing values
        players = new List<Player>();
        minBetAmount = minBet;
        maxBetAmount = maxBet;
        currentPlayerIndex = 0;
        selectedNumber = -1;
        casinoBalance = 0;
        // Adding players
        for (int i = 0; i < numberOfPlayers; ++i)
            players.Add(new Player(playerNames[i], startingBalance));

        // Showing info to player
        IM.UpdatePlayerInfoUI(getCurrentPlayer().name, getCurrentPlayer().balance);
    }

    public void SelectBettingNumber(int n)
    {
        if (n < 0 || n > 9)
        {
            IM.Log("Incorrect number for selection");
            return;
        }

        selectedNumber = n;
        IM.UpdateSelectedNumberUI(selectedNumber);
    }

    public void PlaceBet()
    {
        // 1. Validating information
        // Checking for correct selected number
        if (selectedNumber < 0)
        {
            IM.Log("Select number");
            return;
        }

        // Checking for correct input of bet amount
        if (!int.TryParse(IM.betAmountInput.text, out int betAmount) || betAmount < 1)
        {
            IM.Log("Enter correct bet amount");
            return;
        }
        if (betAmount < minBetAmount)
        {
            IM.Log($"Enter bet amount greater that minimal bet amount of {minBetAmount}");
            return;
        }
        if (betAmount > maxBetAmount)
        {
            IM.Log($"Enter bet amount lesser that maximal bet amount of {maxBetAmount}");
            return;
        }

        // Checking for sufficient player's balance
        if (getCurrentPlayer().balance < betAmount)
        {
            IM.Log("Insufficient balance to place bet");
            return;
        }
        
        // 2. Placing bet
        getCurrentPlayer().PlaceBet(selectedNumber, betAmount);
        IM.Log($"Player \"{getCurrentPlayer().name}\" made a bet of ${betAmount} on number {selectedNumber}");

        // Updating UI
        selectedNumber = -1;
        // If player doesn't have any balance left - go to next player or spinning part
        if (getCurrentPlayer().balance < 1)
        {
            NextPlayer();
            return;
        }

        // If there are other players left - show button to skip to the next player
        if (currentPlayerIndex < players.Count - 1)
            IM.ShowNextPlayerButtonUI();
        else // Show button to spin
            IM.ShowSpinButtonUI();

        IM.UpdateSelectedNumberUI(selectedNumber);
        IM.UpdatePlayerInfoUI(getCurrentPlayer().name, getCurrentPlayer().balance);
    }

    public void NextPlayer()
    {
        if (currentPlayerIndex < players.Count - 1)
            currentPlayerIndex++;
        else
        {
            IM.ToggleBetButtonUI();
            IM.ShowSpinButtonUI();
        }
        selectedNumber = -1;
        IM.HideNextPlayerButtonUI();
        IM.UpdateSelectedNumberUI(selectedNumber);
        IM.UpdatePlayerInfoUI(getCurrentPlayer().name, getCurrentPlayer().balance);
    }

    public void CalculateResults(int winningNumber)
    {
        Dictionary<Player, int> winners = new Dictionary<Player, int>();
        int totalBets = 0;
        int totalBetsOnWinningNumber = 0;

        // Calculating total bets on each number and on winning number separately
        foreach (Player player in players)
        {
            for (int i = 0; i < 10; ++i)
            {
                if (player.bets[i] > 0)
                {
                    totalBets += player.bets[i];
                    if (i == winningNumber)
                    {
                        totalBetsOnWinningNumber += player.bets[i];
                        winners.Add(player, player.bets[i]);
                    }
                }
            }
        }

        // Outputing information about losers
        foreach (Player player in players)
        {
            if (player.bets[winningNumber] == 0)
            {
                int amount = 0;
                for (int i = 0; i < 10; ++i)
                    amount += player.bets[i];
                IM.Log($"Player \"{player.name}\" lost ${amount}.");
            }
        }

        // Distributing winnings
        if (totalBetsOnWinningNumber > 0)
        {
            // Calculating winners` shares
            foreach (var winner in winners)
            {
                float winnerShare = (float)winner.Value / totalBetsOnWinningNumber;
                int prize = (int)(totalBets * winnerShare);
                winner.Key.GainWinnings(prize);
                IM.Log($"Player \"{winner.Key.name}\" won ${prize}.");
            }
        }
        else
        {
            IM.Log($"No bets on winning number. Casino wins ${totalBets}.");
            casinoBalance += totalBets;
        }

        // Updating values
        // Removing players with no balance left
        for (int i = players.Count - 1; i >= 0; --i)
        {
            if (players[i].balance < minBetAmount)
            {
                IM.Log($"Player \"{players[i].name}\" has not enough money to play on this table and is removed.");
                players.RemoveAt(i);
            }
        }
        // Checking if game continues
        if (players.Count < 1)
        {
            IM.EndGame();
            return;
        }
        // Clearing bets
        foreach (Player player in players)
        {
            player.bets = new Dictionary<int, int>();
        }
        currentPlayerIndex = 0;
        selectedNumber = -1;
        // Updating UI
        IM.UpdatePlayerInfoUI(getCurrentPlayer().name, getCurrentPlayer().balance);
        IM.UpdateSelectedNumberUI(selectedNumber);
        IM.UpdateCasinoBalanceUI(casinoBalance);
    }

    private Player getCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
}
