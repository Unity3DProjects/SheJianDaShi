using AD.NodeTree;
using UnityEngine;

#if UNITY_WEBGL
using AD.BlackBag.Shield;
#endif

public class ObjectHolodayShield : MonoBehaviour
{
#if UNITY_WEBGL
    private void Awake()
    {
        gameObject.SetActive(!IsHolidayNode.IsHoliday());

#if UNITY_EDITOR
        gameObject.SetActive(true);
#endif
    }
#endif
}