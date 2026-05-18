using UnityEngine;
using UnityEngine.UI;

namespace AD.Instance
{
    
    internal class EyeCarePanel : GroupBase<EyeCarePanel>
    {
        protected override void InitComponent()
        {
            var root = transform;
            root.Find("Button (Legacy)").GetComponent<Button>().InitButton(Hide);
        }
    }
}