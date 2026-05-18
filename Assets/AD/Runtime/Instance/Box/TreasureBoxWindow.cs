using System;
using AD.BlackBag;
using AD.Tools;
using UnityEngine;

namespace AD.Instance
{
    internal class TreasureBoxWindow : GroupManagerBase<TreasureBoxWindow, TreasureBoxPanel>
    {
        /// <summary>
        /// 显示宝箱
        /// </summary>
        public void ShowTreasureBox()
        {
            var canShowBox = BlackBagManager.Instance.CanShowBox;
#if UNITY_EDITOR
            this.Log($"尝试显示宝箱:{canShowBox}", ELogColor.Green);
#endif
            if (canShowBox)
            {
                Action onFinish = () => ADUtils.SendMsgToADManager(ADManagerConst.ShowBoxCallback, "");
                Time.timeScale = 0;
                onFinish += () => Time.timeScale = 1;
                GroupInstance
                    .OnFinish(onFinish)
                    .SetCloseable(false)
                    .Done();
            }
        }

        /// <summary>
        /// 显示宝箱
        /// </summary>
        public void ShowTreasureBoxCloseable()
        {
            var canShowBox = BlackBagManager.Instance.CanShowBox;
#if UNITY_EDITOR
            this.Log($"尝试显示宝箱:{canShowBox}", ELogColor.Green);
#endif
            if (canShowBox)
            {
                Action onSuccess = () => ADUtils.SendMsgToADManager(ADManagerConst.ShowBoxCallback, "");
                Time.timeScale = 0;
                GroupInstance
                    .OnFinish(() => Time.timeScale = 1)
                    .OnSuccess(onSuccess)
                    .SetCloseable(true)
                    .Done();
            }
        }
    }
}