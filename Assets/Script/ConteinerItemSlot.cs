using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private string _typeCurrency;
    private uint _costItem;

    private void Start()
    {
        _openInfoButton.onClick.AddListener(ShowItemInfo);
    }

    private void ShowItemInfo()
    {
        Instantiate(_panelItemInfo, this.transform).ShowItemInfo(_catalogItem);
    }

    private void BuyItem()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = _catalogItem.CatalogVersion,
            ItemId = _catalogItem.ItemId,
            Price = (int)_costItem,
            VirtualCurrency = _typeCurrency
        },
        result => 
        {
            Debug.Log($"Buy complete");
        },
        OnLoginError);
        GetInventory();
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.Log($"{error.ErrorMessage}");
    }

    public void ShowItemSlot(string itemName, string itemCost, uint cost, string currency, CatalogItem catalogItem)
    {
        _itemName.text = itemName;
        _itemCost.text = itemCost;
        _catalogItem = catalogItem;

        _typeCurrency = currency;
        _costItem = cost;
    }
    public void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => ShowInventory(result.Inventory), OnLoginError);
    }

    public void ShowInventory(List<ItemInstance> items)
    {
        foreach(var item in items)
            Debug.Log($"{item.ItemId}");
    }
}
