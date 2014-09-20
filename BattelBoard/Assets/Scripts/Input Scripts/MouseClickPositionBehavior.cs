using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class MouseClickPositionBehavior : MonoBehaviour
    {
        public event EventHandler MousePositionChanged;

        #region Variables

        public GameControllerBehaviour GameController { get { return FindObjectOfType<GameControllerBehaviour>(); } }

        #endregion

        #region Methods

        private void HandleMouseClickPosition()
        {
            print(GUIUtility.hotControl);
            if (Input.GetMouseButtonDown(0) && ! GameController.IsGuiSelected) // Left MouseButton
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                var isRaycastColliding = Physics.Raycast(ray, out hitInfo);
                if (!isRaycastColliding)
                {
                    return;
                }
                var colliderIsGround = hitInfo.transform.tag == Tags.Ground;
                if (colliderIsGround)
                {
                    SetTransformPosition(hitInfo.point);
                }
            }
        }

        private void SetTransformPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            InvokeMousePositionChanged(new EventArgs());
        }

        private void InvokeMousePositionChanged(EventArgs e)
        {
            if (MousePositionChanged != null)
                MousePositionChanged(this, e);
        }

        #endregion

        #region MonoBehaviour Implementation

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseClickPosition();
        }

        #endregion
    }
}