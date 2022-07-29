using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class ConteinerItemSlot : MonoBehaviour
{ 
    [SerializeField]
    private GameObject _itemSlot;

    [SerializeField]
    private TMP_Text _itemName;

    [SerializeField]
    private TMP_Text _itemCost;

    [SerializeField]
    private Button _openInfoButton;

    [SerializeField]
    private PanelItemInfo _panelItemInfo;

    private CatalogItem _catalogItem;
    private bool _showInfo = false;
    public TMP_Text ItemName => _itemName;
    public TMP_Text ItemCost => _itemCost;
    private void Start()
    {
        _openInfoButton.onClick.AddListener(ShowItemInfo);
    }

    private void ShowItemInfo()
    {
        Instantiate(_panelItemInfo, this.transform).ShowItemInfo(_catalogItem);
    }

    public void ShowItemSlot(string itemName, string itemCost, CatalogItem catalogItem)
    {
        _itemName.text = itemName;
        _itemCost.text = itemCost;
        _catalogItem = catalogItem;

        //Instantiate(_itemSlot, parentTransform);
    }
}
