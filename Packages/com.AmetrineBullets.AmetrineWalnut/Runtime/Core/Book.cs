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
                // 各ページからBookへの参照をクリア
                page.ParentBook = null;
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
            
            // ページをスタックから削除し、Bookへの参照をクリア
            _pageHistory.Pop();
            page.ParentBook = null;
        }

        public virtual IPage PeekPage()
        {
            if (_pageHistory.TryPeek(out IPage page))
            {
                return page;
            }
            return null;
        }

        public IPage GoToBackPage(string pageName)
        {
            var page = _pageHistory.PopForTargetItem(pageName);
            if (page != null)
            {
                // 削除されたページからBookへの参照をクリア
                page.ParentBook = null;
            }
            return page;
        }

        public IPage GetDefaultPage()
        {
            return defaultPage;
        }
        public void SetDefaultPage(IPage page)
        {
            defaultPage = page;
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

            // 最初の要素は上ですでに非表示にしているので処理をスキップ 
            foreach (var item in _pageHistory.Skip(1))
            {
                await EasyPageHide(item);
            }
        }

        // ページを表示したい時の簡単セット
        public async Task EasyPageView(IPage page)
        {
            // 新しいページの表示処理
            // TODO この辺の処理がUnityのライフサイクル(Startとか)と一致していないのでどこかでStartが終わって処理が再開するような感じにしたい。

            await page.Init();

            await page.ObjectLoad();
            
            // ページにこのBookへの参照を設定
            page.ParentBook = this;
            
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
            await page.PostExitTransition();

            await EasyPageHide(page);

            // ページからBookへの参照をクリア
            page.ParentBook = null;
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
