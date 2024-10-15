using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    public float spinSpeed = 1000f;
    public bool isSpinning = false;
    private float spinTime = 2f;

    // Method is called when all bets are made player starts roulette spinning
    public void Spin()
    {
        // Hiding spin button, showing bet button
        UIManager IM = FindObjectOfType<UIManager>();
        IM.ToggleBetButtonUI();
        IM.HideSpinButtonUI();
        IM.HideNextPlayerButtonUI();

        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinRoulette());
        }
    }

    private IEnumerator SpinRoulette()
    {
        // Roulette's spinning animation
        float elapsedTime = 0f;
        while (elapsedTime < spinTime)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSpinning = false;

        // Generating random winning number
        int winningNumber = Random.Range(0, 10);
        Debug.Log($"Winning number: {winningNumber}.");

        // Calculating winners
        FindObjectOfType<GameManager>().CalculateResults(winningNumber);
    }
}
