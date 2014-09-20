using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class UserInterfaceBehaviour : MonoBehaviour
    {

        #region Variables

        public GameControllerBehaviour GameController { get { return FindObjectOfType<GameControllerBehaviour>(); } }

        #endregion

        #region Methods

        public void OnSkipButtonClick()
        {
            print(GUIUtility.hotControl);
        }

        public void OnSkipButtonEnter()
        {
            GameController.IsGuiSelected = true;
        }

        public void OnSkipButtonExit()
        {
            GameController.IsGuiSelected = false;
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