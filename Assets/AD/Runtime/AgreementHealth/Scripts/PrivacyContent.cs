using UnityEngine;

namespace AgreementHealth.Scripts
{
    public class PrivacyContent : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<UnityEngine.UI.Text>().text = AgreementHealth.Instance.AgreementDataSo.PrivacyText;
        }
    }
}