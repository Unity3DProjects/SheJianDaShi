using UnityEngine;

namespace AD.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnWhiteAD : BtnAdBase
    {
        protected override void OnClick()
        {
            ADManager.Instance.ShowWhiteAd();
        }
    }
}