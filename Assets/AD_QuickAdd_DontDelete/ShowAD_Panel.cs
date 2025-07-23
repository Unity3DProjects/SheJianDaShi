using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CustomType_E
{
    正常模板,
    设置签到,
    结算模板,
    自弹模板,
    其他模板
}
public class ShowAD_Panel : MonoBehaviour
{
    [Header("默认激活页面时  调用黑包，勾选显示白包")]
    public bool ShowWhite;
    public bool ShowBox;
    [Header("烨子，路总需填写，所以在开发时选好，其他页面 默认 就行")]
    public CustomType_E customType = CustomType_E.其他模板;
    [Header("是否在页面关闭时  调用黑包")]
    public bool WhenPanelClose;
    private void OnEnable()
    {
#if UNITY_EDITOR
        Debug.Log("广告-------------- > " + gameObject.name);
#endif

        if (ShowWhite)
        {
            //如果原工程没有使用到CustomType_E，不传参数也可以
            ADManager.Instance.ShowWhiteAd();
        }
        else
        {
            ADManager.Instance.ShowBlackAd();
        }

        if (ShowBox)
        {
            ADManager.Instance.ShowBlackAd();
            ADManager.Instance.ShowBox();
        }
        // VivoAdManager.ShowWhiteAd();
        // TreasureBox.Instance.ShowTreasureBox();
    }

    private void OnDisable()
    {
        if (WhenPanelClose)
        {
            ADManager.Instance.ShowBlackAd();
        }
    }
}
