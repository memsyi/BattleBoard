using System;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public delegate void MousePostionChangedEventHandler(object sender, EventArgs e);

    public class ShowMouseClickPositionBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform _mousePositionTarget = null;

        public Transform MousePositionTarget { get { return _mousePositionTarget; } }

        public event MousePostionChangedEventHandler MousePositionChanged;


        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            SetMouseClickPosition();
        }

        private void SetMouseClickPosition()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                var isRaycastColliding = Physics.Raycast(ray, out hitInfo);

                if (isRaycastColliding && hitInfo.transform.tag == Tags.Ground)
                {
                    _mousePositionTarget.position = hitInfo.point;
                    InvokeMousePositionChanged(new EventArgs());
                }
            }

        }

        private void InvokeMousePositionChanged(EventArgs e)
        {
            if (MousePositionChanged != null)
                MousePositionChanged(this, e);
        }
    }
}