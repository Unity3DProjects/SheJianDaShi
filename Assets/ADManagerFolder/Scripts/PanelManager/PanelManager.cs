using System;
using _ADD;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] TreasureBoxPanel;
    public GameObject[] PrivacyPanel;
    public GameObject[] AddDesktopIconPanel;
    public GameObject[] EyeCareReminderPanel;
    public Text[] TipText;

    
    [Header("结算宝箱；定时宝箱")] 
    [SerializeField]
    private bool bool_TimeBox_checkout;
    [Header("宝箱间隔时间")]
    public int FirstShowTime = 180;
    public int ThenShowTime = 120;


    private bool treasurerBoxCanShow = true;

    private float cantShowTime = 0;

    //用于判断是不是第一次开宝箱，路总vivo只显示一次宝箱
    private bool isLuZong_First=true;
    
    private int Opentation;
  

 

    private void Start()
    {
        Opentation = (int)(ADManager.Instance.orientation);
        for (int i = 0; i < 2; i++)
        {
            TipText[i].gameObject.SetActive(false);
            TreasureBoxPanel[i].SetActive(false);
            PrivacyPanel[i].SetActive(false);
            AddDesktopIconPanel[i].SetActive(false);
            EyeCareReminderPanel[i].SetActive(false);
        }

        
    }

    /// <summary>
    /// 初始化定时任务
    /// </summary>
    public void InitTimedMothed()
    {
        // todo:------------------------游戏没有明显结算，就使用定时宝箱-------------------------------------
        if (bool_TimeBox_checkout&&ADManager.Instance.总开关_宝箱)
        {
            InvokeRepeating(nameof(OpenBox_Timed), FirstShowTime, ThenShowTime); //定时宝箱
            Debug.Log("该游戏 结算 使用定时宝箱：已开启");
        }

        //todo:------------------------定时原生-------------------------------------
        if (ADManager.Instance.timedCustom_Black&&ADManager.Instance.总开关_定时_原生)
        {
            InvokeRepeating(nameof(ShowAd), 45, 45);
            // Debug.Log("定时原生:开");
        }

        //todo:------------------------陈志晴专属-------------------------------------
        if (ADManager.Instance.timedShowEye&&ADManager.Instance.总开关_定时_护眼提示)
        {
            InvokeRepeating(nameof(ShowEyeCarePanel), 45, 45);
            // Debug.Log("定时护眼弹窗（陈志晴）:开");
        }

        //todo:------------------------王勇专属，，如果添加桌面未添加成功，定时弹 添加图标Panel-------------------------------------
        if (ADManager.Instance.timedShowAddIcon&&ADManager.Instance.总开关_定时_添加桌面弹窗)
        {
            InvokeRepeating(nameof(ShowAddDesktopIconPanel), 45, 40);
            // Debug.Log("定时添加桌面图标弹窗:开");   
        }
    }

    private void Update()
    {
        //如果使用了结算间隔2分钟宝箱，并完成了第一次游戏结算，进入计时
        if (!treasurerBoxCanShow)
        {
            cantShowTime += Time.deltaTime;
            if (cantShowTime >= 120)
            {
                treasurerBoxCanShow = true;
            }
        }
    }

    #region 添加游戏图标到桌面的弹窗

    private bool SuccessAddIcon;

    /// <summary>
    ///显示 添加游戏图标到桌面的弹窗
    /// </summary>
    public void ShowAddDesktopIconPanel()
    {
        if (!SuccessAddIcon && ADManager.Instance.switchState.Black)
        {
            ADManager.Instance.ShowAD_Black();
            Debug.Log("桌面图标弹窗");
            AddDesktopIconPanel[Opentation].SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 弹窗的按钮注册事件
    /// </summary>
    /// <param name="isAdd">确定按钮，需添加桌面</param>
    public void CloseAddDesktopIconPanel(bool isAdd)
    {
        if (isAdd)
        {
            ADManager.Instance.AddDesktopIcon_Directly();
        }

        AddDesktopIconPanel[Opentation].SetActive(false);
        Time.timeScale = 1;
    }


    /// <summary>
    /// 接收  添加桌面图标成功的回调
    /// </summary>
    public void ChackDesktopIConIsSuccessfully()
    {
        //添加成功
        SuccessAddIcon = true;
    }

    #endregion

    #region 主页面显示隐私政策

    /// <summary>
    /// 主页面显示隐私政策
    /// </summary>
    public void ShowPrivacy()
    {
        // uiCamera.SetActive(true);
        PrivacyPanel[Opentation].SetActive(true);
    
    }

    #endregion

    #region 护眼提示

    public void ShowEyeCarePanel()
    {
        // uiCamera.SetActive(true);
        if (ADManager.Instance.switchState.Black)
        {
            ADManager.Instance.ShowAD_Black();
            EyeCareReminderPanel[Opentation].SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void CloseEyeCarePanel()
    {
        // uiCamera.SetActive(false);
        EyeCareReminderPanel[Opentation].SetActive(false);
        Time.timeScale = 1;
    }

    #endregion

    #region 提示文本

    /// <summary>
    /// 提示文本
    /// </summary>
    /// <param name="str">提示内容</param>
    public void ShowTipText(string str)
    {
        // uiCamera.SetActive(true);
        TipText[Opentation].gameObject.SetActive(true);
        TipText[Opentation].text = str;

        Invoke(nameof(HideTipText), 1.5f);
    }

    public void HideTipText()
    {
        // uiCamera.SetActive(false);
        TipText[Opentation].gameObject.SetActive(false);
        TipText[Opentation].text = "";
    }

    #endregion

    #region 打开宝箱

    /// <summary>
    /// 弹宝箱，开始宝箱不传参数
    /// </summary>
    /// <param name="isLoop">结算太频繁(转RPK时埋点用)，设置间隔两分钟的宝箱，默认false,不间隔</param>
    public void ShowTreasureBox(bool isLoop = false)
    {
        
        Debug.Log(ADManager.Instance.总开关_宝箱
            ? "-----打开宝箱激励-----".FH7_RoyalBlue()
            : "-----没有宝箱，确定吗(确定就无需理会)-----".FH3_Red());

        //宝箱总开关 默认true，开
        if (!ADManager.Instance.总开关_宝箱) return;

       
        TreasureBoxPanel[Opentation].SetActive(ADManager.Instance.switchState.Box && !isLoop && isLuZong_First);
        //结算弹宝箱后，间隔两分钟后的结算 才能再次弹宝箱
        if (treasurerBoxCanShow && isLoop&&isLuZong_First)
        {
            Time.timeScale = 0;
            TreasureBoxPanel[Opentation].SetActive(ADManager.Instance.switchState.Box);
            treasurerBoxCanShow = false;
        }

        //不是路总的就无需理会
        //用于路总的宝箱  只在开始显示一次，
        if (ADManager.Instance.blockTypeE == BlockType_E.深圳路总)
        {
            //显示过一次，就关掉
            isLuZong_First = false;
        }
    }

    /// <summary>
    /// 定时宝箱
    /// </summary>
    public void OpenBox_Timed()
    {
        ShowTreasureBox(false);
    }

    #endregion


    /// <summary>
    /// 定时原生所调用的方法
    /// </summary>
    public void ShowAd()
    {
        ADManager.Instance.ShowAD_Black(CustomType_E.自弹模板);
    }
}