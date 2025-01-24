using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    public class EventScheduler : MonoBehaviour
    {
        /// <summary>
        /// the current location of which event it is on. 
        /// </summary>
        private int currentLocation = 0;
        [SerializeField, Tooltip("What are all the event sequences that exist.")]
        private NextEventToLoad[] nextEvents;
        [SerializeField, Tooltip("All the events that could run.")]
        private UnityEvent[] eventsToRun;
        [SerializeField, Tooltip("When you call Reset At Current Location play this event before starting the next one from the current location.")]
        private UnityEvent onReset;

        /// <summary>
        /// Resets the currently playing event and restarts it without a delay playing. 
        /// </summary>
        public void ResetAtCurrentLocation()
        {
            onReset.Invoke();
            StopAllCoroutines();
            NextEvent(currentLocation, false);
        }

        /// <summary>
        /// Called to start executing the current location event
        /// </summary>
        public void Run()
        {
            NextEvent(currentLocation, true);
        }

        /// <summary>
        /// Called to start executing at a specific location event. 
        /// </summary>
        /// <param name="position">The new location you want to trigger.</param>
        public void Run(int position)
        {
            NextEvent(position, true);
        }

        /// <summary>
        /// This private method starts the coroutine and tracks what the last played event is. 
        /// </summary>
        /// <param name="nextEvent">What is the ID of the event we want to execute.</param>
        /// <param name="delay">If the delayed time should execute for the event.</param>
        private void NextEvent(int nextEvent, bool delay)
        {
            currentLocation = nextEvent;
            StartCoroutine(_NextEvent(nextEvent, delay));
        }

        /// <summary>
        /// The coroutine which will wait the time delay and then execute the event afterwards.
        /// </summary>
        /// <param name="nextEvent">What event is it playing.</param>
        /// <param name="delay">if it should wait the delay.</param>
        /// <returns>The coroutine</returns>
        private IEnumerator _NextEvent(int nextEvent, bool delay)
        {
            if (delay)
                yield return new WaitForSeconds(nextEvents[nextEvent].timeDelay);
            eventsToRun[nextEvents[nextEvent].eventToSwitchTo].Invoke();
        }

        [System.Serializable]
        public struct NextEventToLoad
        {
            [Tooltip("the name of the event. this is an internal value to make it easier to read the events.")]
            public string _name;
            [Tooltip("the position in the list of events to play next.")]
            public int eventToSwitchTo;
            [Tooltip("What is the time delay before the event is fired.")]
            public float timeDelay;
        }
    }
}