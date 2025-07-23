using UnityEngine;
using UnityEngine.UI;

namespace AD.Runtime.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnBox : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>()?.onClick.AddListener(() => { ADManager.Instance.ShowBox(); });
        }

        private void OnDestroy()
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
        }
    
        public void OnClick()
        {
            ADManager.Instance.ShowBox();
        }
    }
}