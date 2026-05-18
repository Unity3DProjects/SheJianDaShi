using UnityEngine;

namespace AD
{
    public abstract class TimingComponent : MonoBehaviour
    {
        public int firstTime;
        public int repeatTime;
        public virtual void Init()
        {
        }

        public void StartTiming()
        {
            if (enabled) Enable();
            else Disable();
        }

        protected abstract void Enable();
        protected abstract void Disable();
        public abstract void Reset();
        private void Start(){}
    }
}