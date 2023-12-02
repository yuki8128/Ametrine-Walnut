using System.Collections;
using System.Collections.Generic;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using Cysharp.Threading.Tasks;

public class SampleBook : Book
{
    public new string BookName { get; set;} = "sample";
    public override UniTask Open()
    {
        throw new System.NotImplementedException();
    }

    public new UniTask Close()
    {
        throw new System.NotImplementedException();
    }

    public new UniTask NextBook(IBook book)
    {
        throw new System.NotImplementedException();
    }


    public override void Dispose()
    {
        // TODO release managed resources here
    }
}
