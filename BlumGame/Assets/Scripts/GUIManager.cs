using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{

    private int _coinScore = 0;
    [SerializeField] TextMeshProUGUI _coinText;
    private int _health = 3;
    [SerializeField] private Image[] _hearths;

    public void AddCoinScore()
    {
        _coinScore++;
        UpdateGUI();
    }

    public void LoseHealth()
    {
        if(_health > 0)
        {
            _health--;
            _hearths[_health].gameObject.SetActive(false);
        }
    }

    private void UpdateGUI()
    {
        _coinText.text = "Coins: " + _coinScore;
    }
}
