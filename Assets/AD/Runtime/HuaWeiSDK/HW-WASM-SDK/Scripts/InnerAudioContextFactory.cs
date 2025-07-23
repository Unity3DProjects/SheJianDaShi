using System;
using System.Collections.Generic;
using UnityEngine;

namespace HWWASM
{
    public class InnerAudioContextFactory
    {
        private readonly Dictionary<string, InnerAudioContext>
            _dictionary = new Dictionary<string, InnerAudioContext>();

        public static InnerAudioContextFactory Instance { get; } = new InnerAudioContextFactory();

        public InnerAudioContext CreateInnerAudioContext(InnerAudioContextOption param)
        {
            string callbackId = Guid.NewGuid().ToString();
            InnerAudioContext innerAudioContext = new InnerAudioContext(callbackId, param);
            _dictionary.Add(callbackId, innerAudioContext);
            return innerAudioContext;
        }

        public InnerAudioContext _GetInnerAudioContext(string callbackId)
        {
            return _dictionary.ContainsKey(callbackId) ? _dictionary[callbackId] : null;
        }

        public void _RemoveInnerAudioContext(string callbackId)
        {
            if (_dictionary.ContainsKey(callbackId))
            {
                _dictionary.Remove(callbackId);
            }
        }
    }
}