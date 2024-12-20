namespace OleksiiStepanov.Utils
{
    /// <summary>
    /// Singleton Implementation
    /// </summary>
    public abstract class Singleton<T> where T: new()
    {
        protected static T instance = default(T);
        protected Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if(instance == null || instance.Equals(default(T)))
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
