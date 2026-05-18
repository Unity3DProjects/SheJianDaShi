using AD.BlackBag;
using AD.Tools;

namespace AD.Instance
{
    internal class AddDeskWindow : GroupManagerBase<AddDeskWindow, AddDeskPanel>
    {
        private bool haveShortcut;

        public void ShowAddDeskPanel()
        {
            if (haveShortcut) return;

            if (!BlackBagManager.Instance.CanShowBlackAD) return;

            ShortcutHepler.HasShortcutInstalled(installed =>
            {
                haveShortcut = installed;
                var msg = haveShortcut ? "桌面已存在图标" : "桌面不存在图标，尝试添加";
                msg.Log();
                if (!haveShortcut)
                {
                    GroupInstance.Show();
                    ADManager.Instance.Show(AdSourceType.Black);
                }
            });
        }
    }
}