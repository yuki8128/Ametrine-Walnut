#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
#endif


namespace com.AmetrineBullets.AmetrineWalnut.Interface{

    public interface IPageManagement {
        public UniTask PushPage(IPage page, IBook book = null, bool isClearHistory = false);
        public void PopPage();
        public void PopBook();
        
        public void PopTargetPage(IPage page);
        
    }
}