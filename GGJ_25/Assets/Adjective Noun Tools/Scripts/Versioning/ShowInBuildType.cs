using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ANT
{
    public class ShowInBuildType : MonoBehaviour
    {
        [SerializeField]
        private RuntimePlatform[] types;

        void Awake()
        {
            gameObject.SetActive(types.Contains(Application.platform));
        }
    }
}