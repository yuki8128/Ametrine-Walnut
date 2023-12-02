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
        private static PageManagement instance { get; set; }

        private IDesk _desk;

        private PageManagement()
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

        public async Task PushPage(IPage page, IBook book = null, bool isClearHistory = false)
        {
            if (book != null)
            {
                await _desk.PushBook(book, page, isClearHistory);
            }
            else
            {
                await _desk.PushBook(await _desk.PeekBook(),page,isClearHistory);
            }
        }

        public void PopPage()
        {
            throw new NotImplementedException();
        }

        public void PopBook()
        {
            throw new NotImplementedException();
        }

        public void PopTargetPage(IPage page)
        {
            throw new NotImplementedException();
        }
        
        public IPage PeekPage()
        {
            return _desk.PeekBook().Result.PeekPage();
        }
        
        public IBook PeekBook()
        {
            return _desk.PeekBook().Result;
        }
        
        public void SetDesk(IDesk desk)
        {
            this._desk = desk;
        }
    }
}