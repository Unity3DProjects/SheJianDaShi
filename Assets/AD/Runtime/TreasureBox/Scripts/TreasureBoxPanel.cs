using System;
using System.Collections;
using AD.Runtime.BlackBag;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AD.Runtime.TreasureBox.Scripts
{
    public class TreasureBoxPanel : MonoBehaviour
    {
        public Slider BoxSlider;
        public float MaxValue;
        public float LostSpeed = 2;
        private float randomIncrement;
        private float TriggerValue;
        private bool IsFinish;


        public Action OnFinish;

        public void AddListnner(Action OnFinish)
        {
            this.OnFinish = null;
            this.OnFinish += OnFinish;
        }

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
            randomIncrement = Random.Range(MaxValue * 0.05f, MaxValue * 0.3f);
            float halfMaxValue = BoxSlider.maxValue / 2f;
            float minTriggerValue = halfMaxValue - (BoxSlider.maxValue * 0.2f); // 最大值的50% - 20%
            float maxTriggerValue = halfMaxValue + (BoxSlider.maxValue * 0.2f); // 最大值的50% + 30%
            TriggerValue = Random.Range(minTriggerValue, maxTriggerValue);
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
                if (BlackBagManager.Instance.CanShowAD)
                {
                    try
                    {
                        ADManager.Instance.ShowRewardAD(name, "");
                        ADManager.Instance.ShowBlackAd();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("请检查激励广告参数或初始化");
                    }
                }

                OnFinish?.Invoke();
                OnFinish = null;
                gameObject.SetActive(false);
            }
        }

        private IEnumerator LoseProgress()
        {
            while (!IsFinish)
            {
                BoxSlider.value -= (Time.unscaledDeltaTime * LostSpeed);
                yield return null;
            }
        }

        public void OnClickOpenBox()
        {
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
}