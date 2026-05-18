using AD.Instance;

namespace AD
{
    public class EyeCareTiming : TimingComponent
    {
        protected override void Enable()
        {
            InvokeRepeating(nameof(ShowEyeCare), firstTime, repeatTime);
        }

        protected override void Disable()
        {
            CancelInvoke(nameof(ShowEyeCare));
        }

        public override void Reset()
        {
            enabled = false;
            firstTime = 180;
            repeatTime = 120;
        }

        public void ShowEyeCare()
        {
            EyeCareWindow.Instance.ShowEyeCare();
        }
    }
}