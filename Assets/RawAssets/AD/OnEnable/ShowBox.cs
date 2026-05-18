using UnityEngine;

namespace AD.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowBox : ShowAdBase
    {
        protected override void CallAd()
        {
            ADManager.Instance.ShowBox();
        }
    }
}