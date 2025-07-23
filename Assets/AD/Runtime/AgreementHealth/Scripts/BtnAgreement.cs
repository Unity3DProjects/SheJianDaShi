using AD.Runtime.BlackBag;
using UnityEngine;
using UnityEngine.UI;

namespace AgreementHealth.Scripts
{
    public class BtnAgreement : MonoBehaviour
    {
        private void Awake()
        {
            var btn = GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnClick);
            }
        }


        private void Start()
        {
            if (!BlackBagManager.Instance.NeedPrivacy || AgreementHealth.Instance.IsClosePrivacy)
            {
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            AgreementHealth.Instance.ShowAgreementPanel();
        }
    }
}