using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class SampleTopPage : Page<SampleTopPage>
{
    private SampleTopPage() : base()
    {
        Debug.Log("SampleTopPage Constractor done");
    }

//    private static Lazy<SampleTopPage> instance = new Lazy<SampleTopPage>();
//    public static SampleTopPage Instance => instance.Value;


    public override bool IsEqualPage(PageNameEnum pageName)
    {
        Debug.Log("isisisisis");
//        return pageName == this.PageName ? true : false;
        return true;
    }

    GameObject prefab;

    GameObject go;

    public override async UniTask OnActivate()
    {
        UnityEngine.Debug.Log("Hello Page2");
        if (go != null)
        {
            go.SetActive(true);
        }

        UnityEngine.Debug.Log("prefab2");

        var myCanvas = (Canvas)Object.FindObjectOfType(typeof(Canvas));

        UnityEngine.Debug.Log(
            $"{prefab} {myCanvas} {myCanvas.name} {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
//        go = Object.Instantiate(prefab, new Vector3(100.0f, 100.0f, -8.0f), Quaternion.Euler(0f, 0f, 0f),
//            myCanvas.transform);
        go = Object.Instantiate(prefab, myCanvas.transform);

        await UniTask.RunOnThreadPool(() => { });
    }

    public override UniTask PageVisible()
    {
        throw new NotImplementedException();
    }

    public override UniTask PageInvisible()
    {
        throw new NotImplementedException();
    }

    public override async UniTask OnTransitionIn(UniTaskCompletionSource loadPageCompletionSource)
    {
        UnityEngine.Debug.Log("OnTransitionStart");
        await loadPageCompletionSource.Task;

        UnityEngine.Debug.Log("OnTransitionEnd");
    }

    
    public static Func<object, SampleTopPage > CreateInstanceDelegate => (param) =>
    {
        return new SampleTopPage();
    }; 
}