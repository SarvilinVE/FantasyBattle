using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyBattle.UI
{
    public class UnitInfoUI : MonoBehaviour
    {

        #region Fields

        [SerializeField] TMP_Text _nameUnit;
        [SerializeField] TMP_Text _hpBarUnit;
        [SerializeField] Slider _hpBar;

        #endregion


        #region Properties

        public bool IsActive { get; set; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            //IsActive = false;
            //gameObject.SetActive(IsActive);
        }

        #endregion


        #region Methods

        public void SetData(string nameUnit, int currentHp, int maxHp, bool isActive)
        {
            if (isActive)
            {
                _nameUnit.text = nameUnit;
                _hpBarUnit.text = $"{currentHp} / {maxHp}";
                _hpBar.value = currentHp;
                _hpBar.maxValue = maxHp;

                gameObject.SetActive(isActive);
            }
        }

        #endregion

    }
}
