using System;
using System.Collections.Generic;
using AD.AdAdapter;
using AD.Instance;
using UnityEngine;

public partial class ADManager
{
    private IDictionary<AdSourceType, Action> CallBridge { get; } = new Dictionary<AdSourceType, Action>();

    private void InitSourceBinder(IAdAdapter adapter)
    {
        CallBridge.Clear();
        //todo add other

        CallBridge.Add(AdSourceType.Pause, adapter.ShowWhiteAD);
        CallBridge.Add(AdSourceType.Setting, adapter.ShowWhiteAD);
        CallBridge.Add(AdSourceType.GameEnd, adapter.ShowWhiteAD);
        CallBridge.Add(AdSourceType.White, adapter.ShowWhiteAD);

        CallBridge.Add(AdSourceType.Black, adapter.ShowBlackAD);
        CallBridge.Add(AdSourceType.Callback, adapter.ShowBlackAD);
        CallBridge.Add(AdSourceType.TimingAd, adapter.ShowBlackAD);
        CallBridge.Add(AdSourceType.Other, adapter.ShowBlackAD);

        CallBridge.Add(AdSourceType.Box, TreasureBoxWindow.Instance.ShowTreasureBox);
        CallBridge.Add(AdSourceType.CloseableBox, TreasureBoxWindow.Instance.ShowTreasureBoxCloseable);
        CallBridge.Add(AdSourceType.TimingBox, TreasureBoxWindow.Instance.ShowTreasureBox);

        CallBridge.Add(AdSourceType.Banner, () => adapter.ShowBannerAD());
        CallBridge.Add(AdSourceType.Custom, () => adapter.ShowCustomAD());
    }

    internal void Call(AdSourceType type)
    {
        Debug.Log($"Current Receive {type}");
        if (CallBridge.TryGetValue(type, out var action))
            action();
    }
}