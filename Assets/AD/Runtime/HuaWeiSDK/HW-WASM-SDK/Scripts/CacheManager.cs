using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace HWWASM
{
    public class CacheManager
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void QG_CacheManagerAddCacheIdentifiers(string conf);

        [DllImport("__Internal")]
        private static extern void QG_CacheManagerAddExcludeCacheIdentifiers(string conf);

        [DllImport("__Internal")]
        private static extern bool QG_CacheManagerHasCache(string url);

        [DllImport("__Internal")]
        private static extern void QG_CacheManagerDeleteCache(string url);

        [DllImport("__Internal")]
        private static extern void QG_CacheManagerClearLru();
#else
        private void QG_CacheManagerAddCacheIdentifiers(string conf)
        {
        }

        private void QG_CacheManagerAddExcludeCacheIdentifiers(string conf)
        {
        }

        private bool QG_CacheManagerHasCache(string url)
        {
            return false;
        }

        private void QG_CacheManagerDeleteCache(string url)
        {
        }

        private void QG_CacheManagerClearLru()
        {
        }
#endif

        /// <summary>
        /// 添加需要进行缓存的url的标识符
        /// </summary>
        /// <param name="cacheIdentifiers">需要缓存的url的标识符列表</param>
        public void AddIdentifiers(List<string> cacheIdentifiers)
        {
            if (cacheIdentifiers == null || cacheIdentifiers.Count == 0)
            {
                return;
            }

            IdentifierOption identifierOption =
                new IdentifierOption { cacheIdentifiers = cacheIdentifiers };
            QG_CacheManagerAddCacheIdentifiers(JsonUtility.ToJson(identifierOption));
        }

        /// <summary>
        /// 添加明确不需要缓存的url的标识符（用于满足AddIdentifiers中需要剔除的情况）
        /// </summary>
        /// <param name="excludeCacheIdentifiers">不需要缓存的url的标识符列表</param>
        public void AddExcludeIdentifiers(List<string> excludeCacheIdentifiers)
        {
            if (excludeCacheIdentifiers == null || excludeCacheIdentifiers.Count == 0)
            {
                return;
            }

            ExcludeIdentifierOption excludeIdentifierOption =
                new ExcludeIdentifierOption { excludeCacheIdentifiers = excludeCacheIdentifiers };
            QG_CacheManagerAddExcludeCacheIdentifiers(JsonUtility.ToJson(excludeIdentifierOption));
        }

        /// <summary>
        /// 是否有url对应的本地缓存资源
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>该缓存是否存在</returns>
        public bool HasCache(string url)
        {
            return QG_CacheManagerHasCache(url);
        }
        
        /// <summary>
        /// 删除url对应的本地缓存资源
        /// </summary>
        /// <param name="url">url</param>
        public void DeleteCache(string url)
        {
            QG_CacheManagerDeleteCache(url);
        }

        /// <summary>
        /// 触发Lru删除本地缓存
        /// </summary>
        public void ClearLru()
        {
            QG_CacheManagerClearLru();
        }

        private class IdentifierOption
        {
            public List<string> cacheIdentifiers;
        }

        private class ExcludeIdentifierOption
        {
            public List<string> excludeCacheIdentifiers;
        }
    }
}