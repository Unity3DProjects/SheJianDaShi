using UnityEngine;

namespace AD.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnBlackAD : BtnAdBase
    {
        protected override void OnClick()
        {
            ADManager.Instance.ShowBlackAd();
        }
    }
}