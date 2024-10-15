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
    private List<Player> players = new List<Player>();
    private Dictionary<int, List<PlayerBet>> allBets = new Dictionary<int, List<PlayerBet>>();
    private int currentPlayerIndex = 0;
    private int numberOfPlayers;

    public int casinoBalance = 0;

    // UI Elements
    public List<TMP_InputField> playerNameInputs;
    public TMP_InputField startBalanceInput;
    public TMP_Dropdown numberOfPlayersDropdown;
    public GameObject welcomeCanvas;
    public GameObject bettingCanvas;

    public void StartGame()
    {
        // Intitializing values
        numberOfPlayers = numberOfPlayersDropdown.value + 1;
        int startBalance = int.Parse(startBalanceInput.text);
        // Adding players
        for (int i = 0; i < numberOfPlayers; ++i)
            players.Add(new Player(playerNameInputs[i].text, startBalance));
        
        // Adding bets
        for (int i = 0; i < 10; ++i)
            allBets[i] = new List<PlayerBet>();

        // Switching UI elements from starting screen to game screen
        welcomeCanvas.SetActive(false);
        bettingCanvas.SetActive(true);

        // Starting betting
        FindObjectOfType<BettingManager>().StartBetting(players[0]);
    }

    public void AddBet(Player player, int number, int amount)
    {
        if (allBets.ContainsKey(number))
        {
            allBets[number].Add(new PlayerBet { player = player, amount = amount });
            Debug.Log($"{player.name} made a bet of {amount} on number {number}");
        }
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

        // Distributing winnings
        if (totalBetsOnWinningNumber > 0)
        {
            // Calculating winners` shares
            foreach (var winner in winners)
            {
                float winnerShare = (float)winner.Value / totalBetsOnWinningNumber;
                int prize = (int)(totalBets * winnerShare);
                winner.Key.GainWinnings(prize);
                Debug.Log($"{winner.Key.name} won {prize}.");
            }
            // Outputing information about losers
            foreach (Player player in players)
            {
                if (player.bets[winningNumber] == 0)
                {
                    int amount = 0;
                    for (int i = 0; i < 10; ++i)
                        amount += player.bets[i];
                    Debug.Log($"{player.name} lost {amount}.");
                }
            }
        }
        else
        {
            Debug.Log($"No bets on winning number. Casino wins {totalBets}.");
            casinoBalance += totalBets;
        }

        // Clearing player's bets
        foreach (Player player in players)
        {
            player.bets.Clear();
        }
    }
}
