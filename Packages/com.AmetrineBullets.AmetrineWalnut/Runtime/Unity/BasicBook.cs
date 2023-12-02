using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.AmetrineBullets.AmetrineWalnut.Unity
{
    public class BasicBook : Book
    {
        public BasicBook(IPage defaultPage)
        {
            base.defaultPage = defaultPage;
        }

        public override UniTask Open()
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}