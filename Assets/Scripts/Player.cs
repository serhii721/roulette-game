using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public int balance;
    public Dictionary<int, int> bets = new Dictionary<int, int>(); // Roulette number and bet amount

    public Player(string playerName, int initialBalance)
    {
        name = playerName;
        balance = initialBalance;
        for (int i = 0; i < 10; ++i)
            bets[i] = 0;
    }

    public void PlaceBet(int number, int amount)
    {
        if (balance >= amount)
        {
            bets[number] = amount;
            balance -= amount;
        }
        else
            Debug.Log("Insufficient balance to place bet");
    }

    public void GainWinnings(int amount)
    {
        balance += amount;
    }
}
