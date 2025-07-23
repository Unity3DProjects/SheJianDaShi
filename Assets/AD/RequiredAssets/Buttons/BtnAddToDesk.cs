using UnityEngine;
using UnityEngine.UI;

namespace AD.Runtime.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnAddToDesk : MonoBehaviour
    {
#if UNITY_WEBGL
        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(() => { ADManager.Instance.CreateShortcutButton(); });
        }

        private void OnDestroy()
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
        }


        public void OnClick()
        {
            ADManager.Instance.CreateShortcutButton();
        }
#endif
    }
}