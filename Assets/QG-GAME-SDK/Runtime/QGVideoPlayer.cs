using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGVideoPlayer : QGBasePlayer
    {
        public QGVideoPlayer(string adId) : base(adId)
        {

        }

        public virtual void RequestFullScreen(int direction)
        {
            QGMiniGameManager.Instance.VideoPlayerRequestFullScreen(playerId, direction);
        }

        public virtual void ExitFullScreen()
        {
            QGMiniGameManager.Instance.VideoPlayerExitFullScreen(playerId);
        }
    }
}
