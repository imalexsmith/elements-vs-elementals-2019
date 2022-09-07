using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public static int Count;


    public MapInfo MapInfoObj;
    public WalkMatrix WalkMatrixObj;
    public Castle CastleObj;
    public MoneyManager MoneyManagerObj;

    public float Speed = 1f;
    public float Health = 100f;
    public int MoneyReward;

    public List<Effect> Effects;

    private int _nextPosX;
    private int _nextPosY;
    private Vector3 _nextPos;
    private AudioSource _audioS;
    private ParticleSystem _deadPart;

    void Start()
    {
        Count++;
        _nextPos = transform.position;
        FindNext();
        _audioS = GetComponent<AudioSource>();
        _deadPart = GetComponentInChildren<ParticleSystem>();
    }

    private void OnDestroy()
    {
        Count--;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _nextPos, Time.deltaTime * Speed);

        if (Vector3.Distance(transform.position, _nextPos) < 0.01f)
        {
            FindNext();
        }
    }

    public void ApplyDmg(float value)
    {
        Health -= value;

        _audioS.Play();
        if (Health <= 0)
        {
            MoneyManagerObj.AddMoney(MoneyReward);

            Kill();
        }
    }

    public void ApplyEffect(Effect effect)
    {
        var eff = Instantiate(effect, transform);
        Effects.Add(eff);
        eff.OnApply(this);
    }

    public void Kill()
    {
        _audioS.transform.SetParent(null);
        _deadPart.Play();
        _deadPart.transform.SetParent(null);
        Destroy(_deadPart.gameObject, _deadPart.main.duration);
        Destroy(_audioS, _audioS.clip.length);
        Destroy(gameObject);
    }

    private void FindNext()
    {
        var cellPos = MapInfoObj.BackLayer.WorldToCell(_nextPos);
        if (cellPos.x == MapInfoObj.EndX && cellPos.y == MapInfoObj.EndY)
        {
            CastleObj.DoDmg();
            Kill();
            return;
        }

        var dp = WalkMatrixObj.dp[cellPos.x, cellPos.y];


        if (cellPos.x > 0)
            if (WalkMatrixObj.dp[cellPos.x - 1, cellPos.y] == dp - 1)
            {
                _nextPos = MapInfoObj.BackLayer.GetCellCenterWorld(new Vector3Int(cellPos.x - 1, cellPos.y, 0));
                return;
            }

        if (cellPos.x < MapInfoObj.SizeX - 1)
            if (WalkMatrixObj.dp[cellPos.x + 1, cellPos.y] == dp - 1)
            {
                _nextPos = MapInfoObj.BackLayer.GetCellCenterWorld(new Vector3Int(cellPos.x + 1, cellPos.y, 0));
                return;
            }

        if (cellPos.y > 0)
            if (WalkMatrixObj.dp[cellPos.x, cellPos.y - 1] == dp - 1)
            {
                _nextPos = MapInfoObj.BackLayer.GetCellCenterWorld(new Vector3Int(cellPos.x, cellPos.y - 1, 0));
                return;
            }

        if (cellPos.y < MapInfoObj.SizeY - 1)
            if (WalkMatrixObj.dp[cellPos.x, cellPos.y + 1] == dp - 1)
            {
                _nextPos = MapInfoObj.BackLayer.GetCellCenterWorld(new Vector3Int(cellPos.x, cellPos.y + 1, 0));
                return;
            }
    }
}
