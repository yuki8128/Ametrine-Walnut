
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR || UNITY_STANDALONE
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace com.AmetrineBullets.AmetrineWalnut.Interface
{
    public interface IDesk
    {

        public IBook GetCurrentBook();
        public Task PushBook(IBook book, IPage page, bool isClearHistory = false);

        public Task PopBook();

        public IBook PeekBook();
        public IBook GetDefaultBook();
        public void SetDefaultBook(IBook book);
        public Task ClearDesk();

        public List<DebugPageInfo> GetDebugHistoryStack();
    }

    /// <summary>
    /// デバッグ用のページ情報を含むクラス
    /// </summary>
    public class DebugPageInfo
    {
        public string DeskInfo { get; set; }
        public string BookName { get; set; }
        public string PageName { get; set; }
    }
}
