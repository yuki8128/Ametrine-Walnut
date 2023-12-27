using System;
using System.Collections;
using System.Collections.Generic;
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
            return _bookHistory.Peek();
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

                    await book.Open();
                }
            }
            // 履歴がなければそのままPushしてOpenする
            else
            {
                _bookHistory.Push(book);

                await book.Open();
            }

            // すでに同じページが開かれていてもPushPageする
            // 例えばポップアップからポップアップに遷移する時ポップアップの種類は同じだけど中身の値やアイテムが違う、などの場合は同じページだけど遷移したいため。
            await book.PushPage(page);
            return;
        }

        public async Task PopBook()
        {
            IBook previousBook = _bookHistory.Pop();

            await previousBook.PreExitTransition();
            await previousBook.ExitTransition();
            await previousBook.PostExitTransition();

            await previousBook.Close();

            IBook book = _bookHistory.Peek();

            await book.PreEntryTransition();
            await book.EntryTransition();
            await book.PostEntryTransition();

            await book.Open();
            return;
        }

        public IBook PeekBook()
        {
            return _bookHistory.Peek();

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
