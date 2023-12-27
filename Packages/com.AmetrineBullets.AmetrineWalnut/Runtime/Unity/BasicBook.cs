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
        }

        public override async UniTask Open()
        {
            await SceneManager.LoadSceneAsync(GetBookName());

            if (_pageHistory.Count > 0)
            {
                // 履歴があれば一番上のページを表示する。
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
