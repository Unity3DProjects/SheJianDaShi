namespace AD.Decorator
{
    public interface IADParamDecorator
    {
        bool Pass { get; }
#if UNITY_EDITOR
        void Layout();
#endif
    }
}