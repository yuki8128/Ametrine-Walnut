using NUnit.Framework;
using com.AmetrineBullets.AmetrineWalnut.Core;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using UnityEngine;

namespace TestScripts
{
    public class PageBookAssociationTest
    {
        [Test]
        public void Page_Should_Know_Its_Parent_Book()
        {
            // Arrange
            var desk = new Desk();
            var book = new SampleBook();
            var page = SampleTopPage.CreateInstanceDelegate(null);
            
            // Act
            desk.PushBook(book, page);
            book.PushPage(page).Wait();
            
            // Assert
            Assert.That(page.ParentBook, Is.Not.Null, "Page should have a parent book reference");
            Assert.That(page.ParentBook, Is.EqualTo(book), "Page's parent book should be the book it was pushed to");
            Debug.Log($"Success: Page knows it belongs to Book '{book.GetBookName()}'");
        }
        
        [Test]
        public void Page_Parent_Book_Should_Be_Cleared_When_Popped()
        {
            // Arrange
            var book = new SampleBook();
            var page1 = SampleTopPage.CreateInstanceDelegate(null);
            var page2 = SampleTopPage.CreateInstanceDelegate(null);
            
            // Act - Push two pages
            book.PushPage(page1).Wait();
            book.PushPage(page2).Wait();
            
            // Verify both pages have parent book
            Assert.That(page1.ParentBook, Is.EqualTo(book));
            Assert.That(page2.ParentBook, Is.EqualTo(book));
            
            // Pop the second page
            book.PopPage().Wait();
            
            // Assert
            Assert.That(page2.ParentBook, Is.Null, "Popped page should not have a parent book reference");
            Assert.That(page1.ParentBook, Is.EqualTo(book), "Remaining page should still have parent book reference");
        }
        
        [Test]
        public void All_Pages_Should_Have_Parent_Book_Cleared_When_Book_Closes()
        {
            // Arrange
            var book = new SampleBook();
            var page1 = SampleTopPage.CreateInstanceDelegate(null);
            var page2 = SampleTopPage.CreateInstanceDelegate(null);
            
            // Act
            book.PushPage(page1).Wait();
            book.PushPage(page2).Wait();
            
            // Close the book
            book.Close().Wait();
            
            // Assert
            Assert.That(page1.ParentBook, Is.Null, "Page 1 should not have parent book after book closes");
            Assert.That(page2.ParentBook, Is.Null, "Page 2 should not have parent book after book closes");
        }
    }
}