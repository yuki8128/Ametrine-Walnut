public abstract class BaseSingleton<T> where T : BaseSingleton<T>
{
    public static T Instance;

    public BaseSingleton()
    {
        Instance = (T)this;
    }
}

public class Singleton : BaseSingleton<Singleton> { }