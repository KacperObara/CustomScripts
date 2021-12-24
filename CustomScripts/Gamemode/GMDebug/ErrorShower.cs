using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts.Gamemode.GMDebug
{
    public class ErrorShower : MonoBehaviourSingleton<ErrorShower>
    {
        public Text ErrorText;

        public void Show()
        {
            ErrorText.gameObject.SetActive(true);
        }
    }
}