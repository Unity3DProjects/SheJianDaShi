namespace AD.Instance
{
    internal class EyeCareWindow : GroupManagerBase<EyeCareWindow, EyeCarePanel>
    {
        /// <summary>
        /// 护眼提示
        /// </summary>
        public void ShowEyeCare()
        {
            GroupInstance.Show();
            ADManager.Instance.Show(AdSourceType.Black);
        }
    }
}