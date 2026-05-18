using UnityEngine;

namespace AD.Instance
{
    public class PrivacyContent : MonoBehaviour
    {
        private static AgreementDataSO _agreementDataSo;


        private static AgreementDataSO AgreementDataSo
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

        private void Start()
        {
            GetComponent<UnityEngine.UI.Text>().text = AgreementDataSo.PrivacyText;
        }
    }
}