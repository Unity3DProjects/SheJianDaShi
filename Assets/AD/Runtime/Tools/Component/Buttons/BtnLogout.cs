using AD.BlackBag;
using UnityEngine;
using UnityEngine.UI;

public class BtnLogout : MonoBehaviour
{
#if UNITY_WEBGL

    void Start()
    {
        if (ADManager.Instance.AllowedButtonType().HasFlag(AdButtonType.注销账号))
            GetComponent<Button>().onClick.AddListener(OnClick);
        else Destroy(gameObject);
    }

    public void OnClick()
    {
        HWSDK.Instance.OpenLogoutPanel();
    }
#endif
}