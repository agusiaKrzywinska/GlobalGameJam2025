using ANT.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    [CreateAssetMenu(fileName = "Helper", menuName = "ANT/Helper")]
    public class HelperScriptable : ScriptableObject
    {
        /// <summary>
        /// Called to open an external link. 
        /// </summary>
        /// <param name="path">the link you wish to send the player to.</param>
        public void OpenLink(string path)
        {
            Application.OpenURL(path);
        }

        /// <summary>
        /// Will close the game or if in editor stop playing. 
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Using the load manager will load a new scene. 
        /// </summary>
        /// <param name="sceneToLoad"></param>
        public void LoadScene(string sceneToLoad)
        {
            if (LoadingManager.Instance)
                LoadingManager.Instance.LoadScene(sceneToLoad);
        }

        /// <summary>
        /// Will save the game information. 
        /// </summary>
        public void SaveGame()
        {
            if (SaveManager.Instance)
                SaveManager.Instance.WriteSaveData();
        }

        /// <summary>
        /// will load the game information. 
        /// </summary>
        public void LoadGame()
        {
            if (SaveManager.Instance)
                SaveManager.Instance.ReadSaveData();
        }

        #region Music
        /// <summary>
        /// will play a new track on the music player. 
        /// </summary>
        /// <param name="sound">The audio file you want to play.</param>
        public void PlayNewTrack(SoundDataHolder sound)
        {
            if (MusicPlayer.Instance)
                MusicPlayer.Instance.PlaySound(sound);
        }

        /// <summary>
        /// Will fade in the music player. 
        /// </summary>
        /// <param name="isPrimary">if it's fading the primary or secondary channel.</param>
        public void FadeInChannel(bool isPrimary)
        {
            if (MusicPlayer.Instance)
                MusicPlayer.Instance.Fade(isPrimary, true);
        }

        /// <summary>
        /// Will fade in the music player. 
        /// </summary>
        /// <param name="isPrimary">if it's fading the primary or secondary channel.</param>
        public void FadeOutChannel(bool isPrimary)
        {
            if (MusicPlayer.Instance)
                MusicPlayer.Instance.Fade(isPrimary, false);
        }

        /// <summary>
        /// Will fade to a new channel for the music. 
        /// </summary>
        /// <param name="isPrimary">if it's fading the primary or secondary channel.</param>
        public void FocusChannel(bool isPrimary)
        {
            if (MusicPlayer.Instance == null) return;

            if (isPrimary)
            {
                MusicPlayer.Instance.Fade(true, true);
                MusicPlayer.Instance.Fade(false, false);
            }
            else
            {
                MusicPlayer.Instance.Fade(true, false);
                MusicPlayer.Instance.Fade(false, true);
            }
        }

        /// <summary>
        /// Will play a new segment of the clip. 
        /// </summary>
        /// <param name="clip">The clip to go to the next segment in.</param>
        public void PlayNextMultiComponentTrack(MultiComponentClip clip)
        {
            if (MusicPlayer.Instance)
                MusicPlayer.Instance.PlayNextComponentSegment(clip);
        }
        #endregion

        #region Debug
        /// <summary>
        /// Prints a message to the logger as a log. 
        /// </summary>
        /// <param name="message">the message to send.</param>
        public void DebugLog(string message)
        {
            Logger.Log(message);
        }
        /// <summary>
        /// Prints a message to the logger as a warning. 
        /// </summary>
        /// <param name="message">the message to send.</param>
        public void DebugLogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        /// <summary>
        /// Prints a message to the logger as an error. 
        /// </summary>
        /// <param name="message">the message to send.</param>
        public void DebugLogError(string message)
        {
            Logger.LogError(message);
        }
        #endregion 
    }
}