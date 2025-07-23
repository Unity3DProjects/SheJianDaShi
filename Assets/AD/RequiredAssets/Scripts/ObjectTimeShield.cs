using UnityEngine;

#if UNITY_WEBGL
using AD.Runtime.BlackBag.Shield;
#endif

public class ObjectTimeShield : MonoBehaviour
{
#if UNITY_WEBGL
    private void Awake()
    {
        gameObject.SetActive(TimeCheck.BlackTimeReach);

#if UNITY_EDITOR
        gameObject.SetActive(true);
#endif
    }
#endif
}