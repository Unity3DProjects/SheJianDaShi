using System;
using AD.Runtime.Tools.Tools;
using HWWASM;
using UnityEngine;

public partial class ADManager
{
    /// <summary>
    /// 登录
    /// </summary>
    public void Login(Action success, Action faild)
    {
        if (ADParam is ADHuaWeiParam adHuaWeiParam)
        {
            this.Log("华为账号登录");
            GameLoginOption option = new GameLoginOption();
            option.appid = adHuaWeiParam.appid;
            this.Log($"华为id:{adHuaWeiParam.appid}");
            option.forceLogin = 1;
            option.success += s => { success?.Invoke(); };
            option.fail += s => { faild?.Invoke(); };
            QG.GameLogin(option);
        }
    }
}