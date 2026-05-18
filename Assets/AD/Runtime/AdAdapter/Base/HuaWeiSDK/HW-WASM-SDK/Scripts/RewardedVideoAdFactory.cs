using System.Collections.Generic;

namespace HWWASM
{
    public class RewardedVideoAdFactory

    {
        private readonly Dictionary<string, RewardedVideoAd> _dictionary = new Dictionary<string, RewardedVideoAd>();

        public static RewardedVideoAdFactory Instance { get; } = new RewardedVideoAdFactory();

        public RewardedVideoAd CreateRewardedVideoAd(CreateRewardedVideoAdOption option)
        {
            string callbackId =
                RTCallback<CreateRewardedVideoAdSuccessResult, CreateRewardedVideoAdFailResult>.AddCallback(
                    option.success, option.fail, option.complete);
            RewardedVideoAd rewardedVideoAd = new RewardedVideoAd(option, callbackId);
            _dictionary.Add(callbackId, rewardedVideoAd);
            return rewardedVideoAd;
        }

        public RewardedVideoAd _GetRewardedVideoAd(string callbackId)
        {
            return _dictionary.ContainsKey(callbackId) ? _dictionary[callbackId] : null;
        }

        public void _RemoveRewardedVideoAd(string callbackId)
        {
            if (_dictionary.ContainsKey(callbackId))
            {
                _dictionary.Remove(callbackId);
            }
        }
    }
}