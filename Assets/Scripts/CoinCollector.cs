using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector instance;
    public TextMeshProUGUI text;
    int coinScore;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ChangeCoinScore(int coinValue)
    {
        coinScore += coinValue;
        text.text = "X" + coinScore.ToString();
    }
}
