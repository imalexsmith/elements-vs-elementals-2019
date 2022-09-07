using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[Serializable]
public enum ElementsBase
{
    None,
    Fire,
    Water,
    Earth,
    Air
}

[Serializable]
public class Element
{
    public ElementsBase FirstEl;
    public ElementsBase SecondEl;
}

public class Tower : MonoBehaviour
{
    public Element Elements;
    public float Dmg;
    public float FireDelay;
    public int MoneyPrice;
    public float Radius;
    public Effect TowerEffect;
    public Sprite InfoSprite;
    public TrailRenderer FireRenderer;
    public List<Enemy> SeekEnemies;

    private float _fireTimer;


    void Update()
    {
        if (SeekEnemies.Count > 0)
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= FireDelay)
            {
                var e = SeekEnemies[0];
                e.ApplyDmg(Dmg);
                if (TowerEffect != null)
                    e.ApplyEffect(TowerEffect);
                FireRenderer.Clear();
                FireRenderer.AddPosition(transform.position);
                FireRenderer.AddPosition(e.transform.position);
                
                _fireTimer = 0f;
            }

            return;
        }

        _fireTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var e = collision.GetComponent<Enemy>();
        if (e != null)
        {
            SeekEnemies.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var e = collision.GetComponent<Enemy>();
        if (e != null && SeekEnemies.Contains(e))
        {
            SeekEnemies.Remove(e);
        }
    }
}
