using System;
using AD.Runtime.AdAdapter.Interface;

namespace AD.Runtime.AdAdapter
{
    public class HuaWeiAdAdapter : IAdAdapter
    {
        private static ADHuaWeiParam _huaWeiADParam;
        public static ADHuaWeiParam HuaWeiADParam => _huaWeiADParam;

        public void Init(IADParam normalParam)
        {
            if (normalParam is ADHuaWeiParam adHuaWeiParam)
            {
                _huaWeiADParam = adHuaWeiParam;
            }
            else
            {
                throw new Exception("AD参数初始化错误");
            }
        }

        public void ShowWhiteAD()
        {
            HWAdAdapter.Instance.ShowNativeAd();
        }

        public void ShowBlackAD()
        {
            HWAdAdapter.Instance.ShowNativeAd();
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param = null)
        {
            HWAdAdapter.ShowRewardAd(callBackObjectName, callBackMethodName, param);
        }

        public void AddToDesktop()
        {
        }

        public void Quit()
        {
            HWAdAdapter.Instance.ExitApp();
        }
    }
}