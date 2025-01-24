using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ANT
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField, Tooltip("the gameobject parent for the UI for the load screen.")]
        private GameObject loadScreenUI;
        /// <summary>
        /// is the game currently loading. 
        /// </summary>
        private bool isLoading = false;
        public static bool IsLoading => instance.isLoading;
        [SerializeField, Tooltip("what is the buffer scene to load."), NaughtyAttributes.Scene()]
        private string bufferScene = "Buffer";
        [SerializeField, Min(0f), Tooltip("the gameobject parent for the UI for the load screen.")]
        private float loadingDelay = 0f;

        /// <summary>
        /// Will start loading the scene passed through. 
        /// </summary>
        /// <param name="scene">What scene are we loading.</param>
        /// <param name="showLoading">Should it show the loading scene.</param>
        public void LoadScene(string scene, bool showLoading = true)
        {
            if (!isLoading)
                StartCoroutine(_LoadScene(scene, showLoading));
        }

        /// <summary>
        /// the coroutine to actually load the new scene. 
        /// </summary>
        /// <param name="scene">The name of the scene to load. </param>
        /// <param name="showLoading">if it show the load screen UI.</param>
        /// <returns>A coroutine</returns>
        private IEnumerator _LoadScene(string scene, bool showLoading)
        {
            isLoading = true;
            //turn on the UI if it should. 
            if (showLoading)
                loadScreenUI.SetActive(true);

            //get the active scene to unload. 
            Scene toUnload = SceneManager.GetActiveScene();
            AsyncOperation result = null;

            //add buffer scene
            result = SceneManager.LoadSceneAsync(bufferScene, LoadSceneMode.Additive);
            while (!result.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            //remove old scene
            result = SceneManager.UnloadSceneAsync(toUnload.name);
            while (!result.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            //add new scene
            result = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!result.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            //remove buffer scene
            result = SceneManager.UnloadSceneAsync(bufferScene);
            while (!result.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            //add delay at the end of the loading time based on 
            if (showLoading && loadingDelay > 0f)
            {
                yield return new WaitForSecondsRealtime(loadingDelay);
            }

            //turn off the load screen UI in case it was turned on. 
            loadScreenUI.SetActive(false);
            isLoading = false;
        }
    }
}