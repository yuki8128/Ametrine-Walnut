using NUnit.Framework;
using UnityEngine.TestTools;
using com.AmetrineBullets.AmetrineWalnut.Core;
using UnityEngine;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
#endif

namespace TestScripts
{
    public class PageBookAssociationTest
    {
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
    }
}
