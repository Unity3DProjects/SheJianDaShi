using UnityEngine;

namespace AD.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowWhiteAD : ShowAdBase
    {
        protected override void CallAd()
        {
            ADManager.Instance.ShowWhiteAd();
        }
    }
}