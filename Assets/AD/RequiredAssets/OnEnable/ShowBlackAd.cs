using UnityEngine;

namespace AD.Runtime.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowBlackAd : MonoBehaviour
    {
        private void OnEnable()
        {
            ADManager.Instance.ShowBlackAd();
        }
    }
}
