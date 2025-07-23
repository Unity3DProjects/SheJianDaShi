using System;
using System.Collections;
using AD.Runtime.BlackBag;
using AD.Runtime.Tools.Tools;
using UnityEngine;

namespace AD.Runtime.TreasureBox.Scripts
{
    /// <summary>
    /// 负责处理定时的附加选项
    /// </summary>
    public class TimingManager : MonoBehaviour
    {
        public static TimingManager Instance;

        [Header("是否开启定时广告")] [SerializeField] private bool isRepeatAD = true;
        public int firstShowADTime = 45;
        public int showADIntervalTime = 45;
        [Header("是否开启定时宝箱")] [SerializeField] private bool isRepeatBox = false;
        public int firstShowBoxTime = 180;
        public int showBoxIntervalTime = 120;

        [Header("是否开启定时添加桌面")] [SerializeField]
        private bool isRepeatAddDesk;

        [SerializeField] private int addDeskTime = 30;

        [Header("是否开启定时添加桌面弹窗")] [SerializeField]
        private bool isRepeatAddDeskPanel;

        [SerializeField] private int addDeskPanelTime = 35;

        [Header("组件")] public GameObject TreasureBoxPanel;
        public GameObject EyeCarePanel;
        public GameObject AddDeskPanel;

        public OperationType OperationType => BlackBagManager.Instance.OperationType;

        public EPlatform CurPlatform => ADManager.Instance.Platform;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            TreasureBoxPanel.SetActive(false);
            EyeCarePanel.SetActive(false);

            if (isRepeatAD)
                InvokeRepeating(nameof(ShowAdWithLimite), firstShowADTime, showADIntervalTime); //定时广告
            if (isRepeatBox)
                StartCoroutine(CoShowBox()); //定时宝箱
            if (OperationType == OperationType.ChengZhiQing && CurPlatform == EPlatform.VIVO)
                InvokeRepeating(nameof(ShowEyeCare), firstShowBoxTime, showBoxIntervalTime); //定时护眼
            if (OperationType == OperationType.YeZi || isRepeatAddDesk)
                StartCoroutine(AddToDeskUntilFinish()); //定时添加桌面
            if (OperationType == OperationType.WanBan || OperationType == OperationType.WanBianTi ||
                isRepeatAddDeskPanel)
                StartCoroutine(ShowAddDeskPanelUntilFinish()); //定时添加桌面弹窗

            if (CurPlatform == EPlatform.XiaoMi)
            {
                isRepeatAD = false;
            }
        }

        private IEnumerator CoShowBox()
        {
            yield return new WaitForSecondsRealtime(firstShowBoxTime);

            while (true)
            {
                ShowTreasureBox();
                yield return new WaitForSecondsRealtime(showBoxIntervalTime);
            }
        }


        /// <summary>
        /// 显示宝箱
        /// </summary>
        /// <param name="OnFinish"></param>
        public void ShowTreasureBox(Action OnFinish = null)
        {
#if UNITY_EDITOR
            this.Log($"尝试显示宝箱:{BlackBagManager.Instance.CanShowBox}", ELogColor.Green);
#endif
            if (BlackBagManager.Instance.CanShowBox)
            {
                Time.timeScale = 0;
                OnFinish += () => Time.timeScale = 1;

                TreasureBoxPanel.SetActive(BlackBagManager.Instance.CanShowBox); // 正常宝箱
                TreasureBoxPanel.GetComponent<TreasureBoxPanel>()?.AddListnner(OnFinish);
            }
        }

        /// <summary>
        /// 护眼提示
        /// </summary>
        private void ShowEyeCare()
        {
            EyeCarePanel.SetActive(true); // 正常宝箱
            ShowAdWithLimite();
        }


        private void ShowAdWithLimite()
        {
            ADManager.Instance.ShowBlackAd();
        }


        private bool haveShortcut;


        /// <summary>
        /// 间隔30秒弹出添加到桌面，直到添加成功
        /// </summary>
        private IEnumerator AddToDeskUntilFinish()
        {
            WaitForSecondsRealtime addTime = new WaitForSecondsRealtime(addDeskTime);
            yield return addTime;

            if (!BlackBagManager.Instance.CanShowAD)
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

        /// <summary>
        /// 间隔30秒弹出添加到桌面窗口，直到添加成功
        /// </summary>
        private IEnumerator ShowAddDeskPanelUntilFinish()
        {
            WaitForSecondsRealtime addTime = new WaitForSecondsRealtime(addDeskPanelTime);
            yield return addTime;

            if (!BlackBagManager.Instance.CanShowAD)
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
                        AddDeskPanel.SetActive(true);
                    }
                });
                yield return addTime;
            }
        }
    }
}