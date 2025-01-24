using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{
    [RequireComponent(typeof(AudioSource)), DefaultExecutionOrder(-1)]
    public class SpawnableSFX : SFXPlayer
    {
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (IsPlaying == false && isPaused == false)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
