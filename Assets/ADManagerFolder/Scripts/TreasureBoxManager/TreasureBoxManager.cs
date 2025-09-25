using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBoxManager : MonoBehaviour
{
    public Slider BoxSlider;
    public float MaxValue;
    public float LostSpeed = 2;
    private float randomIncrement;
    private float TriggerValue;
    private bool IsFinish;

    private void Init()
    {
        IsFinish = false;
        BoxSlider.maxValue = MaxValue;
        BoxSlider.value = 0f;
        StartCoroutine(LoseProgress());
    }

    private void OnEnable()
    {
        Init();
        // Time.timeScale = 0;
        float halfMaxValue = BoxSlider.maxValue / 2f;
        float minTriggerValue = halfMaxValue - (BoxSlider.maxValue * 0.2f); // 最大值的50% - 20%
        float maxTriggerValue = halfMaxValue + (BoxSlider.maxValue * 0.2f); // 最大值的50% + 30%
        TriggerValue = Random.Range(minTriggerValue, maxTriggerValue);
        // Debug.Log(TriggerValue);
        // Debug.Log(randomIncrement);
    }

    private void Update()
    {
        CheckIsFinish();
    }

    private void CheckIsFinish()
    {
        var CurrentValue = GetCurrentSliderValue(BoxSlider);
        if (CurrentValue >= TriggerValue && !IsFinish)
        { 
            IsFinish = true;
            ADManager.Instance.ShowAD_Reward(gameObject.name, nameof(Play));
            gameObject.SetActive(false);
           
        }
    }

    public void Play()
    {
        Time.timeScale = 1;
    }

    private IEnumerator LoseProgress()
    {
        while (!IsFinish)
        {
            BoxSlider.value -= (Time.deltaTime * LostSpeed);
            yield return null;
        }
    }

    public void OnClickOpenBox()
    {
        randomIncrement = Random.Range(MaxValue * 0.05f, MaxValue * 0.3f);
        IncreaseSliderValue(BoxSlider, randomIncrement);
    }

    private float GetCurrentSliderValue(Slider slider)
    {
        return slider.value;
    }

    private void IncreaseSliderValue(Slider slider, float value)
    {
        slider.value += value;
    }

    private void OnDisable()
    {
        StopCoroutine(LoseProgress());
    }
}