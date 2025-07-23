using AD.Runtime.AdAdapter.Interface;
using AD.Runtime.Tools.Tools;
using UnityEngine;

namespace AD.Runtime.AdAdapter
{
    public class TestAdAdapter : IAdAdapter
    {
        public void Init(IADParam normalParam)
        {
        }

        public void ShowWhiteAD()
        {
            this.Log("尝试调用白包");
        }

        public void ShowBlackAD()
        {
            this.Log("尝试调用黑包");
        }

        public void ShowRewardAD(string callBackObjectName, string callBackMethodName, string param)
        {
            this.Log("激励");
            GameObject.Find(callBackObjectName).SendMessage(callBackMethodName, param);
        }

        public void AddToDesktop()
        {
            this.Log("添加桌面");
        }

        public void Quit()
        {
            this.Log("退出");
        }
    }
}