using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class EventFixerEditor : EditorWindow
{
   private string searchPath = "Assets";

   
   [MenuItem("Tools/批量修复事件调用方式")]
   public static void ShowWindow()
   {
      GetWindow<EventFixerEditor>("事件调用形式 修复工具");
      
   }

   private void OnGUI()
   {
      GUILayout.Label("扫描路径",EditorStyles.boldLabel);
      searchPath = EditorGUILayout.TextField("文件夹路径", searchPath);

      if (GUILayout.Button("开始扫描并修复"))
      {
         int count = FixOldEventCalls(searchPath);
         EditorUtility.DisplayDialog("完成", $"已修改 {count} 个文件。", "好的");
      }

   }

   public static int FixOldEventCalls(string rootPath)
   {
      string[] files = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories);
      int modifiedCount = 0;
      
      string pattern = @"if\s*\(\s*this\.(\w+)\s*!=\s*null\s*\)\s*\{\s*this\.\1\s*\(\s*\);\s*\}";
      Regex regex = new Regex(pattern);

      foreach (var file in files)
      {
         string content = File.ReadAllText(file);
         if (regex.IsMatch(content))
         {
            string newContent = regex.Replace(content, "this.$1?.Invoke();");
            File.WriteAllText(file, newContent);
            Debug.Log($"已修复：{file}");
            modifiedCount++;
         }
      }

      AssetDatabase.Refresh();
      return modifiedCount;
   }
   
}
