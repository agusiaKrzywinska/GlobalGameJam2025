using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ANT
{
    public class FontSetter : MonoBehaviour
    {
        [SerializeField, Tooltip("font to change all text objects to.")]
        private TMP_FontAsset font;
        /// <summary>
        /// Allow the user to update the font they which to change to. 
        /// </summary>
        public TMP_FontAsset Font
        {
            get => font;
            set
            {
                font = value;
                UpdateFont(font);
            }
        }

        [SerializeField, Tooltip("the root objects for what you want to change to this associated font.")]
        private Transform[] rootObjectsWithText = null;
        //all the associated text mesh pro UI elements associated with it. 
        private List<TextMeshProUGUI> textMeshGUIs = new List<TextMeshProUGUI>();

        //all the associated text mesh pro elements associated with it. 
        private List<TextMeshPro> textMeshes = new List<TextMeshPro>();

        private void Awake()
        {
            GetAllText();
        }

        private void Start()
        {
            UpdateFont(font);
        }

        /// <summary>
        /// Goes through each root object and calls add new root. 
        /// </summary>
        public void GetAllText()
        {
            foreach (Transform root in rootObjectsWithText)
            {
                AddNewRoot(root);
            }
        }

        /// <summary>
        /// Will take a transform and find all associated text mesh pro elements and add them to a list to keep track of. 
        /// </summary>
        /// <param name="root">The parent of transform off all the text mesh pro objects you want to get. </param>
        public void AddNewRoot(Transform root)
        {
            //for UI elements. 
            TextMeshProUGUI[] txtUI = root.GetComponentsInChildren<TextMeshProUGUI>(true);
            for (int i = 0; i < txtUI.Length; i++)
            {
                //check to see if it has the ignore font change associated to it. 
                if (txtUI[i].GetComponent<IgnoreFontUpdating>() != null) continue;
                //check to see if it already exists in the list.
                if (textMeshGUIs.Contains(txtUI[i])) continue;
                //add to the list. 
                textMeshGUIs.Add(txtUI[i]);
            }

            //for text mesh elements. 
            TextMeshPro[] txt = root.GetComponentsInChildren<TextMeshPro>(true);
            for (int i = 0; i < txt.Length; i++)
            {
                //check to see if it has the ignore font change associated to it. 
                if (txt[i].GetComponent<IgnoreFontUpdating>() != null) continue;
                //check to see if it already exists in the list.
                if (textMeshes.Contains(txt[i])) continue;
                //add to the list. 
                textMeshes.Add(txt[i]);
            }
        }

        /// <summary>
        /// Change all current font objects to new font. 
        /// </summary>
        /// <param name="font"></param>
        public void UpdateFont(TMP_FontAsset font)
        {
            foreach (TextMeshProUGUI text in textMeshGUIs)
            {
                text.font = font;
            }

            foreach (TextMeshPro text in textMeshes)
            {
                text.font = font;
            }
        }
    }
}