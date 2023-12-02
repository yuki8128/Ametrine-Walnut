using NUnit.Framework;
using com.AmetrineBullets.AmetrineWalnut.Core;
using UnityEngine;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using com.AmetrineBullets.AmetrineWalnut.Unity;

namespace TestScripts
{
    public class Example
    {
        // Testアトリビュートを付ける
        [Test]
        public void ExampleTest()
        {
            // 条件式がtrueだったら成功
            Assert.That(1 < 10);
        }


        [Test]
        public void DeskHistory()
        {
            var desk = new Desk();
            IBook sampleBook = new SampleBook();
            desk.PushBook(new SampleBook(), SampleTopPage.CreateInstanceDelegate(null));

            Debug.Log($"{desk.GetCurrentBook().GetType()}   {sampleBook.GetType()}");
            // 条件式がtrueだったら成功
            Assert.That(desk.GetCurrentBook().GetType(), Is.EqualTo(sampleBook.GetType()));
        }

        [Test]
        public void PushPage()
        {
            var desk = new Desk();
            IBook sampleBook = new SampleBook();
            desk.PushBook(new SampleBook(), SampleTopPage.CreateInstanceDelegate(null));
            sampleBook.PushPage(SampleTopPage.CreateInstanceDelegate(null));

            Debug.Log($"{desk.GetCurrentBook().GetType()}   {sampleBook.GetType()}");
// 条件式がtrueだったら成功
            Assert.That(desk.GetCurrentBook().GetType(), Is.EqualTo(sampleBook.GetType()));
        }
    }
}