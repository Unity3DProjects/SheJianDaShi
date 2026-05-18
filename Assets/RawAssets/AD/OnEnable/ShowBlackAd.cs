using UnityEngine;

namespace AD.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowBlackAd : ShowAdBase
    {
        protected override void CallAd()
        {
            ADManager.Instance.ShowBlackAd();
        }
    }
}
