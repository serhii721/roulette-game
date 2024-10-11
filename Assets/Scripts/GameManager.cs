using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    private int winningNumber;
    public int casinoBalance = 0;

    public void SpinRoulette()
    {
        winningNumber = Random.Range(0, 10);
        Debug.Log("Winning number: " + winningNumber);
        CalculateResults();
    }

    public void CalculateResults()
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
                Debug.Log($"{winner.Key.playerName} won {prize}");
            }
            casinoBalance = 0;
        }
        else
        {
            Debug.Log("No bets on winning number. Casino wins all.");
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
