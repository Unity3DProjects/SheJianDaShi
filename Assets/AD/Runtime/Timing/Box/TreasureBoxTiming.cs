using System.Collections;
using AD.Instance;
using UnityEngine;

namespace AD
{
    public class TreasureBoxTiming : TimingComponent
    {
        private Coroutine crt;

        protected override void Enable()
        {
            crt = StartCoroutine(CoShowBox());
        }

        protected override void Disable()
        {
            if (crt != null) StopCoroutine(crt);
        }

        public override void Reset()
        {
            enabled = false;
            firstTime = 180;
            repeatTime = 120;
        }

        private IEnumerator CoShowBox()
        {
            yield return new WaitForSecondsRealtime(firstTime);

            while (true)
            {
                TreasureBoxWindow.Instance.ShowTreasureBox();
                yield return new WaitForSecondsRealtime(repeatTime);
            }
        }
    }
}