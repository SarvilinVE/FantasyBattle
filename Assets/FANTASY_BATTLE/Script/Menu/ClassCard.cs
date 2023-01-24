using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.Menu
{
    public class ClassCard : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _nameClass;

        [SerializeField]
        private TMP_Text _classHP;

        [SerializeField]
        private TMP_Text _classMP;

        [SerializeField]
        private TMP_Text _classDamage;

        [SerializeField]
        private GameObject _lockedPanel;

        [SerializeField]
        private Button _selectClass;


        public TMP_Text NameClass => _nameClass;
        public TMP_Text ClassHP => _classHP;
        public TMP_Text ClassMP => _classMP;
        public TMP_Text ClassDamage => _classDamage;
        public Button SelectClass { get => _selectClass; set => _selectClass = value; }

        public void ShowCard(bool isLocked, Sprite icon, string name, float hp, float mp, float damage)
        {
            _icon.sprite = icon;
            _nameClass.text = name;
            _classHP.text = hp.ToString();
            _classMP.text = mp.ToString();
            _classDamage.text = damage.ToString();

            _lockedPanel.SetActive(isLocked);
        }
    }
}
