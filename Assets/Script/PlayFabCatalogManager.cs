using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabCatalogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;

    [SerializeField]
    private ConteinerItemSlot _conteinerItemSlot;

    private List<ConteinerItemSlot> _items;
    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnGetCatalogError);
        _items = new List<ConteinerItemSlot>();
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        HandleResult(result.Catalog);
        Debug.Log($"Catalog was loaded successfully!");

    }

    private void HandleResult(List<CatalogItem> catalog)
    {
        foreach(var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            CreateSlot();
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
        }

        SetDataInSlot(catalog);
    }

    private void CreateSlot()
    {
        var slotCreate = GameObject.Instantiate(_conteinerItemSlot, _parent.transform);
        _items.Add(slotCreate);
    }

    private void SetDataInSlot(List<CatalogItem> catalog)
    {
        for(var i = 0; i < catalog.Count; i++)
        {
            string priceString =  $"";
            foreach(var price in catalog[i].VirtualCurrencyPrices)
            {
                priceString += $"{price.Value} {price.Key} "; 
            }

            _items[i].ShowItemSlot(catalog[i].ItemId, $"{priceString}", catalog[i]);
        }
    }

    private void OnGetCatalogError(PlayFabError error)
    {
        var errorMesssage = error.GenerateErrorReport();
        Debug.LogError($"{errorMesssage}");
    }
}
