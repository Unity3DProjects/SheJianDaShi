namespace AD.Tools
{
    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                return instance = new T();
            }
        }
    }
}