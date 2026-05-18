using System;
using System.Collections.Generic;
using AD.Tools;
using UnityEngine;
using UnityEngine.Scripting;

public partial class ADManager
{
    private IDictionary<string, List<Action<string>>> binding = new Dictionary<string, List<Action<string>>>();

    private void InitReceiver()
    {
        binding.Clear();
    }

    public void RemoveAllReceiver(string key)
    {
        binding.Remove(key);
    }

    public void RemoveReceiver(string key, Action<string> receiver)
    {
        binding.RemoveItem(key, receiver);
    }

    public void BindReceiver(string key, Action<string> receiver)
    {
        binding.AddItem(key, receiver);
    }


    public void Receiver(string msg)
    {
        var receiveMsg = JsonUtility.FromJson<ReceiveMsg>(msg);
        if (receiveMsg != null && receiveMsg.key != null && binding.TryGetValue(receiveMsg.key, out var list))
        {
            using (ListPool<Action<string>>.Get(out var temp))
            {
                temp.AddRange(list);
                foreach (var action in temp)
                {
                    try
                    {
                        action(receiveMsg.data);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }
    }

    [Serializable, Preserve]
    public class ReceiveMsg
    {
        public string key;
        public string data;
    }
}