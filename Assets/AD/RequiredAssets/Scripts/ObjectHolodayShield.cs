using UnityEngine;

#if UNITY_WEBGL
using AD.Runtime.BlackBag.Shield;
#endif

public class ObjectHolodayShield : MonoBehaviour
{
#if UNITY_WEBGL
    private void Awake()
    {
        gameObject.SetActive(HolidayBlock.CheckCanShowAd());

#if UNITY_EDITOR
        gameObject.SetActive(true);
#endif
    }
#endif
}