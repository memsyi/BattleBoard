using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class ShowMouseClickPositionBehavior : MonoBehaviour
    {
        [SerializeField]
        private Transform mousePositionTarget = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
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