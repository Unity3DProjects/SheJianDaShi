using UnityEngine;
using UnityEngine.UI;

namespace AD.Tools
{
    public abstract class BtnAdBase : MonoBehaviour
    {
        private Button btn;

        protected void Start()
        {
            btn = GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(OnClick);
        }

        protected void OnDestroy()
        {
            if (btn != null)
                btn.onClick.RemoveListener(OnClick);
        }

        protected abstract void OnClick();
    }
}