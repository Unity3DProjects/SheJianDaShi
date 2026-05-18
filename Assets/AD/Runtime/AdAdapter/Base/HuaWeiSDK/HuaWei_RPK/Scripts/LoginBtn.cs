using UnityEngine;
using UnityEngine.UI;

public class LoginBtn : MonoBehaviour
{
    private void Start()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        ADManager.Instance.Login();
    }
}