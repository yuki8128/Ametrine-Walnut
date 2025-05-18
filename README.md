# Ametrine-Walnut

Unityでのシーン遷移管理ライブラリ

現在開発中でアルファ版です。

Unity依存部分を切り離すことで別プラットフォームでの利用も視野に入れています。

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

public class SampleUsage : MonoBehaviour
{
    private IDesk _desk;

    void Start()
    {
        // Deskの初期化
        _desk = new Desk();

        // Bookの作成と登録
        var book1 = new Book("Book1");
        var book2 = new Book("Book2");
        _desk.AddBook(book1);
        _desk.AddBook(book2);

        // Pageの作成と登録
        var page1 = new Page("Page1");
        var page2 = new Page("Page2");
        book1.AddPage(page1);
        book1.AddPage(page2);

        // 最初のBookとPageを表示
        _desk.ShowBook("Book1");
        _desk.ShowPage("Book1", "Page1");
    }

    public void GoToPage2InBook1()
    {
        _desk.ShowPage("Book1", "Page2");
    }

    public void GoToPage1InBook2()
    {
        _desk.ShowBook("Book2");
        _desk.ShowPage("Book2", "Page1");
    }

    public void GoBack()
    {
        _desk.Back();
    }
}
```

## APIリファレンス

主要なクラスやインターフェースの簡単な説明と主要なメソッドを以下に示します。詳細は、別途APIリファレンスを参照してください。（ここにAPIリファレンスへのリンクを記載予定）

### IDesk / Desk

アプリケーション全体の画面管理を司るインターフェース/クラスです。複数のBookを管理します。

-   `AddBook(IBook book)`: Bookを追加します。
-   `RemoveBook(string bookName)`: 指定した名前のBookを削除します。
-   `ShowBook(string bookName)`: 指定した名前のBookを表示します。
-   `HideBook(string bookName)`: 指定した名前のBookを非表示にします。
-   `ShowPage(string bookName, string pageName)`: 指定したBook内のPageを表示します。
-   `HidePage(string bookName, string pageName)`: 指定したBook内のPageを非表示にします。
-   `Back()`: 一つ前の画面に戻ります。

### IBook / Book

特定の機能やまとまりを持つ画面群（Page）を管理するインターフェース/クラスです。Unityにおいてはシーンに相当することを想定しています。

-   `AddPage(IPage page)`: Pageを追加します。
-   `RemovePage(string pageName)`: 指定した名前のPageを削除します。
-   `ShowPage(string pageName)`: 指定した名前のPageを表示します。
-   `HidePage(string pageName)`: 指定した名前のPageを非表示にします。
-   `Back()`: 一つ前のPageに戻ります。

### IPage / Page

画面の最小単位を表すインターフェース/クラスです。UnityにおいてはPrefabやGameObjectに相当することを想定しています。

-   `Show()`: Pageを表示します。
-   `Hide()`: Pageを非表示にします。

### IPageManagement

Pageの表示・非表示を管理するためのインターフェースです。

-   `ShowPage(string pageName)`: 指定した名前のPageを表示します。
-   `HidePage(string pageName)`: 指定した名前のPageを非表示にします。
