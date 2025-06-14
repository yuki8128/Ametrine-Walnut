using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using com.AmetrineBullets.AmetrineWalnut.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.AmetrineBullets.AmetrineWalnut.Unity
{
    public abstract class BasicBook : Book
    {
        public BasicBook(IPage defaultPage, string bookName)
        {
            base.defaultPage = defaultPage;
            base.BookName = bookName;
            // デフォルトページに親Bookを設定
            if (defaultPage != null)
            {
                defaultPage.ParentBook = this;
            }
        }

        public override async UniTask Open(IPage page = null)
        {
            if (SceneManager.GetActiveScene().name != GetBookName())
            {
                await SceneManager.LoadSceneAsync(GetBookName());
            }

            // 開くページを指定していればそのページを開く 
            if (page != null)
            {
                await EasyPageView(page);
            }
            else if (_pageHistory.Count > 0)
            {
                // ページ指定なしで、履歴があれば一番上のページを表示する。
                await EasyPageView(_pageHistory.Peek());
            }
            else
            {
                // 履歴がなければデフォルトページを表示する
                await EasyPageView(defaultPage);
            }
        }

        public abstract override void Dispose();
    }
}
