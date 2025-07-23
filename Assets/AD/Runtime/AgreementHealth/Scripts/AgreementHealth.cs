using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.Runtime.BlackBag;
using AD.Runtime.Tools.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AgreementHealth.Scripts
{
    public class AgreementHealth : SingletonMonoBase<AgreementHealth>
    {
        [Header("是否跳转下一场景")] [SerializeField] private bool isAutoLoadNextScene = true;

        [Header("是否关闭隐私政策")] [SerializeField] private bool isClosePrivacy = false;


        [Header("组件")] [SerializeField] private GameObject m_agreeGo;
        [SerializeField] private GameObject m_healthGo;

        [SerializeField] private Text c_companyText;
        [SerializeField] private Text c_codeText;
        [SerializeField] private Image companyImage;
        [SerializeField] private Image ageTip;

        [SerializeField] private GameObject[] agreeTypeGO;
        [SerializeField] private GameObject[] agreeBtn, declineBtn, confirmBtn;


        private const string key_agre = "1001";


        private AgreementDataSO _agreementDataSo;


        public AgreementDataSO AgreementDataSo
        {
            get
            {
                if (_agreementDataSo == null)
                {
                    _agreementDataSo = Resources.Load<AgreementDataSO>("AgreementDataSO");
                }

                return _agreementDataSo;
            }
        }

        private int PrivacyType => (int)AgreementDataSo.privacyType;

        public bool IsClosePrivacy => isClosePrivacy;

        public override void Awake()
        {
            base.Awake();
            foreach (var button in agreeBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Onclick_Agree);
            }

            foreach (var button in declineBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Onclick_Decline);
            }

            foreach (var button in confirmBtn.Select(e => e.GetComponent<UnityEngine.UI.Button>()))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Onclick_Confirm);
            }
        }

        private void Start()
        {
            switch (AgreementDataSo.copyrightType)
            {
                case ECopyrightType.text:
                    companyImage.gameObject.SetActive(false);
                    c_companyText.text = $"著作权人：{AgreementDataSo.companyName}";
                    c_codeText.text = $"登记号：{AgreementDataSo.registrationNum}";
                    break;
                case ECopyrightType.picture:
                    companyImage.gameObject.SetActive(true);
                    companyImage.sprite = AgreementDataSo.companySprite;
                    break;
            }

            switch (AgreementDataSo.ageType)
            {
                case EAgeType.Sixteen:
                    var s1 = Resources.Load<Sprite>("AD_Age/16");
                    ageTip.sprite = s1 != null ? s1 : ageTip.sprite;
                    break;
                case EAgeType.Twelve:
                    var s2 = Resources.Load<Sprite>("AD_Age/12");
                    ageTip.sprite = s2 != null ? s2 : ageTip.sprite;
                    break;
            }

            m_agreeGo.SetActive(false);
            m_healthGo.SetActive(false);
            SetAll(agreeTypeGO, false);
            agreeTypeGO[PrivacyType].SetActive(true);
            SetBtn(0b110);

            bool isAgree = PlayerPrefs.GetInt(key_agre) == 1;


            if (!BlackBagManager.Instance.NeedPrivacy || IsClosePrivacy)
            {
                StartCoroutine(ShowHealthUI());
            }
            else
            {
                if (isAgree)
                {
                    StartCoroutine(ShowHealthUI());
                }
                else
                {
                    m_agreeGo.SetActive(true);
                }
            }
        }


        public void Onclick_Agree()
        {
            PlayerPrefs.SetInt(key_agre, 1);
            m_agreeGo.SetActive(false);
            StartCoroutine(ShowHealthUI());
        }

        public void Onclick_Decline()
        {
            ADManager.Instance.ExitApplication();
        }

        public void Onclick_Confirm()
        {
            m_agreeGo.SetActive(false);
        }

        public void OnClick_Http()
        {
            HWAdAdapter.OpenUrl(AgreementDataSo.privacyHttp);
        }

        private IEnumerator ShowHealthUI()
        {
            if (ADManager.Instance.Platform == EPlatform.HuaWei)
            {
                m_healthGo.SetActive(true);
                yield return new WaitForSeconds(3);
                m_healthGo.SetActive(false);
                ADManager.Instance.Login(
                    success: () => { HWAdAdapter.Instance.OnLoginSuccess(isAutoLoadNextScene); },
                    faild: () => { HWAdAdapter.Instance.OnLoginFail(); });

#if UNITY_EDITOR
                if (isAutoLoadNextScene)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
#endif
                yield break;
            }

            if (isAutoLoadNextScene)
            {
                m_healthGo.SetActive(true);
                AsyncOperation ao = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                ao.allowSceneActivation = false;
                yield return new WaitForSeconds(3);
                m_healthGo.SetActive(false);
                ao.allowSceneActivation = true;
                if (BlackBagManager.Instance.CanShowAD)
                {
                    ADManager.Instance.CreateShortcutButton();
                }
            }
            else
            {
                m_healthGo.SetActive(true);
                yield return new WaitForSeconds(3);
                m_healthGo.SetActive(false);
                if (BlackBagManager.Instance.CanShowAD)
                {
                    ADManager.Instance.CreateShortcutButton();
                }
            }
        }

        public void ShowAgreementPanel()
        {
            if (AgreementDataSo.privacyType == EPrivacyType.Http)
            {
                OnClick_Http();
            }
            else
            {
                SetBtn(0b001);
                m_agreeGo.SetActive(true);
            }
        }

        private void SetBtn(int flag)
        {
            SetAll(agreeBtn, false);
            SetAll(declineBtn, false);
            SetAll(confirmBtn, false);
            agreeBtn[PrivacyType].SetActive((flag & 0b100) != 0);
            declineBtn[PrivacyType].SetActive((flag & 0b10) != 0);
            confirmBtn[PrivacyType].SetActive((flag & 0b1) != 0);
        }

        private static void SetAll(IEnumerable<GameObject> go, bool state)
        {
            foreach (var o in go) o.SetActive(state);
        }
    }
}