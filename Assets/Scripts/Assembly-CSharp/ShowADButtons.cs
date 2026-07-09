using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowADButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject ADButtons;

    private void Awake()
    {
        SetTargetsActive(false);
    }

    private void OnEnable()
    {
        MenuScreen.OnChangeCurrentPhase += OnMenuPhaseChanged;
    }

    private void OnDisable()
    {
        MenuScreen.OnChangeCurrentPhase -= OnMenuPhaseChanged;
    }

    private void OnMenuPhaseChanged(MenuPhase phase)
    {
        SetTargetsActive(phase == MenuPhase.Mode);
    }

    private void SetTargetsActive(bool active)
    {
        if (ADButtons == null)
        {
            return;
        }
        else
        {
            ADButtons.SetActive(active);
        }
    }
}
