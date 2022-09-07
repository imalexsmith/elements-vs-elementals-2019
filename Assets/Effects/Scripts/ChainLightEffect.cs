using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightEffect : Effect
{
    public TrailRenderer FireRenderer;
    public int ChainCount;
    public float Dmg;
    public LayerMask enemyLayerMask;

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);

        enemy.ApplyDmg(Dmg);

       
        var thisCollider = GetComponent<CircleCollider2D>();
        var enemies = new List<Enemy>();
        Collider2D[] enemyColliders = Physics2D.OverlapAreaAll(thisCollider.bounds.min, thisCollider.bounds.max, enemyLayerMask);
        foreach (Collider2D enemyCol in enemyColliders)
        {
            var e = enemyCol.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                enemies.Add(e);
            }
        }

        var indexes = new List<int>(ChainCount);

        for (int i = 0; i < ChainCount; i++)
        {
            indexes.Add(Random.Range(0, enemies.Count));
        }

        FireRenderer.Clear();
        FireRenderer.AddPosition(transform.position);
        for (int i = 0; i < ChainCount; i++)
        {
            enemies[indexes[i]].ApplyDmg(Dmg);
            
            FireRenderer.AddPosition(enemies[indexes[i]].transform.position);
        }
    }
    
    void Update()
    {
        if ((Time.time - StartTime) >= Duration)
        {
            Destroy(gameObject);
            return;
        }
    }
}
