using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ANT
{
    public class TouchHelper
    {
        /// <summary>
        /// Will detect if there is a press on UI to avoid OnMouseDown being called through raycast elements.  
        /// </summary>
        /// <returns>if the press is over a raycast element object.</returns>
        public static bool TouchOnUI()
        {
            foreach (Touch touch in Input.touches)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}