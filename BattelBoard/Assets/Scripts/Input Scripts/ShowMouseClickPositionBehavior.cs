using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class ShowMouseClickPositionBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform mousePositionTarget = null;

        private MouseScreenSelectionBehavior _mouseScreenSelectionBehavior;

        // Use this for initialization
        void Start()
        {
            _mouseScreenSelectionBehavior = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<MouseScreenSelectionBehavior>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonUp("Fire1") && !_mouseScreenSelectionBehavior.IsDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                float distance = 100f;

                if (transform.collider.Raycast(ray, out hitInfo, distance))
                {
                    mousePositionTarget.position = hitInfo.point;
                }
            }
        }
    }
}