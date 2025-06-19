
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif


namespace com.AmetrineBullets.AmetrineWalnut.Interface
{

    public interface IPageManagement
    {
        public Task Init(IBook defaultBook, IPage defaultPage, bool isForceInit);
        public Task PushPage(IPage page, IBook book = null, bool hideVisiblePages = false, bool isClearHistory = false);
        public Task PopPage();
        public Task PopBook();

        public Task PopTargetPage(IPage page);

    }
}
