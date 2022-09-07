using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public Castle CastleObj;
    public SpawnManager SpawnManagerObj;

    public GameObject ImageBlock;
    public GameObject WinText;
    public GameObject LoseText;

    // Update is called once per frame
    void Update()
    {
        if (CastleObj.Health <= 0)
        {
            ImageBlock.SetActive(true);
            LoseText.SetActive(true);
            enabled = false;
            return;
        }

        if (SpawnManagerObj.AllSpawned && Enemy.Count <= 0)
        {
            ImageBlock.SetActive(true);
            WinText.SetActive(true);
            enabled = false;
            return;
        }
    }
}
