using UnityEngine;
using System.Collections;

public class MouseClickPosition : MonoBehaviour
{
    [SerializeField]
    Transform mousePositionTarget = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(transform.collider.Raycast(ray, out hit, 100))
            {
                mousePositionTarget.position = hit.point;
            }

            //if(Physics.Raycast(ray, out hit, 1 << 8))
            //{
            //    mousePositionTarget.position = hit.point;
            //}
        }
    }
}
