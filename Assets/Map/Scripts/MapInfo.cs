using UnityEngine;
using UnityEngine.Tilemaps;


public class MapInfo : MonoBehaviour
{
    public int SizeX;
    public int SizeY;
    public int StartX;
    public int StartY;
    public int EndX;
    public int EndY;
    public Tilemap BackLayer;

    public Vector3 GetTileCenter(Vector3Int position)
    {
        return BackLayer.GetCellCenterWorld(position);
    }
}
