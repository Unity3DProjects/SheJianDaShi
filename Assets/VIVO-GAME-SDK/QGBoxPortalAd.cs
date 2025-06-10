using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGBoxPortalAd : QGBaseAd
    {

        public Action onShowAction;

        public QGBoxPortalAd(string adId) : base(adId)
        {

        }

        public void OnShow(Action onShow)
        {
            onShowAction += onShow;
        }


        public void OffShow(Action offShow)
        {
            onShowAction -= offShow;
        }
    }
}

