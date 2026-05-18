using System.Collections;
using AD.Instance;
using UnityEngine;

namespace AD
{
    public class AddDeskWindowTiming : TimingComponent
    {
        private Coroutine crt;

        protected override void Enable()
        {
            crt = StartCoroutine(ShowAddDeskPanelUntilFinish());
        }

        protected override void Disable()
        {
            if (crt != null) StopCoroutine(crt);
        }

        public override void Reset()
        {
            enabled = false;
            firstTime = repeatTime = 35;
        }
        
        private IEnumerator ShowAddDeskPanelUntilFinish()
        {
            WaitForSecondsRealtime addTime = new WaitForSecondsRealtime(firstTime);
            yield return addTime;
            addTime = new WaitForSecondsRealtime(repeatTime);
            while (true)
            {
                AddDeskWindow.Instance.ShowAddDeskPanel();
                yield return addTime;
            }
        }
    }
}