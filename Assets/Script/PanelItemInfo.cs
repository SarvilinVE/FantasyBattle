using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PanelItemInfo : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _info;

    [SerializeField]
    private TMP_Text _parametr;

    [SerializeField]
    private TMP_Text _value;

    [SerializeField]
    private Button _background;

    public void ShowItemInfo(CatalogItem catalogItem)
    {
        _info.text = catalogItem.Description;

        string stringInfo = "";
        var customData = catalogItem.CustomData.Split(new char[] { '"', ':', ' ', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        for (var i=0; i < customData.Length; i++)
        {
            if(i != 2)
            {
                stringInfo += $"{customData[i]}";
            }
            else
            {
                _parametr.text = stringInfo;
                _value.text = customData[i];
            }
        }
    }
    private void Start()
    {
        _background.onClick.AddListener(Destroy);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
