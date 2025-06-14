using System;
using com.AmetrineBullets.AmetrineWalnut.Core;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using UnityEngine;
#if UNITY_EDITOR || UNITY_STANDALONE
using Cysharp.Threading.Tasks;
using Task = Cysharp.Threading.Tasks.UniTask;
#endif

namespace Examples
{
    /// <summary>
    /// Example demonstrating how Pages can now access their parent Book
    /// </summary>
    public class PageBookAssociationExample
    {
        public static void DemoPageBookAssociation()
        {
            // Create a book
            IBook myBook = new SampleBook();

            // Create a page
            IPage myPage = SampleTopPage.CreateInstanceDelegate(null);

            // When the page is pushed to the book, it automatically knows its parent
            myBook.PushPage(myPage).GetAwaiter();

            // Now the page can access its parent book
            if (myPage.ParentBook != null)
            {
                Debug.Log($"Page '{myPage.PageName}' belongs to Book '{myPage.ParentBook.GetBookName()}'");

                // This allows the page to interact with its book if needed
                // For example, checking if it's the current page:
                bool isCurrentPage = myPage.ParentBook.PeekPage() == myPage;
                Debug.Log($"Is current page: {isCurrentPage}");
            }
        }

        public static void DemoPageCanAccessBookFeatures()
        {
            // Example: A page that needs to know about other pages in its book
            IBook book = new SampleBook();
            IPage page1 = SampleTopPage.CreateInstanceDelegate(null);
            IPage page2 = SampleTopPage.CreateInstanceDelegate(null);

            book.PushPage(page1).GetAwaiter();
            book.PushPage(page2).GetAwaiter();

            // From page2, we can now access the book and see all pages
            if (page2.ParentBook != null)
            {
                var allPagesInBook = page2.ParentBook.GetPages();
                Debug.Log($"Book '{page2.ParentBook.GetBookName()}' has {allPagesInBook.Length} pages");

                foreach (var page in allPagesInBook)
                {
                    Debug.Log($"  - Page: {page.PageName}");
                }
            }
        }
    }

    /// <summary>
    /// Example of a custom page that uses the parent book reference
    /// </summary>
    public class SmartPage : Page<SmartPage>
    {
        public SmartPage() : base()
        {
            PageName = "SmartPage";
        }

        public override bool IsEqualPage(PageNameEnum pageName)
        {
            return true;
        }

        public override async Task PageVisible()
        {
            // Now we can check which book we belong to
            if (ParentBook != null)
            {
                Debug.Log($"SmartPage is being shown in Book: {ParentBook.GetBookName()}");

                // We can also check our position in the stack
                var allPages = ParentBook.GetPages();
                for (int i = 0; i < allPages.Length; i++)
                {
                    if (allPages[i] == this)
                    {
                        Debug.Log($"SmartPage is at position {i} in the page stack");
                        break;
                    }
                }
            }

            await Task.CompletedTask;
        }

        public override async Task PageInvisible()
        {
            Debug.Log("SmartPage is being hidden");
            await Task.CompletedTask;
        }

        // Example method showing how a page might use its book reference
        public void NavigateToSibling(string siblingPageName)
        {
            if (ParentBook != null)
            {
                // Check if the sibling page exists in our book
                var pages = ParentBook.GetPages();
                foreach (var page in pages)
                {
                    if (page.PageName == siblingPageName)
                    {
                        Debug.Log($"Found sibling page '{siblingPageName}' in our book!");
                        // Could trigger navigation here through PageManagement
                        return;
                    }
                }

                Debug.Log($"Sibling page '{siblingPageName}' not found in book '{ParentBook.GetBookName()}'");
            }
            else
            {
                Debug.LogWarning("Cannot navigate - page is not associated with any book");
            }
        }
    }
}
