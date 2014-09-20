using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class GuiController : Singleton<GuiController>
    {
        public void OnSkipButtonClick()
        {
            print(GUIUtility.hotControl);
        }

        public void OnSkipButtonEnter()
        {
            GameController.Instance.IsGuiSelected = true;
        }

        public void OnSkipButtonExit()
        {
            GameController.Instance.IsGuiSelected = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}