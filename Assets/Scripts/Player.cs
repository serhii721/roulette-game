using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public int balance;
    public Dictionary<int, int> bets = new Dictionary<int, int>(); // Roulette number and bet amount

    public Player(string namee, int initialBalance)
    {
        name = namee;
        balance = initialBalance;
    }

    public void PlaceBet(int number, int amount)
    {
        if (balance >= amount)
        {
            if (bets.ContainsKey(number))
                bets[number] += amount;
            else
                bets[number] = amount;
            balance -= amount;
        }
        else
            Debug.Log("Insufficient balance to place bet");
    }
}
