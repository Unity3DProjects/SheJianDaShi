using UnityEngine;
using UnityEngine.UI;

public class BtnLogout : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(HWAdAdapter.Instance.OpenLogoutPanel);
    }

}
