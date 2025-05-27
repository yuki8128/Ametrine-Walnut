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
        protected static PageManagement instance { get; private set; }
        protected IDesk _desk;
        protected bool isInited = false;
        public bool IsInitialized => isInited;

        public static PageManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    // インスタンスが設定されていない場合はエラーをスローします。
                    // アプリケーションの初期化フローで派生クラスのインスタンスが設定されることを期待します。
                    throw new InvalidOperationException(
                        "PageManagement.Instance is not initialized. " +
                        "Ensure a derived class (like AppPageManager) initializes and sets this instance " +
                        "via SetBaseInstance() or through its own Instance property."
                    );
                }
                return instance;
            }
        }

        protected PageManagement()
        {
            // Deskのインスタンスはコンストラクタで生成します。
            _desk = new Desk();
        }

        // 派生クラスがシングルトンインスタンスを設定するために使用します。
        protected static void SetBaseInstance(PageManagement newInstance)
        {
            if (instance != null && instance != newInstance)
            {
                // 既存のインスタンスを置き換える場合は警告などを出すことも考慮できます。
                // Debug.LogWarning("PageManagement.instance is being replaced.");
            }
            instance = newInstance;
        }

        public virtual async Task Init(IBook defaultBook, IPage defaultPage, bool isForceInit = false)
        {
            if (!isInited || isForceInit)
                await _desk.ClearDesk();

            defaultBook.SetDefaultPage(defaultPage);
            _desk.SetDefaultBook(defaultBook);

            isInited = true;
        }

        public virtual async Task PushPage(IPage page, IBook book = null, bool isClearHistory = false)
        {
            if (book != null)
            {
                await _desk.PushBook(book, page, isClearHistory);
            }
            else
            {
                await _desk.GetCurrentBook().PushPage(page, isClearHistory);
            }

            page.AfterPush().Forget();

        }

        public virtual async Task PopPage()
        {
            await _desk.PeekBook().PopPage();
        }

        public virtual async Task PopBook()
        {
            await _desk.PopBook();
        }

        public virtual async Task PopTargetPage(IPage page)
        {
            // 現在のBookから指定されたページを履歴から削除
            _desk.PeekBook().GoToBackPage(page.PageName);
            await Task.CompletedTask; // 非同期メソッドとして定義するため
        }

        public virtual IPage PeekPage()
        {
            return _desk.PeekBook().PeekPage();
        }

        public virtual IBook PeekBook()
        {
            return _desk.PeekBook();
        }

        public virtual void SetDesk(IDesk desk)
        {
            this._desk = desk;
        }
    }
}
