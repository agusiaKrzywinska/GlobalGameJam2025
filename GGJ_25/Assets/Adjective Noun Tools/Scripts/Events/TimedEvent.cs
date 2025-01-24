using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    [AddComponentMenu("Adjective Noun Tools/Scriptable Properties/Timed Event")]
    public class TimedEvent : MonoBehaviour
    {
        [SerializeField, Tooltip("Events that run at the start of the event time")]
        private UnityEvent onTimedStart = null;
        [SerializeField, Tooltip("Events that run at the end of the event time")]
        private UnityEvent onTimedEnd = null;
        [SerializeField, Tooltip("The mininum time it waits between events")]
        private float waitTimeBetweenMin = 1f;
        [SerializeField, Tooltip("The maximum time it waits between events")]
        private float waitTimeBetweenMax = 1f;
        [SerializeField, Tooltip("The minimum time between the start and end events")]
        private float timeOfEventMin = 1f;
        [SerializeField, Tooltip("The maximum time between the start and end events")]
        private float timeOfEventMax = 1f;
        [SerializeField, Tooltip("The minimum delay for when it first calls the first instance of the start event")]
        private float startDelayMin = 1f;
        [SerializeField, Tooltip("The maximum delay for when it first calls the first instance of the start event")]
        private float startDelayMax = 1f;
        [SerializeField, Tooltip("Does this event run multiple times if triggered once?")]
        private bool isRepeating = true;
        [SerializeField, Tooltip("How many times does this event run? \n 0 means forever if the isRepeating is true")]
        private int totalRepeatTimes = 0;
        [SerializeField, Tooltip("How many times does this event can be triggered? \n 0 means forever")]
        private int totalTimesCanTrigger = 0;
        private int currentPlayedTimes;
        [SerializeField, Tooltip("Does this event get triggered when the gameobject turns on?")]
        private bool playOnEnable = true;

        private void OnEnable()
        {
            if(playOnEnable)
                StartCoroutine(Timer());
        }

        public void ActiveEvents()
        {
            StartCoroutine(Timer());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Timer()
        {
            if(currentPlayedTimes >= totalTimesCanTrigger && totalTimesCanTrigger != 0)
                yield break;
                
            currentPlayedTimes++;
            int totalTimes = 0;

            yield return new WaitForSeconds(Random.Range(startDelayMin, startDelayMax));
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(waitTimeBetweenMin, waitTimeBetweenMax));
                onTimedStart.Invoke();
                yield return new WaitForSeconds(Random.Range(timeOfEventMin, timeOfEventMax));
                onTimedEnd.Invoke();

                //updating counter for limited events
                if (totalRepeatTimes != 0)
                {
                    totalTimes++;
                    if (totalTimes > totalRepeatTimes)
                    {
                        break;
                    }
                }

                //one off timed event
                if (!isRepeating)
                    break;
            }
        }
    }
}