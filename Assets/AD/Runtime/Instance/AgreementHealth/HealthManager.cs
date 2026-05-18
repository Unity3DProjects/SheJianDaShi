using System;
using System.Collections;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace AD.Instance
{
    internal class HealthManager : AHGroupBase<HealthManager>, IADInit
    {
        public event Action OnHealthStart;
        public event Action OnHealthEnd;
        private Text content;
        private Image contentImage;
        private Image ageImage;
        private Text company, code;

        protected override void InitComponent()
        {
            var healthRoot = transform;
            content = healthRoot.Find("Content").GetComponent<Text>();
            ageImage = healthRoot.Find("Age").GetComponent<Image>();
            contentImage = healthRoot.Find("ContentImage").GetComponent<Image>();
            var bottom = healthRoot.Find("Bottom");
            company = bottom.Find("Company").GetComponent<Text>();
            code = bottom.Find("Code").GetComponent<Text>();
            switch (AgreementDataSo.copyrightType)
            {
                case ECopyrightType.Text:
                    contentImage.gameObject.SetActive(false);
                    company.text = $"著作权人：{AgreementDataSo.companyName}";
                    code.text = $"登记号：{AgreementDataSo.registrationNum}";
                    break;
                case ECopyrightType.Picture:
                    contentImage.gameObject.SetActive(true);
                    contentImage.sprite = AgreementDataSo.companySprite;
                    break;
            }

            switch (AgreementDataSo.ageType)
            {
                case EAgeType.Sixteen:
                    var s1 = Resources.Load<Sprite>("AD_Age/16");
                    ageImage.sprite = s1 != null ? s1 : ageImage.sprite;
                    break;
                case EAgeType.Twelve:
                    var s2 = Resources.Load<Sprite>("AD_Age/12");
                    ageImage.sprite = s2 != null ? s2 : ageImage.sprite;
                    break;
                default:
                    break;
            }
        }

        public void Init()
        {
        }

        public void ShowHealthPanel()
        {
            Show();
            if (BlackBagManager.Instance.NeedHealth)
                StartCoroutine(ShowHealthUI());
            else
            {
                Call(ref OnHealthStart);
                Call(ref OnHealthEnd);
            }
        }

        private IEnumerator ShowHealthUI()
        {
            yield return new WaitForEndOfFrame();
            Call(ref OnHealthStart);
            yield return new WaitForSeconds(3);
            Call(ref OnHealthEnd);
            Hide();
        }

        private static void Call(ref Action action)
        {
            action?.Invoke();
            action = null;
        }
    }
}