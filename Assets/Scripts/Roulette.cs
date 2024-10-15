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
        FindObjectOfType<UIManager>().Log($"Winning number: {winningNumber}.");
        ChangeSprite(winningNumber);

        // Calculating winners
        FindObjectOfType<GameManager>().CalculateResults(winningNumber);
    }

    private void ChangeSprite(int number)
    {
        // Selecting path for correct sprite
        string path;
        switch (number)
        {
            case 0: path = "Sprites/Roulette0"; break;
            case 1: path = "Sprites/Roulette1"; break;
            case 2: path = "Sprites/Roulette2"; break;
            case 3: path = "Sprites/Roulette3"; break;
            case 4: path = "Sprites/Roulette4"; break;
            case 5: path = "Sprites/Roulette5"; break;
            case 6: path = "Sprites/Roulette6"; break;
            case 7: path = "Sprites/Roulette7"; break;
            case 8: path = "Sprites/Roulette8"; break;
            case 9: path = "Sprites/Roulette9"; break;
            default: path = "Sprites/Roulette"; break;
        }

        // Getting sprite renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Loading sprite
        if (spriteRenderer != null)
            spriteRenderer.sprite = Resources.Load<Sprite>(path);
        else
            Debug.LogError("Sprite not found in Resources!");
    }
}
