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
            _bookHistory.Peek().Suspend();
        }
        _bookHistory.Push(book);
        await book.Open();
        await book.PushPage(page);
        return;
    }

    public async Task PopBook()
    {
        IBook previousBook = _bookHistory.Pop();
        previousBook.Close();
        return;
    }
    
    public async Task<IBook> PeekBook()
    {
        return _bookHistory.Peek();
        
    }
    
    public async Task<IBook> DefaultBook()
    {
        return defaultBook;
    }
    
    public void SetDefaultBook(IBook book)
    {
        this.defaultBook = book;
    }
}
