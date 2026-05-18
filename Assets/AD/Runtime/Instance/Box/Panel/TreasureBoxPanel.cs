using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AD.Instance
{
    internal class TreasureBoxPanel : GroupBase<TreasureBoxPanel>
    {
        private Slider BoxSlider;
        private float MaxValue;
        private float LostSpeed = 2;
        private GameObject[] closeGOs = new GameObject[0];

        protected override void InitComponent()
        {
            var root = transform;
            MaxValue = 10;
            LostSpeed = 1;
            BoxSlider = root.Find("Slider").GetComponent<Slider>();
            closeGOs = new GameObject[]
            {
                root.Find("Close").gameObject,
                root.Find("OpenBox/VideoIcon").gameObject,
            };
            root.Find("Close").GetComponent<Button>().InitButton(Close);
            root.Find("OpenBox").GetComponent<Button>().InitButton(OnClickOpenBox);
        }

        private float randomIncrement;
        private float TriggerValue;
        private bool IsFinish;


        private Action onSuccess, onFailure, onFinish;
        private bool closeable = false;

        public TreasureBoxPanel OnSuccess(Action action)
        {
            this.onSuccess = action;
            return this;
        }

        public TreasureBoxPanel OnFinish(Action action)
        {
            onFinish = action;
            return this;
        }

        public TreasureBoxPanel SetCloseable(bool v)
        {
            this.closeable = v;
            return this;
        }

        public void Done()
        {
            Show();
        }


        private void CallSuccess()
        {
            try
            {
                onSuccess?.Invoke();
                onSuccess = null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void CallFailure()
        {
            try
            {
                onFailure?.Invoke();
                onFailure = null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void CallFinish()
        {
            try
            {
                onFinish?.Invoke();
                onFinish = null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Init()
        {
            IsFinish = false;
            BoxSlider.maxValue = MaxValue;
            BoxSlider.value = 0f;
            StartCoroutine(nameof(LoseProgress));
        }

        protected override void OnShow()
        {
            Init();
            randomIncrement = Random.Range(MaxValue * 0.05f, MaxValue * 0.3f);
            float halfMaxValue = BoxSlider.maxValue / 2f;
            float minTriggerValue = halfMaxValue - (BoxSlider.maxValue * 0.2f); // 最大值的50% - 20%
            float maxTriggerValue = halfMaxValue + (BoxSlider.maxValue * 0.2f); // 最大值的50% + 30%
            TriggerValue = Random.Range(minTriggerValue, maxTriggerValue);
            foreach (var go in closeGOs) go.SetActive(closeable);
        }

        protected override void OnHide()
        {
            StopCoroutine(nameof(LoseProgress));

            CallFinish();

            onSuccess = null;
            onFailure = null;
            onFinish = null;
            closeable = false;
        }


        private void Update()
        {
            if (Displaying)
                CheckIsFinish();
        }

        private void CheckIsFinish()
        {
            var currentValue = GetCurrentSliderValue(BoxSlider);
            if (currentValue >= TriggerValue && !IsFinish)
            {
                IsFinish = true;
                try
                {
                    ADManager.Instance.ShowRewardAD(CallSuccess, CallFailure);
                    ADManager.Instance.Show(AdSourceType.Black);
                }
                catch (Exception e)
                {
                    throw new Exception("请检查激励广告参数或初始化");
                }

                Hide();
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

        private void Close()
        {
            CallFailure();
            Hide();
        }

        private void OnClickOpenBox()
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
    }
}