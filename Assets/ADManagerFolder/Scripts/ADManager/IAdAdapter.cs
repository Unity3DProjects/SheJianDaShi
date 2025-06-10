using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 广告接口，目前有 Vivo、Oppo
/// </summary>
public interface IAdAdapter
{
     void Init();
     void ShowBannerAd();
     void ShowCustomAd(CustomType_E customType=CustomType_E.其他模板);
     void ShowRewardAd(string gameObjectName,string methodName);
     void CreateShortcutButton();
     void ExitApplication();
}