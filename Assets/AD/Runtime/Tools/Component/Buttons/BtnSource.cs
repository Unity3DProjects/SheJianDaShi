using UnityEngine;
using UnityEngine.UI;

namespace AD.Tools.Buttons
{
    public class BtnSource : MonoBehaviour
    {
        public AdSourceType type = AdSourceType.GameEnd;

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

        protected void OnClick()
        {
            ADManager.Instance.Show(type);
        }
    }
}