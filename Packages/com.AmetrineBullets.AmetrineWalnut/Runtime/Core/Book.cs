using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Core
{
    public abstract class Book : IBook
    {
        public string BookName { get; }

        protected IPage defaultPage;

        private StackKeyedCollection<String, IPage> _pageHistory =
            new StackKeyedCollection<String, IPage>(page => page.PageName);

        public abstract Task Open();

        public Task Close()
        {
            throw new System.NotImplementedException();
        }

        public Task NextBook(IBook book)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task PushPage(IPage page, bool isHistoryClear = false)
        {
            UniTask EntryTransitionTask = new UniTask();

            await page.Init();

            //        元々表示されてるページの非表示処理
            if (_pageHistory.Count > 0)
            {
                IPage previousPage = _pageHistory.Peek();
                await previousPage.PreExitTransition();
                await previousPage.ExitTransition();
                await previousPage.PostEntryTransition();

                await previousPage.PrePageInvisible();
                await previousPage.PageInvisible();
                await previousPage.PostPageInvisible();
            }

            if (isHistoryClear)
            {
                _pageHistory.Clear();
            }

            //        重複したページをpushした時の動作
            if (_pageHistory.Count > 0)
            {
                if (_pageHistory.Contains(page.ToString()))
                {
                    _pageHistory.Remove(page.ToString());
                }
            }

            //        新しいページの表示処理

            await page.ObjectLoad();
            _pageHistory.Push(page);

            await page.PreEntryTransition();
            await page.EntryTransition();
            await page.PostEntryTransition();

            await page.PrePageVisible();
            await page.PageVisible();
            await page.PostPageVisible();

            await page.PreAppearanceEffect();
            await page.AppearanceEffect();
            await page.PostAppearanceEffect();
        }

        public virtual async Task PopPage()
        {
            var page = PeekPage();

            await page.PrePageExitBackPrevious();
            await page.PageExitBackPrevious();
            await page.PostPageExitBackPrevious();

            await page.PreExitTransition();
            await page.ExitTransition();
            await page.PostEntryTransition();

            await page.PrePageInvisible();
            await page.PageInvisible();
            await page.PostPageInvisible();

            _pageHistory.Pop();
        }

        public virtual IPage PeekPage()
        {
            return _pageHistory.Peek();
        }

        public IPage GoToBackPage(string pageName)
        {
            return _pageHistory.PopForTargetItem(pageName);
        }

        public IPage DefaultPage()
        {
            return defaultPage;
        }

        public IPage[] GetPages()
        {
            return _pageHistory.ToArray();
        }

        public abstract void Dispose();
    }
}