using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildMenu : MonoBehaviour
{
    public TowerManager TowerManagerObj;
    public Element Elements;
    public Sprite ElementsEmpty;
    public int Level;

    public List<GameObject> Level0GO;
    public List<GameObject> Level1GO;

    public Image FirstElementImage;
    public Image SecondElementImage;
    public Image CombinedTowerImage;
    public Image RadiusImage;
    public GameObject InfoPanel;
    public Image InfoImage;
    public Image DeniedBuild;
    public AudioSource DeniedSound;

    private Tower _tower;
    private float _hideDeniedImageTime;


    private void Start()
    {
        HideMenu();
    }

    public void AddBaseElement(ElementButton elB)
    {
        if (Elements.FirstEl == ElementsBase.None)
        {
            Elements.FirstEl = elB.Element;
            FirstElementImage.sprite = TowerManagerObj.GetTowerSprite(new Element { FirstEl = elB.Element });
            UpdateCombined();
            return;
        }

        if (Elements.SecondEl == ElementsBase.None)
        {
            Elements.SecondEl = elB.Element;
            SecondElementImage.sprite = TowerManagerObj.GetTowerSprite(new Element { FirstEl = elB.Element });
            UpdateCombined();
            return;
        }
    }

    public void RemoveFirstElement()
    {
        Elements.FirstEl = ElementsBase.None;
        FirstElementImage.sprite = ElementsEmpty;
        UpdateCombined();
    }

    public void RemoveSecondElement()
    {
        Elements.SecondEl = ElementsBase.None;
        SecondElementImage.sprite = ElementsEmpty;
        UpdateCombined();
    }

    public void HideMenu()
    {
        TowerManagerObj.CanSelectGlobal = true;
        RemoveFirstElement();
        RemoveSecondElement();
        gameObject.SetActive(false);
        InfoPanel.SetActive(false);
    }

    public void UpdateLevelVisible()
    {
        switch (Level)
        {
            case 0:
                foreach (var item in Level0GO)
                    item.SetActive(true);
                foreach (var item in Level1GO)
                    item.SetActive(false);
                break;
            case 1:
                foreach (var item in Level0GO)
                    item.SetActive(false);
                foreach (var item in Level1GO)
                    item.SetActive(true);
                break;
            default:
                break;
        }

        RemoveFirstElement();
        RemoveSecondElement();
    }

    public void UpdateCombined()
    {
        _tower = TowerManagerObj.GetTowerPrefab(Elements);
        CombinedTowerImage.sprite = TowerManagerObj.GetTowerSprite(Elements);
        InfoPanel.SetActive(true);
        InfoImage.sprite = _tower.InfoSprite;
        RadiusImage.rectTransform.localScale = Vector3.one * _tower.Radius * 2f;
    }

    public void Build()
    {
        var result = TowerManagerObj.Build(_tower);
        if (result)
            HideMenu();
        else
        {
            DeniedBuild.gameObject.SetActive(true);
            DeniedSound.Play();
            _hideDeniedImageTime = Time.time + 0.25f;
            StartCoroutine(HideDenied());
        }
    }

    IEnumerator HideDenied()
    {
        while (Time.time < _hideDeniedImageTime)
        {
            yield return null;
        }
        DeniedBuild.gameObject.SetActive(false);
    }
}
