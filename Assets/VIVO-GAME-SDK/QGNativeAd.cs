using UnityEngine;
using System;

namespace QGMiniGame
{
    public class QGNativeAd : QGBaseAd
    {
        public Action<QGNativeResponse> onLoadNativeAction;

        public QGNativeAd(string adId) : base(adId)
        {

        }

        public override void Show(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            Debug.LogWarning("QGNativeAd no Show Function");
        }

        public override void Hide(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            Debug.LogWarning("QGNativeAd no Hide Function");
        }

        public void OnLoad(Action<QGNativeResponse> onLoad)
        {
            onLoadNativeAction += onLoad;
        }


        public void OffLoad(Action<QGNativeResponse> offLoad)
        {
            onLoadNativeAction -= offLoad;
        }


        public void Load(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.LoadAd(adId, success, failed);
        }

        public void ReportAdShow(QGNativeReportParam param)
        {
            QGMiniGameManager.Instance.ReportAdShow(adId, param);
        }

        public void ReportAdClick(QGNativeReportParam param)
        {
            QGMiniGameManager.Instance.ReportAdClick(adId, param);
        }
    }
}
