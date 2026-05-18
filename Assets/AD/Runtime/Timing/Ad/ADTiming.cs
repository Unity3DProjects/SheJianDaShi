namespace AD
{
    public class ADTiming : TimingComponent
    {

        protected override void Enable()
        {
            InvokeRepeating(nameof(ShowAdWithLimite), firstTime, repeatTime);
        }

        protected override void Disable()
        {
            CancelInvoke(nameof(ShowAdWithLimite));
        }

        public override void Reset()
        {
            enabled = false;
            firstTime = repeatTime = 45;
        }

        private void ShowAdWithLimite()
        {
            ADManager.Instance.Show(AdSourceType.TimingAd);
        }
    }
}