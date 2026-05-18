using System;
using System.Collections.Generic;
using System.Linq;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace AD.Instance
{
    public class PrivacyManager : AHGroupBase<PrivacyManager>, IADInit
    {
        private const string KeyAgree = "1001";
        public event Action OnAgree;
        public event Action OnDecline;
        public event Action OnConfirm;
        public event Action OnHttp;

        private GameObject[] agreeTypeGO;
        private GameObject[] agreeBtn, declineBtn, confirmBtn, httpBtn;


        private int PrivacyType => (int) AgreementDataSo.privacyType;

        protected override void InitComponent()
        {
            var agreementRoot = transform;
            agreeTypeGO = new GameObject[]
            {
                agreementRoot.Find("Http").gameObject,
                agreementRoot.Find("Content").gameObject,
                agreementRoot.Find("Content").gameObject,
            };
            agreeBtn = new GameObject[]
            {
                agreementRoot.Find("Http/Btns/Agree").gameObject,
                agreementRoot.Find("Content/Btns/btn_agree").gameObject,
                agreementRoot.Find("Content/Btns/btn_agree").gameObject,
            };
            declineBtn = new GameObject[]
            {
                agreementRoot.Find("Http/Btns/Exit").gameObject,
                agreementRoot.Find("Content/Btns/btn_cancel").gameObject,
                agreementRoot.Find("Content/Btns/btn_cancel").gameObject,
            };
            confirmBtn = new GameObject[]
            {
                agreementRoot.Find("Http/Btns/Confirm").gameObject,
                agreementRoot.Find("Content/Btns/btn_confirm").gameObject,
                agreementRoot.Find("Content/Btns/btn_confirm").gameObject,
            };
            httpBtn = new GameObject[]
            {
                agreementRoot.Find("Http/Start/Http").gameObject,
            };
            foreach (var button in agreeBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(Onclick_Agree);
            }

            foreach (var button in declineBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(Onclick_Decline);
            }

            foreach (var button in confirmBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(Onclick_Confirm);
            }

            foreach (var button in httpBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(OnClick_Http);
            }

            SetAll(agreeTypeGO, false);
            agreeTypeGO[PrivacyType].SetActive(true);
            SetBtn(PrivacyButtonType.Agree | PrivacyButtonType.Decline);
        }

        public void Init()
        {
            bool isAgree = PlayerPrefs.GetInt(KeyAgree) == 1;


            if (BlackBagManager.Instance.NeedPrivacy && !isAgree)
            {
                Show();
            }
            else
            {
                Onclick_Agree();
            }
        }


        private void Onclick_Agree()
        {
            PlayerPrefs.SetInt(KeyAgree, 1);
            Hide();
            OnAgree?.Invoke();
        }

        private void Onclick_Decline()
        {
            ADManager.Instance.ExitApplication();
            OnDecline?.Invoke();
        }

        private void Onclick_Confirm()
        {
            Hide();
            OnConfirm?.Invoke();
        }

        private void OnClick_Http()
        {
            HWSDK.OpenUrl(AgreementDataSo.privacyHttp);
            OnHttp?.Invoke();
        }

        private void SetBtn(PrivacyButtonType flag)
        {
            SetAll(agreeBtn, false);
            SetAll(declineBtn, false);
            SetAll(confirmBtn, false);
            agreeBtn[PrivacyType].SetActive((flag & PrivacyButtonType.Agree) != 0);
            declineBtn[PrivacyType].SetActive((flag & PrivacyButtonType.Decline) != 0);
            confirmBtn[PrivacyType].SetActive((flag & PrivacyButtonType.Confirm) != 0);
        }

        private static void SetAll(IEnumerable<GameObject> go, bool state)
        {
            foreach (var o in go) o.SetActive(state);
        }

        public void ShowAgreementPanel()
        {
            if (AgreementDataSo.privacyType == EPrivacyType.Http)
            {
                OnClick_Http();
            }
            else
            {
                SetBtn(PrivacyButtonType.Confirm);
                Show();
            }
        }
    }
}