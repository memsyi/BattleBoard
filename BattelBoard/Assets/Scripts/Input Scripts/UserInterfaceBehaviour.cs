using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class UserInterfaceBehaviour : MonoBehaviour
    {

        #region Variables
        #endregion

        #region Methods

        public void OnSkipButtonClick()
        {
            print(GUIUtility.hotControl);
        }

        public void OnSkipButtonEnter()
        {
            GameControllerBehaviour.Instance.IsGuiSelected = true;
        }

        public void OnSkipButtonExit()
        {
            GameControllerBehaviour.Instance.IsGuiSelected = false;
        }
        #endregion

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