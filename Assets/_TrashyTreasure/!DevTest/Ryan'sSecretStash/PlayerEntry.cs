using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TrashyTreasure
{
    public class PlayerEntry : MonoBehaviour
    {
        [SerializeField]
        private Toggle readyToggle;
        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [HideInInspector]
        public bool isReady;
        [HideInInspector]
        public string playerNameValue;

        public void updateDisplay()
        {
            readyToggle.isOn = isReady;
            playerNameText.text = playerNameValue;
        }
    }
}
