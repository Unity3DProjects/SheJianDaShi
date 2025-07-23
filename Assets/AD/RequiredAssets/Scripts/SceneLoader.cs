using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour, IPointerClickHandler
{
    public string sceneName;


    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log($"{nameof(SceneLoader)}:场景切换到{sceneName}");
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderEditor : Editor
{
    private int selectedSceneIndex = -1;
    private string[] sceneNames;

    private void OnEnable()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        sceneNames = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            sceneNames[i] = Path.GetFileNameWithoutExtension(
                SceneUtility.GetScenePathByBuildIndex(i));
        }

        SceneLoader loader = (SceneLoader)target;
        selectedSceneIndex = GetSceneIndex(loader.sceneName);
    }

    public override void OnInspectorGUI()
    {
        SceneLoader loader = (SceneLoader)target;

        int newSelectedSceneIndex = EditorGUILayout.Popup("Scene to Load", selectedSceneIndex, sceneNames);

        if (newSelectedSceneIndex != selectedSceneIndex)
        {
            selectedSceneIndex = newSelectedSceneIndex;
            loader.sceneName = sceneNames[selectedSceneIndex];
            EditorUtility.SetDirty(loader);
        }

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("Scene Name", loader.sceneName);
        EditorGUI.EndDisabledGroup();
    }

    private int GetSceneIndex(string sceneName)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (sceneNames[i] == sceneName)
            {
                return i;
            }
        }

        return 0;
    }
}
#endif