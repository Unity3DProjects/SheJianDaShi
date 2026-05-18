using System;
using AD.Tools;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TitleController : MonoBehaviour, IADInit
{
    private Image title;

    private TitleSO _titleSo;
    public TitleSO TitleSo => _titleSo == null ? Resources.Load<TitleSO>("TitleSO") : _titleSo;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        title = GetComponent<Image>();
        title.preserveAspect = true;
        title.color = Color.white;
        title.sprite = TitleSo.title != null ? TitleSo.title : title.sprite;
        title.enabled = false;
#if UNITY_WEBGL
        if (ADManager.Instance != null)
        {
            title.enabled = (title.sprite != null && ADManager.Instance.AdAdapter.HasTitle);
        }
#endif
    }
}