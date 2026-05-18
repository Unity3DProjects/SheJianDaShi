using System;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private const string GUID = "65a9c4f596f028f439eef4a3dfd73ac2";

    private static Action currentAction;

    public static void ShowRewardAD(Action action, Action failure = null)
    {
        if (!GameObject.Find(GUID))
        {
            var go = new GameObject(GUID);
            go.AddComponent<RewardManager>();
            DontDestroyOnLoad(go);
        }

        currentAction = action;

        ADManager.Instance.ShowRewardAD(GUID, nameof(GetReward), null, failure);
    }

    private void GetReward()
    {
        var action = currentAction;
        currentAction = null;
        action?.Invoke();
    }

    private static void GetRewardGlobal()
    {
        var action = currentAction;
        currentAction = null;
        action?.Invoke();
    }
}