using System;

namespace AD.AdAdapter
{
    public abstract class BaseAdAdapter<T>
        : IAdAdapter
        where T : IADParam
    {
        protected T Param { get; private set; }

        public void Init(IADParam param)
        {
            if (!(param is T t)) throw new Exception("非法参数类型");
            Param = t;
            OnInit(Param);
        }

        protected abstract void OnInit(T param);

        public virtual void Login(Action onSuccess, Action onFailure)
        {
            onSuccess?.Invoke();
            ADUtils.SendMsgToADManager(ADManagerConst.LoginKey, true.ToString());
        }

        public abstract void ShowWhiteAD();

        public abstract void ShowBlackAD();

        public abstract void ShowBannerAD(string id = null);
        public abstract void ShowCustomAD(string id = null);

        public abstract void ShowRewardAD(string receiverKey, string id = null);

        public abstract void AddToDesktop();

        public abstract void Quit();
        public virtual bool HasTitle => false;

        public static void SendReward(string receiverKey, bool success)
        {
            ADUtils.SendMsgToADManager(receiverKey, success ? "Success" : "Failure");
        }
    }
}