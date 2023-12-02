using System;

public abstract class LazySingleton<T> where T : LazySingleton<T>
{
    private static Lazy<T> lazyInstance;
    
    protected  static object para;
//        new Lazy<T>(CreateInstance);

//    public static T Instance => lazyInstance.Value;
    public static T Instance
    {
        get
        {
            if (lazyInstance == null)
            {
                lazyInstance = new Lazy<T>(() => CreateInstanceDelegate != null ? CreateInstanceDelegate(para) : (T)Activator.CreateInstance(typeof(T), true));
            }
            return lazyInstance.Value;
        }
    }
    protected LazySingleton()
    {
        // コンストラクタ内でのインスタンスの設定は不要です。
        // Lazy<T> がインスタンスの生成と管理を行います。
    }

    protected static Func<object ,T> CreateInstanceDelegate;

//    protected abstract static T CreateInstance();
//    {
//        return Activator.CreateInstance(typeof(T), true) as T;
//    }
}