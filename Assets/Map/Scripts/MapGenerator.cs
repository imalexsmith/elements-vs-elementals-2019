using UnityEngine;
using UnityEngine.Tilemaps;


public class MapGenerator : MonoBehaviour
{
    public MapInfo MapInfoObj;
    public Tile BackgroundTile;
    public Tile StartTile;
    public Tile EndTile;


    void Start()
    {
        var x = MapInfoObj.SizeX;
        var y = MapInfoObj.SizeY;
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                MapInfoObj.BackLayer.SetTile(new Vector3Int(i, j, 0), BackgroundTile);
            }
        }

        MapInfoObj.BackLayer.SetTile(new Vector3Int(MapInfoObj.StartX, MapInfoObj.StartY, 0), StartTile);
        MapInfoObj.BackLayer.SetTile(new Vector3Int(MapInfoObj.EndX, MapInfoObj.EndY, 0), EndTile);

        Camera.main.transform.position = new Vector3(x / 2f, y / 2f, Camera.main.transform.position.z);
    }
}
