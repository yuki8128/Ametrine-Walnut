using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using com.AmetrineBullets.AmetrineWalnut.Interface;
using com.AmetrineBullets.AmetrineWalnut.Core;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace com.AmetrineBullets.AmetrineWalnut.Unity
{
    public abstract class BasicPage : Page<BasicPage>
    {
        private GameObject pagePrefab;
        
        public override bool IsEqualPage(PageNameEnum pageName)
        {
            throw new System.NotImplementedException();
        }

        public abstract override UniTask PageVisible();

        public abstract override UniTask PageInvisible();

        protected async UniTask<GameObject> LoadAssetWithProgressUniTask(String prefabPath)
        {
            Debug.Log($"Loading");
            AsyncOperationHandle<GameObject> asyncOperationHandle = default;
            try
            {
                asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(prefabPath);

                Debug.Log($"{asyncOperationHandle}");

                while (!asyncOperationHandle.IsDone)
                {
                    Debug.Log(
                        $"{asyncOperationHandle.Status} | Loading progress: {asyncOperationHandle.PercentComplete * 100f}%");
                    await UniTask.Yield();
                }

                return asyncOperationHandle.Result;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            finally
            {
                if (asyncOperationHandle.IsValid())
                {
                    Addressables.Release(asyncOperationHandle);
                }
            }
        }

    }
}