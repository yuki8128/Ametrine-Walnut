

using System;
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Interface
{
    public interface IBook : IDisposable
    {
        public string GetBookName();
        //        初期化処理
        public virtual async Task Init()
        {
            await UniTask.RunOnThreadPool(() => { });
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
        //        自身が開かれた時の処理
        public Task Open();

        //        自身が一時的に閉じられるなど
        public virtual async Task Suspend()
        {
            await UniTask.RunOnThreadPool(() => { });
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
        //        自身が閉じるとき
        public Task Close();

        public Task NextBook(IBook book);

        //        自身にページがpushされた時
        public Task PushPage(IPage page, bool isHistoryClear = false);

        //        自身の配下のページをpopする時
        public Task PopPage();

        //        自身の配下のページをPeekする時
        public IPage PeekPage();

        public IPage DefaultPage();

        //        新しいリストとしてページ全てを返す
        public IPage[] GetPages();
    }
}
