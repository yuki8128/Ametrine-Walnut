using System;
using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

public class Desk : IDesk
{
    private StackKeyedCollection<String, IBook> _bookHistory =
        new StackKeyedCollection<String, IBook>(book => book.BookName);
    
    protected IBook defaultBook;

    public IBook GetCurrentBook()
    {
        return _bookHistory.Peek();
    }

    public async Task PushBook(IBook book, IPage page, bool isClearHistory = false)
    {

        if (_bookHistory.Count > 0)
        {
            if (book.BookName != _bookHistory.Peek().BookName)
            {
                await _bookHistory.Peek().Suspend();
            }
            if (isClearHistory)
            {
                // 本当はここにDispose処理が来る
                _bookHistory.Clear();
            }
            if (!isClearHistory && book.BookName != _bookHistory.Peek().BookName)
            {
                _bookHistory.Push(book);
                await book.Open();
            }
        }

        // すでに同じページが開かれていてもPushPageする
        // 例えばポップアップからポップアップに遷移する時ポップアップの種類は同じだけど中身の値やアイテムが違う、などの場合は同じページだけど遷移したいため。
        await book.PushPage(page);
        return;
    }

    public async Task PopBook()
    {
        IBook previousBook = _bookHistory.Pop();
        await previousBook.Close();
        return;
    }
    
    public async Task<IBook> PeekBook()
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
        // 本当はここにDispose処理が来る
        _bookHistory.Clear();
        defaultBook = null;
    } 
}
