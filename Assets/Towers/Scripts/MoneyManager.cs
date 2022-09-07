using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int MoneyCount;
    public Text MoneyText;

    private void Start()
    {
        MoneyText.text = MoneyCount.ToString();
    }

    public void AddMoney(int count)
    {
        MoneyCount += count;
        MoneyText.text = MoneyCount.ToString();
    }
}
