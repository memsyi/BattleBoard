using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour
{
    // Movement variables
    MoveToMousePosition moveToMousePosition;

    void Awake()
    {
        moveToMousePosition = GetComponent<MoveToMousePosition>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (transform.collider.Raycast(ray, out hit, 100))
            {
                moveToMousePosition.Selected = true;
            }
            else if(moveToMousePosition.Selected)
            {
                moveToMousePosition.Selected = false;
            }
        }
    }
}
