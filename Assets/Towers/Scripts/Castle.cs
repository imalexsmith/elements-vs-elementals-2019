using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public int Health;
    public Text HealthText;

    public void DoDmg()
    {
        Health -= 1;
        HealthText.text = Health.ToString();
        if (Health <= 0)
            Debug.Log("GAME OVER!!!");
    }

    void Start()
    {
        HealthText.text = Health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
