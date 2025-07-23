using UnityEngine;
using UnityEngine.UI;

namespace AD.Runtime.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnBlackAD : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(() => { ADManager.Instance.ShowBlackAd(); });
        }

        private void OnDestroy()
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
        }

        public void OnClick()
        {
            ADManager.Instance.ShowBlackAd();
        }
    }
}