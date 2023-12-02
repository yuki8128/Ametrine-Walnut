
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Interface
{
    public interface IDesk
    {
        public Task PushBook(IBook book, IPage page, bool isClearHistory = false);

        public Task PopBook();

        public Task<IBook> PeekBook();
        public Task<IBook> DefaultBook();
    }
}