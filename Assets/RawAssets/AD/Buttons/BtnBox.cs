using UnityEngine;

namespace AD.Tools.Buttons
{
    [DisallowMultipleComponent]
    public class BtnBox : BtnAdBase
    {
        protected override void OnClick()
        {
            ADManager.Instance.ShowBox();
        }
    }
}