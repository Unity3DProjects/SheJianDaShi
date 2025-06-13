using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGVideoPlayer : QGBaseAd
    {
        public QGVideoPlayer(string adId) : base(adId)
        {

        }

        public void Destroy()
        {
            Debug.Log("adId233: " + adId);
            QGMiniGameManager.Instance.DestroyAd(adId);
            QGAds.Remove(adId);
        }
    }
}
