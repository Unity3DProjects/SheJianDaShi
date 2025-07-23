using System;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private const string GUID = "65a9c4f596f028f439eef4a3dfd73ac2";

    private static Action currentAction;

    public static void ShowRewardAD(Action action)
    {
        if (!GameObject.Find(GUID))
        {
            var go = new GameObject(GUID);
            go.AddComponent<RewardManager>();
        }

        currentAction = action;

#if UNITY_WEBGL
        ADManager.Instance.ShowRewardAD(GUID, nameof(GetReward));
#elif UNITY_ANDROID
        try
        {
            var rewardManager = GameObject.Find(GUID).GetComponent<RewardManager>();
            rewardManager.InvokVideoEvent(rewardManager.gameObject, (s) => { GetRewardGlobal(); });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
#endif
    }

    private void GetReward()
    {
        currentAction?.Invoke();
        currentAction = null;
    }

    private static void GetRewardGlobal()
    {
        currentAction?.Invoke();
        currentAction = null;
    }
}