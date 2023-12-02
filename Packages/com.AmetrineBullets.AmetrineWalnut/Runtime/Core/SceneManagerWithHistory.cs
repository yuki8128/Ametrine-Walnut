using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using com.AmetrineBullets.AmetrineWalnut.Interface;

namespace com.AmetrineBullets.AmetrineWalnut.Core
{
    public class SceneManagerWithHistory
    {
        private static SceneManagerWithHistory instance { get; set; }

        private StackKeyedCollection<String, IPage> _pageHistory =
            new StackKeyedCollection<String, IPage>(page => page.ToString());


//private StackKeyedCollection<PageNameEnum, IPage> _pageHistory =
//    new StackKeyedCollection<PageNameEnum, IPage>(page => page.PageName);


        private UniTaskCompletionSource loadPageCompletionSource;

        public static SceneManagerWithHistory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SceneManagerWithHistory();
                }

                return instance;
            }
        }


        public async UniTask PushPage(IPage page, bool isClearHistory = false)
        {
            UniTask transitionIn = new UniTask();
            bool isRequimentInTransition = _pageHistory.Count != 0;
            if (isRequimentInTransition)
            {
                var beforePage = _pageHistory.Peek();
                await page.OnBeforeTransition();
                loadPageCompletionSource = new UniTaskCompletionSource();
                transitionIn = page.OnTransitionIn(loadPageCompletionSource);
            }

            if (isClearHistory)
            {
                _pageHistory.Clear();
            }


            await page.OnBeforePagePush();
            await page.OnPagePush();
            _pageHistory.Push(page);
            if (isRequimentInTransition)
            {
                await transitionIn;
            }

            await LoadCurrentPage();
            var transitionEnd = page.OnEndTransition();
            await page.OnEndPagePush();
            await transitionEnd;
        }

        public async UniTask PopPage(bool isSkipRestore = false)
        {
            if (_pageHistory.Count > 0)
            {
                await _pageHistory.Peek().OnBeforePagePop();
                await _pageHistory.Peek().OnPagePop();
                var lastpage = _pageHistory.Pop();
                await _pageHistory.Peek().OnEndPagePop();

                if (!isSkipRestore)
                {
                    await lastpage.OnUnskippedPagePop();
                    return;
                }

                await LoadCurrentPage();
            }
        }

        public async void PopMultiPage(int count)
        {
            //        最後の一回だけページの離脱処理、ロードを行う
            for (int i = 0; i < count - 1; i++)
            {
                await PopPage(true);
            }

            await PopPage();
        }

        public void PopTargetPage(String PageName)
        {
        }

        private async UniTask LoadCurrentPage()
        {
            if (_pageHistory.Count > 0)
            {
                var currentPage = _pageHistory.Peek();

                Debug.Log(
                    $"@@@@check scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name} {currentPage.SceneName}");
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != currentPage.SceneName)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(currentPage.SceneName, LoadSceneMode.Single);
                }

                Debug.Log(
                    $"before Activate {currentPage.GetType().Name}");
                await currentPage.OnActivate();
                Debug.Log($"after OnActivate");
                loadPageCompletionSource.TrySetResult();
            }
        }
    }
}