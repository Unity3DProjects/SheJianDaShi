using UnityEngine;
using UnityEngine.UI;

public class BtnAgreement : MonoBehaviour
{
    private void Start()
    {
        if (ADManager.Instance.AllowedButtonType().HasFlag(AdButtonType.隐私政策))
            GetComponent<Button>()?.onClick.AddListener(OnClick);
        else Destroy(gameObject);
    }

    public void OnClick()
    {
        AD.Instance.PrivacyManager.Instance.ShowAgreementPanel();
    }
}