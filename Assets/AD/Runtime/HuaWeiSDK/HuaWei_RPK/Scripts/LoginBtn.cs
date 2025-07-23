using UnityEngine;
using UnityEngine.UI;

public class LoginBtn : MonoBehaviour
{
    private void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                ADManager.Instance.Login(success: () => { HWAdAdapter.Instance.OnLoginSuccess(); },
                    faild: () => { HWAdAdapter.Instance.OnLoginFail(); });
            });
        }
    }
}