using UnityEngine;

public class AdComponentManager : MonoBehaviour
{
    public GameObject privacyBtnGO, addBtnGO, logoutBtnGO;

    private void Awake()
    {
        AddBtn<BtnAgreement>(privacyBtnGO);
        AddBtn<BtnAddToDesk>(addBtnGO);
        AddBtn<BtnLogout>(logoutBtnGO);
    }

    private void Start()
    {
        var type = ADManager.Instance.AllowedButtonType();
        SetState(privacyBtnGO, type.HasFlag(AdButtonType.隐私政策));
        SetState(addBtnGO, type.HasFlag(AdButtonType.添加桌面));
        SetState(logoutBtnGO, type.HasFlag(AdButtonType.注销账号));
    }

    private static void AddBtn<T>(GameObject go) where T : Component
    {
        if (go != null && !go.TryGetComponent<T>(out _))
            go.AddComponent<T>();
    }

    private static void SetState(GameObject go, bool state)
    {
        if (go != null) go.SetActive(state);
    }
}