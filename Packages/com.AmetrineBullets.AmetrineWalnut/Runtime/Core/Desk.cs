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

    public class Desk : IDesk
    {
        private StackKeyedCollection<String, IBook> _bookHistory =
            new StackKeyedCollection<String, IBook>(book => book.GetBookName());

        protected IBook defaultBook;

        public IBook GetCurrentBook()
        {
            if (_bookHistory.TryPeek(out IBook book))
            {
                return book;
            }
            return null;
        }

        public async Task PushBook(IBook book, IPage page, bool isClearHistory = false)
        {

            if (_bookHistory.Count > 0)
            {
                var currentBookName = _bookHistory.Peek().GetBookName();
                // 今開いてるBookと違うBookを開く時
                if (book.GetBookName() != currentBookName)
                {
                    var previousBook = _bookHistory.Peek();

                    await previousBook.PreExitTransition();
                    await previousBook.ExitTransition();
                    await previousBook.PostExitTransition();

                    await previousBook.Suspend();
                }

                if (isClearHistory)
                {
                    // TODO 本当はここにDispose処理が来る
                    // 全てのBookに対してのClose処理も来る
                    _bookHistory.Clear();
                }

                if (book.GetBookName() != currentBookName)
                {
                    _bookHistory.Push(book);

                    await book.PreEntryTransition();
                    await book.EntryTransition();
                    await book.PostEntryTransition();

                    await book.Open(page);
                }
            }
            // 履歴がなければそのままPushしてOpenする
            else
            {
                _bookHistory.Push(book);

                await book.Open(page);
            }

            // すでに同じページが開かれていてもPushPageする
            // 例えばポップアップからポップアップに遷移する時ポップアップの種類は同じだけど中身の値やアイテムが違う、などの場合は同じページだけど遷移したいため。
            // await book.PushPage(page);
            return;
        }

        public async Task PopBook()
        {
            if (!_bookHistory.TryPop(out IBook previousBook))
            {
                return;
            }

            await previousBook.PreExitTransition();
            await previousBook.ExitTransition();
            await previousBook.PostExitTransition();

            await previousBook.Close();

            if (_bookHistory.TryPeek(out IBook book))
            {
                await book.PreEntryTransition();
                await book.EntryTransition();
                await book.PostEntryTransition();

                await book.Open();
            }
            return;
        }

        public IBook PeekBook()
        {
            if (_bookHistory.TryPeek(out IBook book))
            {
                return book;
            }
            return null;
        }

        /// <summary>
        /// デバッグ用にBookHistoryと各BookのPageHistoryの情報を取得します。
        /// 各ページがどのBookに属しているか、そしてそれがどのDeskに関連するかを示すリストを返します。
        /// </summary>
        /// <returns>デバッグ用のページ情報を含むDebugPageInfoオブジェクトのリスト</returns>
        public List<DebugPageInfo> GetDebugHistoryStack()
        {
            List<DebugPageInfo> debugList = new List<DebugPageInfo>();

            // BookHistoryはスタックなので、現在のBookから順に処理するのが自然かもしれません。
            // ここではBookHistory全体の情報をリスト化するため、foreachで回します。
            foreach (var bookEntry in _bookHistory)
            {
                IBook book = bookEntry;
                IEnumerable<IPage> pageHistory = book.GetPageHistory();

                foreach (var page in pageHistory)
                {
                    DebugPageInfo pageInfo = new DebugPageInfo
                    {
                        // Deskの情報として、ここではシンプルにクラス名を使用します。
                        // 必要に応じて、Deskの特定のプロパティ（例: Deskの名前など）を使用できます。
                        DeskInfo = this.GetType().Name,
                        BookName = book.GetBookName(),
                        PageName = page.PageName
                    };
                    debugList.Add(pageInfo);
                }
            }

            return debugList;
        }

        public IBook GetDefaultBook()
        {
            return defaultBook;
        }

        public void SetDefaultBook(IBook book)
        {
            this.defaultBook = book;
        }

        public async Task ClearDesk()
        {
            // TODO 本当はここにDispose処理が来る
            _bookHistory.Clear();
            defaultBook = null;
        }
    }
}
