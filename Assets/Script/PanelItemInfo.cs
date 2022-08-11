using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private FromJson _fromJson;

    public void ShowItemInfo(CatalogItem catalogItem)
    {
        _info.text = catalogItem.Description;

        _fromJson = FromJson.LoadJsonString(catalogItem.CustomData);

        if (_fromJson.MAGIC_ATTACK != null) 
        {
            _parametr.text = $"Magic Attack";
            _value.text = $"+{_fromJson.MAGIC_ATTACK}"; 
        }

        if(_fromJson.MAGIC_DEFENSE != null)
        {
            _parametr.text = $"Magic Defense";
            _value.text = $"+{_fromJson.MAGIC_DEFENSE}";
        }

        //string stringInfo = "";
        //var customData = catalogItem.CustomData.Split(new char[] { '"', ':', ' ', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        //for (var i=0; i < customData.Length; i++)
        //{
        //    if(i != 2)
        //    {
        //        stringInfo += $"{customData[i]}";
        //    }
        //    else
        //    {
        //        _parametr.text = stringInfo;
        //        _value.text = customData[i];
        //    }
        //}
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
