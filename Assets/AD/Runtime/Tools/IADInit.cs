namespace AD.Tools
{
    public interface IADInit
    {
        void Init();
    }

    public interface IADInit<in T>
    {
        void Init(T value);
    }
}