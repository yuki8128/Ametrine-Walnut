using NUnit.Framework;
using UnityEngine.TestTools;
using com.AmetrineBullets.AmetrineWalnut.Core;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using UnityEngine;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
#endif

namespace TestScripts
{
    public class PageBookAssociationTest
    {
        [UnityTest]
        public System.Collections.IEnumerator PushPage_Waits_For_Asset_Load_Before_Showing_Page()
        {
            var book = new SampleBook();
            var page = new LoadControlledPage { PageName = "LoadingPage" };
            var push = book.PushPage(page);

            Assert.That(page.ParentBook, Is.SameAs(book));
            Assert.That(book.PeekPage(), Is.Null);
            Assert.That(page.IsVisible, Is.False);

            page.AssetLoad.TrySetResult();
            yield return push.ToCoroutine();

            Assert.That(book.PeekPage(), Is.SameAs(page));
            Assert.That(page.IsVisible, Is.True);
        }

        [UnityTest]
        public System.Collections.IEnumerator Page_Should_Know_Its_Parent_Book()
        {
            // Arrange
            var desk = new Desk();
            var book = new SampleBook();
            var page = SampleTopPage.CreateInstanceDelegate(null);

            // Act
            yield return desk.PushBook(book, page).ToCoroutine();
            yield return book.PushPage(page).ToCoroutine();

            // Assert
            Assert.That(page.ParentBook, Is.Not.Null, "Page should have a parent book reference");
            Assert.That(page.ParentBook, Is.EqualTo(book), "Page's parent book should be the book it was pushed to");
            Debug.Log($"Success: Page knows it belongs to Book '{book.GetBookName()}'");
        }

        [UnityTest]
        public System.Collections.IEnumerator Page_Parent_Book_Should_Be_Cleared_When_Popped()
        {
            // Arrange
            var book = new SampleBook();
            var page1 = SampleTopPage.CreateInstanceDelegate(null);
            var page2 = SampleTopPage.CreateInstanceDelegate(null);

            // Act - Push two pages
            yield return book.PushPage(page1).ToCoroutine();
            yield return book.PushPage(page2).ToCoroutine();

            // Verify both pages have parent book
            Assert.That(page1.ParentBook, Is.EqualTo(book));
            Assert.That(page2.ParentBook, Is.EqualTo(book));

            // Pop the second page
            yield return book.PopPage().ToCoroutine();

            // Assert
            Assert.That(page2.ParentBook, Is.Null, "Popped page should not have a parent book reference");
            Assert.That(page1.ParentBook, Is.EqualTo(book), "Remaining page should still have parent book reference");
        }

        [UnityTest]
        public System.Collections.IEnumerator All_Pages_Should_Have_Parent_Book_Cleared_When_Book_Closes()
        {
            // Arrange
            var book = new SampleBook();
            var page1 = SampleTopPage.CreateInstanceDelegate(null);
            var page2 = SampleTopPage.CreateInstanceDelegate(null);

            // Act
            yield return book.PushPage(page1).ToCoroutine();
            yield return book.PushPage(page2).ToCoroutine();
            yield return book.Close().ToCoroutine();

            // Assert
            Assert.That(page1.ParentBook, Is.Null, "Page 1 should not have parent book after book closes");
            Assert.That(page2.ParentBook, Is.Null, "Page 2 should not have parent book after book closes");
        }

        private sealed class LoadControlledPage : Page<LoadControlledPage>, IPage
        {
            public UniTaskCompletionSource AssetLoad { get; } = new UniTaskCompletionSource();
            public bool IsVisible { get; private set; }

            public UniTask Init() => UniTask.CompletedTask;

            public UniTask ObjectLoad() => AssetLoad.Task;

            public override bool IsEqualPage(PageNameEnum pageName) => false;

            public override UniTask PageVisible()
            {
                IsVisible = true;
                return UniTask.CompletedTask;
            }

            public override UniTask PageInvisible() => UniTask.CompletedTask;
        }
    }
}
