# Ametrine-Walnut

Unityでのシーン遷移管理ライブラリ

現在開発中でアルファ版です。
画面に出すものの表示非表示と履歴管理、表示を切り替える際の演出の設定を行うライブラリです。

将来的にUnity依存部分を切り離すことで別プラットフォームでの利用も視野に入れています。

Desk、Book、Pageの概念があります。
Pageが最小単位で実際に画面に出るものを表し、デフォルトではPrefabに相当します。
BookはPageを複数まとめたもので、デフォルトではUnityではSceneに相当します。
DeskはBookをまとめたもので最大の単位です。

それぞれの概念は下位の概念の履歴の管理や、表示・非表示などを行えます。

## インストール方法

Unity Package Managerの「Add package from git URL」から以下のURLを入力してください。

`https://github.com/yuki8128/Ametrine-Walnut.git?path=Packages/com.AmetrineBullets.AmetrineWalnut`

## 使用方法

基本的な使用例を以下に示します。

```csharp
using AmetrineBullets.AmetrineWalnut.Core;
using AmetrineBullets.AmetrineWalnut.Interface;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用する場合

// BasicBookを継承したカスタムBookクラスの例
public class MyBook : BasicBook
{
    public MyBook(string bookName) : base(bookName) { }

    // Book固有の初期化処理などをオーバーライド可能
    // public override async Task Open(IPage page = null)
    // {
    //     await base.Open(page);
    //     // カスタム初期化処理
    // }
}

// BasicPageを継承したカスタムPageクラスの例
public class MyPage : BasicPage
{
    public MyPage(string pageName) : base(pageName) { }

    // Page固有の表示・非表示処理などをオーバーライド可能
    // public override async Task PageVisible()
    // {
    //     await base.PageVisible();
    //     // カスタム表示処理
    // }
}


public class SampleUsage : MonoBehaviour
{
    async void Start()
    {
        // PageManagementの初期化
        // カスタムBookとカスタムPageを使用
        var defaultBook = new MyBook("DefaultBook");
        var defaultPage = new MyPage("DefaultPage");
        await PageManagement.Instance.Init(defaultBook, defaultPage);

        // 新しいBookとPageを作成
        var book1 = new MyBook("Book1");
        var page1 = new MyPage("Page1");
        var page2 = new MyPage("Page2");

        // Book1とPage1を表示
        await PageManagement.Instance.PushBook(book1, page1);
    }

    public async void GoToPage2InBook1()
    {
        // 現在のBook (Book1) 内でPage2を表示
        var currentBook = PageManagement.Instance.PeekBook();
        var page2 = new MyPage("Page2"); // Pageは必要に応じて再利用または新規作成
        await currentBook.PushPage(page2);
    }

    public async void GoToPage1InBook2()
    {
        // Book2に切り替えてPage1を表示
        var book2 = new MyBook("Book2");
        var page1 = new MyPage("Page1"); // Pageは必要に応じて再利用または新規作成
        await PageManagement.Instance.PushBook(book2, page1);
    }

    public async void GoBack()
    {
        // 一つ前のPageに戻る
        await PageManagement.Instance.PopPage();
        // または一つ前のBookに戻る場合は PopBook()
        // await PageManagement.Instance.PopBook();
    }
}
```

## APIリファレンス

主要なクラスやインターフェースの簡単な説明と主要なメソッドを以下に示します。詳細は、別途APIリファレンスを参照してください。（ここにAPIリファレンスへのリンクを記載予定）

### IDesk / Desk

アプリケーション全体の画面管理を司るインターフェース/クラスです。複数のBookを管理します。

-   `GetCurrentBook()`: 現在表示されているBookを取得します。
-   `PushBook(IBook book, IPage page, bool isClearHistory = false)`: Bookをスタックに追加し表示します。同時に指定したPageも開きます。
-   `PopBook()`: 現在のBookをスタックから削除し、前のBookに戻ります。
-   `PeekBook()`: 現在のBookを取得します。
-   `GetDefaultBook()`: デフォルトのBookを取得します。
-   `ClearDesk()`: Deskの履歴を全てクリアします。

### IBook / Book

特定の機能やまとまりを持つ画面群（Page）を管理するインターフェース/クラスです。Unityにおいてはシーンに相当することを想定しています。

-   `GetBookName()`: Bookの名前を取得します。
-   `Open(IPage page = null)`: Bookを開きます。
-   `Close()`: Bookを閉じます。
-   `PushPage(IPage page, bool isHistoryClear = false)`: Pageをスタックに追加し表示します。
-   `PopPage()`: 現在表示されているPageをスタックから削除し非表示にします。
-   `PeekPage()`: 現在表示されているPageを取得します。
-   `GoToBackPage(string pageName)`: 指定した名前のPageまで戻ります。
-   `DefaultPage()`: デフォルトのPageを取得します。
-   `GetPages()`: 現在スタックにあるすべてのPageを取得します。
-   `GetPageHistory()`: Pageの履歴を取得します。


### IPage / Page

画面の最小単位を表すインターフェース/クラスです。UnityにおいてはPrefabやGameObjectに相当することを想定しています。

-   `IsEqualPage(PageNameEnum pageName)`: 指定したPage名と一致するか判定します。
-   `OnActivate()`: Pageがアクティブになった時に呼ばれます。
-   `PageVisible()`: Pageが表示された時に呼ばれます。
-   `PageInvisible()`: Pageが非表示になった時に呼ばれます。
-   `OnTransitionIn(UniTaskCompletionSource loadPageCompletionSource)`: Page遷移時に呼ばれます。

## Page Lifecycle

```csharp

        // PushPage時の各IPageの関数の呼ばれる順序
        await page.Init();

        await page.ObjectLoad();
        _pageHistory.Push(page);

        await page.PreEntryTransition();
        await page.EntryTransition();
        await page.PostEntryTransition();

        await page.PrePageVisible();
        await page.PageVisible();
        await page.PostPageVisible();

        await page.PreAppearanceEffect();
        await page.AppearanceEffect();
        await page.PostAppearanceEffect();

        // PopPageした時の各IPageの関数の呼ばれる順序

        await page.PreExitTransition();
        await page.ExitTransition();
        await page.PostEntryTransition();

        await page.PrePageInvisible();
        await page.PageInvisible();
        await page.PostPageInvisible();

        _pageHistory.Pop();
```

### IPageManagement

Pageの表示・非表示を管理するためのインターフェースです。

-   `Init(IBook defaultBook, IPage defaultPage, bool isForceInit = false)`: PageManagementを初期化します。
-   `PushPage(IPage page, IBook book = null, bool isClearHistory = false)`: Pageをスタックに追加し表示します。必要であればBookも切り替えます。
-   `PopPage()`: 現在表示されているPageをスタックから削除し非表示にします。
-   `PopBook()`: 現在のBookをスタックから削除し、前のBookに戻ります。
-   `PopTargetPage(IPage page)`: 指定したPageまで戻ります。
-   `PeekPage()`: 現在表示されているPageを取得します。
-   `PeekBook()`: 現在のBookを取得します。
