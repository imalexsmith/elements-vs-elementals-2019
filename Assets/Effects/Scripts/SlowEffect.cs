using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : Effect
{
    public float SpeedDecreeze;

    private float defaultSpeed;

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);

        defaultSpeed = enemy.Speed;
        enemy.Speed = defaultSpeed * SpeedDecreeze;
    }

    void Update()
    {
        if ((Time.time - StartTime) >= Duration)
        {
            TargEnemy.Speed = defaultSpeed;
            Destroy(gameObject);
        }
    }
}
