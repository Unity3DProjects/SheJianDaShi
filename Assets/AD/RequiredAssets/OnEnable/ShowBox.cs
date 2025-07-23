using UnityEngine;

namespace AD.Runtime.Tools.OnEnable
{
    [DisallowMultipleComponent]
    public class ShowBox : MonoBehaviour
    {
        private void OnEnable()
        {
            ADManager.Instance.ShowBox();
        }
    }
}