
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Interface
{
    public interface IPage
    {
        // 必要に応じてメソッドやプロパティを追加

        public string SceneName { get; }
        public string JsonParameters { get; }

        public string PageName { get; }

        public async Task Init()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public async Task ObjectLoad()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public async Task PreEntryTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public async Task EntryTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public async Task PostEntryTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public async Task PrePageVisible()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public Task PageVisible();

        public async Task PostPageVisible()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PreAppearanceEffect()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task AppearanceEffect()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PostAppearanceEffect()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task AfterPush()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PreExitTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task ExitTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PostExitTransition()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PrePageInvisible()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public Task PageInvisible();

        public virtual async Task PostPageInvisible()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PrePageExitBackPrevious()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PageExitBackPrevious()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task PostPageExitBackPrevious()
        {
            await Task.RunOnThreadPool(() => { });
        }

        public virtual async Task OnAndroidBack()
        {
            await Task.RunOnThreadPool(() => { });
        }
    }
}
