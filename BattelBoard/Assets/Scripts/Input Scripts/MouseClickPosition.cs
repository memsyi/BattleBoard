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
        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 1 << 8))
            {
                mousePositionTarget.position = hit.point;
            }
        }
    }
}
