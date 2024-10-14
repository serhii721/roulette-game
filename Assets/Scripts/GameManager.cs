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
    public List<TMP_InputField> playerNames;
    
    private List<Player> players = new List<Player>();
    private Dictionary<int, List<PlayerBet>> allBets = new Dictionary<int, List<PlayerBet>>();

    public int casinoBalance = 0;

    private void StartGame(int playerNumber = 5)
    {
        // Adding players
        for (int i = 0; i < playerNumber; ++i)
            players.Add(new Player(playerNames[i].ToString(), 1000));
        
        // Adding bets
        for (int i = 0; i < 10; ++i)
            allBets[i] = new List<PlayerBet>();
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
