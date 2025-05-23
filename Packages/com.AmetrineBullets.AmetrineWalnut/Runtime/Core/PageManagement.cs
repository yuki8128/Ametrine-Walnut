using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif
using com.AmetrineBullets.AmetrineWalnut.Interface;

namespace com.AmetrineBullets.AmetrineWalnut.Core
{
    public class PageManagement : IPageManagement
    {
        protected static PageManagement instance { get; set; }

        protected IDesk _desk;

        protected bool isInited = false;
        public bool IsInitialized
        {
            get { return this.isInited; }
        }

        protected PageManagement()
        {

        }

        public static PageManagement Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = new PageManagement();
                instance._desk = new Desk();
                return instance;
            }
        }

        public virtual async Task Init(IBook defaultBook, IPage defaultPage, bool isForceInit = false)
        {
            if (!isInited || isForceInit)
                await _desk.ClearDesk();
            _desk.SetDefaultBook(defaultBook);
            await _desk.PushBook(defaultBook, defaultPage);
            isInited = true;
        }

        public async Task PushPage(IPage page, IBook book = null, bool isClearHistory = false)
        {
            if (book != null)
            {
                await _desk.PushBook(book, page, isClearHistory);
            }
            else
            {
                await _desk.GetCurrentBook().PushPage(page, isClearHistory);
            }
        }

        public async Task PopPage()
        {
            await _desk.PeekBook().PopPage();
        }

        public async Task PopBook()
        {
            await _desk.PopBook();
        }

        public async Task PopTargetPage(IPage page)
        {
            // 現在のBookから指定されたページを履歴から削除
            _desk.PeekBook().GoToBackPage(page.PageName);
            await Task.CompletedTask; // 非同期メソッドとして定義するため
        }

        public IPage PeekPage()
        {
            return _desk.PeekBook().PeekPage();
        }

        public IBook PeekBook()
        {
            return _desk.PeekBook();
        }

        public void SetDesk(IDesk desk)
        {
            this._desk = desk;
        }
    }
}
