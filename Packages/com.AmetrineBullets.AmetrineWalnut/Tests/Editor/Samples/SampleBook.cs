using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Core;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using Cysharp.Threading.Tasks;

public class SampleBook : Book
{
    public SampleBook()
    {
        BookName = "sample";
    }
    
    public override async UniTask Open(IPage page = null)
    {
        // テスト用の簡単な実装
        if (page != null)
        {
            await PushPage(page);
        }
    }

    public UniTask NextBook(IBook book)
    {
        throw new System.NotImplementedException();
    }


    public override void Dispose()
    {
        // TODO release managed resources here
    }
}
