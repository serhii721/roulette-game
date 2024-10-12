using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public Dictionary<int, List<PlayerBet>> allBets;
    public int casinoBalance = 0;

    private void Start()
    {
        allBets = new Dictionary<int, List<PlayerBet>>();
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
        int totalBetOnWinningNumber = 0;
        Dictionary<Player, int> winners = new Dictionary<Player, int>();

        foreach(Player player in players)
        {
            if (player.bets.ContainsKey(winningNumber))
            {
                int betAmount = player.bets[winningNumber];
                totalBetOnWinningNumber += betAmount;
                winners.Add(player, betAmount);
            }
        }

        if (totalBetOnWinningNumber > 0)
        {
            foreach (var winner in winners)
            {
                int prize = casinoBalance * winner.Value / totalBetOnWinningNumber;
                winner.Key.balance += prize;
                Debug.Log($"{winner.Key.name} won {prize}");
            }
            casinoBalance = 0;
        }
        else
        {
            //Debug.Log("No bets on winning number. Casino wins all.");
            foreach (Player player in players)
            {
                foreach (var bet in player.bets)
                {
                    casinoBalance += bet.Value;
                }
            }
        }

        foreach (Player player in players)
        {
            player.bets.Clear();
        }
    }
}

public class PlayerBet
{
    public Player player;
    public int amount;
}