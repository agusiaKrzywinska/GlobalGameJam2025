using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Will wait a said amount of time before executing the actions passed through. This is using scaled time. 
        /// </summary>
        /// <param name="mb">The monobehaviour that will run the coroutine.</param>
        /// <param name="time">How long to wait before it executes the actions.</param>
        /// <param name="action">The code it will execute when it finishes the timer.</param>
        public static void DelayedExecute(this MonoBehaviour mb, float time, Action action)
        {
            mb.StartCoroutine(DelayedExecute_Coroutine(time, action));
        }

        /// <summary>
        /// Executes the coroutine will wait for said amount of time in scaled time and then execute the action. 
        /// </summary>
        /// <param name="time">how long to wait</param>
        /// <param name="action">The code it will execute when it finishes the timer.</param>
        /// <returns>The Coroutine</returns>
        private static IEnumerator DelayedExecute_Coroutine(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        /// <summary>
        /// Will wait a said amount of time before executing the actions passed through. This is using realtime. 
        /// </summary>
        /// <param name="mb">The monobehaviour that will run the coroutine.</param>
        /// <param name="time">How long to wait before it executes the actions.</param>
        /// <param name="action">The code it will execute when it finishes the timer.</param>
        public static void DelayedExecuteRealTime(this MonoBehaviour mb, float time, Action action)
        {
            mb.StartCoroutine(DelayedExecute_CoroutineRealTime(time, action));
        }

        /// <summary>
        /// Executes the coroutine will wait for said amount of time in realtime and then execute the action. 
        /// </summary>
        /// <param name="time">how long to wait</param>
        /// <param name="action">The code it will execute when it finishes the timer.</param>
        /// <returns>The Coroutine</returns>
        private static IEnumerator DelayedExecute_CoroutineRealTime(float time, Action action)
        {
            yield return new WaitForSecondsRealtime(time);
            action();
        }

    }
}