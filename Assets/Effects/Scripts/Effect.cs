using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float Duration;

    protected float StartTime;
    protected Enemy TargEnemy;

    public virtual void OnApply(Enemy enemy)
    {
        StartTime = Time.time;
        TargEnemy = enemy;
    }
}
