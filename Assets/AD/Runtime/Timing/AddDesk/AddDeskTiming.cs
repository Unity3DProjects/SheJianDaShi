using System.Collections;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;

namespace AD
{
    public class AddDeskTiming : TimingComponent
    {
        private Coroutine crt;

        protected override void Enable()
        {
            crt = StartCoroutine(AddToDeskUntilFinish());
        }

        protected override void Disable()
        {
            if (crt != null) StopCoroutine(crt);
        }

        public override void Reset()
        {
            enabled = false;
            firstTime = repeatTime = 30;
        }

        private bool haveShortcut;

        /// <summary>
        /// 间隔30秒弹出添加到桌面，直到添加成功
        /// </summary>
        private IEnumerator AddToDeskUntilFinish()
        {
            WaitForSecondsRealtime addTime = new WaitForSecondsRealtime(firstTime);
            yield return addTime;
            addTime = new WaitForSecondsRealtime(repeatTime);

            if (!BlackBagManager.Instance.CanShowBlackAD)
                yield break;

            while (!haveShortcut)
            {
                ShortcutHepler.HasShortcutInstalled(installed =>
                {
                    haveShortcut = installed;
                    var msg = haveShortcut ? "桌面已存在图标" : "桌面不存在图标，尝试添加";
                    msg.Log();
                    if (!haveShortcut)
                    {
                        ADManager.Instance.CreateShortcutButton();
                    }
                });
                yield return addTime;
            }
        }
    }
}