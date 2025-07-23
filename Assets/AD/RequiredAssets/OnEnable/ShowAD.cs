using UnityEngine;

namespace AD.Runtime.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowAD : MonoBehaviour
    {
        private void OnEnable()
        {
            ADManager.Instance.ShowWhiteAd();
        }
    }
}
