using System;

#if VIVO || OPPO
using QGMiniGame;
#endif

public static class ShortcutHepler
{
#if VIVO
    public static void HasShortcutInstalled(Action<bool> callBack)
    {
        QG.HasShortcutInstalled(succCallback: msg => { callBack?.Invoke(msg.data.hasShortcutInstalled); });
    }
#elif OPPO
    public static void HasShortcutInstalled(Action<bool> callBack)
    {
        QG.HasShortcutInstalled(succCallback: msg => { callBack?.Invoke(msg.hasShortcutInstalled); });
    }

#else
    public static void HasShortcutInstalled(Action<bool> callBack)
    {
    }
#endif
}