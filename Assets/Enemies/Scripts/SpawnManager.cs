using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SpawnWave
{
    public float Delay;
    public Enemy EnemyPrefab;
    public int EnemyCount;
    public float MiniDelay;
}

public class SpawnManager : MonoBehaviour
{
    public bool AllSpawned { get; private set; }
    public MapInfo MapInfoObj;
    public WalkMatrix WalkMatrixObj;
    public Castle CastleObj;
    public MoneyManager MoneyManagerObj;
    public Text WaveIndexText;

    public SpawnWave[] Waves;

    private Vector3 _startWorldPos;
    private int _currentWaveIndex;
    private float _waveSpawnTimer;


    void Start()
    {
        _startWorldPos = MapInfoObj.GetTileCenter(new Vector3Int(MapInfoObj.StartX, MapInfoObj.StartY, 0));
        WaveIndexText.text = string.Format("{0} / {1}", _currentWaveIndex.ToString(), Waves.Length);
    }

    void Update()
    {
        if (_currentWaveIndex >= Waves.Length)
        {
            enabled = false;
            AllSpawned = true;
            return;
        }

        _waveSpawnTimer += Time.deltaTime;
        if (_waveSpawnTimer >= Waves[_currentWaveIndex].Delay)
        {
            StartCoroutine(StartSpawnWave(Waves[_currentWaveIndex]));

            _currentWaveIndex++;
            _waveSpawnTimer = 0f;

            WaveIndexText.text = string.Format("{0} / {1}", _currentWaveIndex.ToString(), Waves.Length);
        }
    }

    IEnumerator StartSpawnWave(SpawnWave wave)
    {
        for (int i = 0; i < wave.EnemyCount; i++)
        {
            var e = Instantiate(wave.EnemyPrefab, _startWorldPos, Quaternion.identity);
            e.MapInfoObj = MapInfoObj;
            e.WalkMatrixObj = WalkMatrixObj;
            e.CastleObj = CastleObj;
            e.MoneyManagerObj = MoneyManagerObj;
            yield return new WaitForSeconds(wave.MiniDelay);
        }
    }
}

//public static class TilemapExtensions
//{
//    public static T[] GetTiles<T>(this Tilemap tilemap) where T : TileBase
//    {
//        List<T> tiles = new List<T>();

//        for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
//        {
//            for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
//            {
//                T tile = tilemap.GetTile<T>(new Vector3Int(x, y, 0));
//                if (tile != null)
//                {
//                    tiles.Add(tile);
//                }
//            }
//        }
//        return tiles.ToArray();
//    }
//}
