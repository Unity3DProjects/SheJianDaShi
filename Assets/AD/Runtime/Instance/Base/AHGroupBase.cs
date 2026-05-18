using UnityEngine;

namespace AD.Instance
{
    public abstract class AHGroupBase<T> : GroupBase<T>
        where T : AHGroupBase<T>
    {
        private AgreementDataSO _agreementDataSo;

        protected AgreementDataSO AgreementDataSo
        {
            get
            {
                if (_agreementDataSo == null)
                {
                    _agreementDataSo = Resources.Load<AgreementDataSO>("AgreementDataSO");
                }

                return _agreementDataSo;
            }
        }
    }
}