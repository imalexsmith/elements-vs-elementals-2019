using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : Effect
{
    public float FullDmg;

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);
    }

    void Update()
    {
        if ((Time.time - StartTime) < Duration)
        {
            TargEnemy.ApplyDmg(FullDmg / Duration * Time.deltaTime);
            return;
        }

        Destroy(gameObject);
    }
}
