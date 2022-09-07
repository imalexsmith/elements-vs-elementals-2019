using System.Collections.Generic;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
    public bool CanSelectGlobal { get; set; }

    public MapInfo MapInfoObj;
    public WalkMatrix WalkMatrixObj;
    public MoneyManager MoneyManagerObj;
    public BuildMenu BuildMenuObj;
    public AudioSource BuildSound;
    public GameObject PathBlockedText;
    public GameObject NoMoneyText;
    public float DeniedBuildInfoTime;
    public List<Tower> TowerPrefabs;
    public Tower[,] Towers;

    private Vector3Int _selectedCell;
    private float _hideDeniedBuildInfoTime;


    public Tower GetTowerPrefab(Element element)
    {
        foreach (var item in TowerPrefabs)
        {
            if ((item.Elements.FirstEl == element.FirstEl && item.Elements.SecondEl == element.SecondEl) ||
                (item.Elements.FirstEl == element.SecondEl && item.Elements.SecondEl == element.FirstEl))
                return item;
        }

        return null;
    }

    public Sprite GetTowerSprite(Element element)
    {
        foreach (var item in TowerPrefabs)
        {
            if ((item.Elements.FirstEl == element.FirstEl && item.Elements.SecondEl == element.SecondEl) ||
                (item.Elements.FirstEl == element.SecondEl && item.Elements.SecondEl == element.FirstEl))
                return item.GetComponent<SpriteRenderer>().sprite;
        }

        return null;
    }

    void Start()
    {
        CanSelectGlobal = false;
        Towers = new Tower[MapInfoObj.SizeX, MapInfoObj.SizeY];
    }

    void Update()
    {
        if (Time.time > _hideDeniedBuildInfoTime)
        {
            PathBlockedText.SetActive(false);
            NoMoneyText.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            BuildMenuObj.HideMenu();
            return;
        }

        if (CanSelectGlobal && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var mouseWorldP= ray.GetPoint(-ray.origin.z / ray.direction.z);
            var cellPos = MapInfoObj.BackLayer.WorldToCell(mouseWorldP);
           
            var tow = Towers[cellPos.x, cellPos.y];
            if (tow != null)
            {
                BuildMenuObj.Level = 1;
                BuildMenuObj.gameObject.SetActive(true);
                BuildMenuObj.UpdateLevelVisible();
                BuildMenuObj.transform.position = Camera.main.WorldToScreenPoint(MapInfoObj.GetTileCenter(cellPos));
                _selectedCell = cellPos;
                return;
            }

            if (!CanSelect(cellPos.x, cellPos.y))
            {
                PathBlockedText.SetActive(true);
                NoMoneyText.SetActive(false);
                _hideDeniedBuildInfoTime = Time.time + DeniedBuildInfoTime;
                return;
            }

            BuildMenuObj.Level = 0;
            BuildMenuObj.gameObject.SetActive(true);
            BuildMenuObj.UpdateLevelVisible();
            BuildMenuObj.transform.position = Camera.main.WorldToScreenPoint(MapInfoObj.GetTileCenter(cellPos));
            _selectedCell = cellPos;
        }
    }

    public bool Build(Tower tower)
    {
        if (MoneyManagerObj.MoneyCount < tower.MoneyPrice)
        {
            PathBlockedText.SetActive(false);
            NoMoneyText.SetActive(true);
            _hideDeniedBuildInfoTime = Time.time + DeniedBuildInfoTime;
            return false;
        }

        var oldTower = Towers[_selectedCell.x, _selectedCell.y];
        if (oldTower != null)
        {
            if (GetTowerPrefab(oldTower.Elements) == GetTowerPrefab(tower.Elements))
                return false;

            Destroy(oldTower.gameObject);
        }

        Towers[_selectedCell.x, _selectedCell.y] = Instantiate(tower, MapInfoObj.GetTileCenter(_selectedCell), Quaternion.identity);
        WalkMatrixObj.ConfirmBuilding();
        MoneyManagerObj.AddMoney(-tower.MoneyPrice);
        BuildSound.Play();

        return true;
    }

    private bool CanSelect(int x, int y)
    {
        return WalkMatrixObj.CanBuild(x, y);
    }
}
