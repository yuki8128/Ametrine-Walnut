using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.AmetrineBullets.AmetrineWalnut.Unity
{
    public abstract class BasicBook : Book
    {
        public BasicBook(IPage defaultPage)
        {
            base.defaultPage = defaultPage;
        }

        public override async UniTask Open(){
            await base.defaultPage.PageVisible();
        }

        public abstract override void Dispose();
    }
}