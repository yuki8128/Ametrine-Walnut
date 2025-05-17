using System;
using UnityEngine;
using com.AmetrineBullets.AmetrineWalnut.Interface;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Core
{

    public abstract class Page<T> : IPage where T : Page<T>
    {
        protected Page(string jsonParameters = null)
        {
            //        this.PageName = pageName;

            this.JsonParameters = jsonParameters;
        }


        public string SceneName { get; private set; }
        public string JsonParameters { get; private set; }

        public string PageName { get; set; }

        public abstract bool IsEqualPage(PageNameEnum pageName);

        public virtual async Task OnActivate()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public abstract Task PageVisible();

        public abstract Task PageInvisible();

        public virtual async Task OnTransitionIn(UniTaskCompletionSource loadPageCompletionSource)
        {
            await Task.RunOnThreadPool(() => { });
        }
    }
}
