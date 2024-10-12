using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    public float spinSpeed = 1000f;
    public bool isSpinning = false;
    private float spinTime = 2f;

    public void Spin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinRoulette());
        }
    }

    private IEnumerator SpinRoulette()
    {
        float elapsedTime = 0f;
        while (elapsedTime < spinTime)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSpinning = false;

        int winningNumber = Random.Range(0, 10);
        Debug.Log("Winning number: " + winningNumber);

        FindObjectOfType<GameManager>().CalculateResults(winningNumber);
    }
}
