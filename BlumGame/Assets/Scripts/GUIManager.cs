using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{

    private int coinScore = 0;
    [SerializeField] TextMeshProUGUI coinText;
    private int health = 3;
    [SerializeField] private Image[] hearths;

    public void AddCoinScore()
    {
        coinScore++;
        coinText.text = "Coins: " + coinScore;
    }

    public void LoseHealth()
    {
        if(health > 0)
        {
            health--;
            hearths[health].gameObject.SetActive(false);
        }
    }
}
