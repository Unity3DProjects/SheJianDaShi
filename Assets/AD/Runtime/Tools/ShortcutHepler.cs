using System;
using UnityEngine;
#if VIVO || OPPO
using QGMiniGame;
#endif

public static class ShortcutHepler
{
    public static void HasShortcutInstalled(Action<bool> callBack)
    {
#if !UNITY_EDITOR
#if VIVO
        QG.HasShortcutInstalled(succCallback: msg => { callBack?.Invoke(msg.data.hasShortcutInstalled); });
#endif
#if OPPO
        QG.HasShortcutInstalled(succCallback: msg => { callBack?.Invoke(msg.hasShortcutInstalled); });
#endif
#endif
    }

    public static void TryInstallShortcut()
    {
        Debug.Log("Try添加桌面");
        HasShortcutInstalled(installed =>
        {
            if (!installed)
            {
                ADManager.Instance.CreateShortcutButton();
            }
        });
    }
}