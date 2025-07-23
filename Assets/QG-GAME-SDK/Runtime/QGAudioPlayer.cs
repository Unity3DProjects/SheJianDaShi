using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGAudioPlayer : QGBasePlayer
    {
        public QGAudioPlayer(string playerId) : base(playerId)
        {

        }


        //volume 0~1
        public virtual void SetVolume(float volume)
        {
            QGMiniGameManager.Instance.AudioPlayerVolume(playerId, volume);
        }

        public virtual void SetLoop(bool bl)
        {
            QGMiniGameManager.Instance.AudioPlayerLoop(playerId, bl);
        }

        public virtual float GetDuration()
        {
            return QGMiniGameManager.Instance.AudioPlayerGetDuration(playerId);
        }

        public virtual float GetCurrentTime()
        {
            return QGMiniGameManager.Instance.AudioPlayerGetCurrentTime(playerId);
        }

        public virtual bool GetPaused()
        {
            return QGMiniGameManager.Instance.AudioPlayerGetPaused(playerId);
        }

        public virtual float GetBuffered()
        {
            return QGMiniGameManager.Instance.AudioPlayerGetBuffered(playerId);
        }
    }
}
