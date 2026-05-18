using System;
using System.Collections.Generic;
using System.Linq;
using AD.BlackBag.Shield;
using AD.NodeTree;
using UnityEditor;
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.Editor
{
    internal class TreeDataDisplayDB : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<Entry> values = new List<Entry>();

        public void Init(Tree tree, TreeDataContainer container)
        {
            values.Clear();
            foreach (var pair in tree.Data)
            {
                values.Add(new Entry()
                {
                    key = pair.Key,
                    value = container.Data.GetValue(pair.Key.Group, pair.Key.key)
                });
            }
        }

        [Serializable]
        internal class Entry
        {
            public Tree.Key key;
            [SerializeReference] public Tree.Value value;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}