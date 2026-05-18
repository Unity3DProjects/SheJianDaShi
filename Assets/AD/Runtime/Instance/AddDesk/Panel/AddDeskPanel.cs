using System;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace AD.Instance
{
    
    internal class AddDeskPanel : GroupBase<AddDeskPanel>
    {
        protected override void InitComponent()
        {
            var root = transform;
            root.Find("Cancel").GetComponent<Button>().InitButton(Hide);
            root.Find("Confirm").GetComponent<Button>().InitButton(Hide, ADManager.Instance.CreateShortcutButton);
        }
    }
}