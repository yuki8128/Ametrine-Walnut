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
        protected string BookName;

        public string GetBookName()
        {
            return BookName;
        }

        protected IPage defaultPage;

        protected StackKeyedCollection<String, IPage> _pageHistory =
            new StackKeyedCollection<String, IPage>(page => page.PageName);

        public abstract Task Open(IPage page = null);

        public virtual async Task Close()
        {
            // Page スタックにあるすべての Page を非表示にする
            foreach (var page in _pageHistory.Reverse())
            {
                await EasyPageHide(page);
            }
            // Page スタックをクリア
            _pageHistory.Clear();
        }

        public virtual async Task PushPage(IPage page, bool isHistoryClear = false)
        {
            UniTask EntryTransitionTask = new UniTask();

            // 元々表示されているページを非表示にする必要はないのでは？
            // ページはスタックして表示するから非表示にする意味がないはず。 
            // // 元々表示されてるページの非表示処理
            // if (_pageHistory.Count > 0)
            // {
            //     IPage previousPage = _pageHistory.Peek();
            //     await previousPage.PreExitTransition();
            //     await previousPage.ExitTransition();
            //     await previousPage.PostEntryTransition();

            //     await previousPage.PrePageInvisible();
            //     await previousPage.PageInvisible();
            //     await previousPage.PostPageInvisible();
            // }

            if (isHistoryClear)
            {
                _pageHistory.Clear();
            }

            if (_pageHistory.Count > 0)
            {
                // 重複したページをpushした時の動作
                if (_pageHistory.Contains(page.ToString()))
                {
                    _pageHistory.Remove(page.ToString());
                }
            }

            await EasyPageView(page);

        }

        public virtual async Task PopPage()
        {
            var page = PeekPage();

            await page.PrePageExitBackPrevious();
            await page.PageExitBackPrevious();
            await page.PostPageExitBackPrevious();

            await EasyPageHide(page);

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

        /// <summary>
        /// デバッグ用にPageHistoryの情報を取得します。
        /// </summary>
        /// <returns>PageHistoryのIPageのコレクション</returns>
        public IEnumerable<IPage> GetPageHistory()
        {
            return _pageHistory;
        }

        public async Task Suspend()
        {
            if (_pageHistory.Count > 0)
            {
                IPage page = _pageHistory.Peek();
                await page.PreExitTransition();
                await page.ExitTransition();
                await page.PostEntryTransition();

                await page.PrePageInvisible();
                await page.PageInvisible();
                await page.PostPageInvisible();
            }

            foreach (var item in _pageHistory)
            {
                await EasyPageHide(item);
            }
        }

        // ページを表示したい時の簡単セット
        public async Task EasyPageView(IPage page)
        {
            // 新しいページの表示処理

            await page.Init();

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

        public async Task EasyPageExit(IPage page)
        {
            await page.PreExitTransition();
            await page.ExitTransition();
            await page.PostEntryTransition();

            await EasyPageHide(page);

            _pageHistory.Pop();
        }

        public async Task EasyPageHide(IPage page)
        {
            await page.PrePageInvisible();
            await page.PageInvisible();
            await page.PostPageInvisible();
        }

        public abstract void Dispose();
    }
}
