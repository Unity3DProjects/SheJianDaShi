using UnityEngine;
using UnityEngine.UI;

namespace AD.Runtime.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnWhiteAD : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(() => { ADManager.Instance.ShowWhiteAd(); });
        }

        private void OnDestroy()
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
        }
    
        public void OnClick()
        {
            ADManager.Instance.ShowWhiteAd();
        }
    }
}