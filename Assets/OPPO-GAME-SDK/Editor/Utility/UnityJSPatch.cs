using System.IO;

namespace QGMiniGame
{
    public static class UnityJSPatch
    {
        private static readonly (string old, string @new)[] Replace = new (string, string)[]
        {
            // loop 属性的 getter
            ("return source.mediaElement.loop", "if(!source.mediaElement)return null;return source.mediaElement.loop"),

            // loop 属性的 setter
            ("if(source.mediaElement.loop!==v)source.mediaElement.loop=v",
                "if(!source.mediaElement)return;if(source.mediaElement.loop!==v)source.mediaElement.loop=v"),

            // currentTime 属性的 getter
            ("return source.mediaElement.currentTime",
                "if(!source.mediaElement)return null;return source.mediaElement.currentTime"),

            // currentTime 属性的 setter
            ("if(source.mediaElement.currentTime!==v)source.mediaElement.currentTime=v",
                "if(!source.mediaElement)return;if(source.mediaElement.currentTime!==v)source.mediaElement.currentTime=v"),

            // mute 属性的 getter
            ("return source.mediaElement.mute", "if(!source.mediaElement)return null;return source.mediaElement.mute"),

            // mute 属性的 setter
            ("if(source.mediaElement.mute!==v)source.mediaElement.mute=v",
                "if(!source.mediaElement)return;if(source.mediaElement.mute!==v)source.mediaElement.mute=v"),

            // playbackRate.value 属性的 getter
            ("return source.mediaElement.playbackRate",
                "if(!source.mediaElement)return null;return source.mediaElement.playbackRate"),

            // playbackRate.value 属性的 setter
            ("if(source.mediaElement.playbackRate!==v)source.mediaElement.playbackRate=v",
                "if(!source.mediaElement)return;if(source.mediaElement.playbackRate!==v)source.mediaElement.playbackRate=v"),

            // onended 属性的 getter
            ("return source.mediaElement.onended",
                "if(!source.mediaElement)return null;return source.mediaElement.onended"),

            // onended 属性的 setter - 注意这里没有条件判断，所以直接添加空值检查
            ("source.mediaElement.onended=onended",
                "if(!source.mediaElement)return;source.mediaElement.onended=onended"),

            // 处理方法调用，如果存在的话
            ("source.mediaElement.play()", "source.mediaElement&&source.mediaElement.play()"),

            ("source.mediaElement.pause()", "source.mediaElement&&source.mediaElement.pause()"),

            ("source.mediaElement.load()", "source.mediaElement&&source.mediaElement.load()"),
        };

        public static void ApplyPatch(string dir)
        {
            var files = Directory.GetFiles(Path.Combine(dir, "Build"), "*.framework.js", SearchOption.TopDirectoryOnly);
            if (files.Length == 0) return;
            var filePath = files[0];
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"文件未找到: {filePath}");

            string content = File.ReadAllText(filePath);

            // 按顺序应用所有替换规则
            foreach (var replacement in Replace)
            {
                // 使用字符串替换
                content = content.Replace(replacement.old, replacement.@new);
            }

            File.WriteAllText(filePath, content);
        }
    }
}