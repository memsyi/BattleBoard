using UnityEngine;
using System.Collections;

public class ShowDistanceToTaget : MonoBehaviour
{
    // Textmesh variables
    TextMesh textMesh;
    MoveToMousePosition moveToMousePosition;

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        moveToMousePosition = GetComponentInParent<MoveToMousePosition>();
    }

    // Update is called once per frame
    void Update()
    {
        if(textMesh && moveToMousePosition.enabled)
        {
            int distance = (int)moveToMousePosition.GetDistanceToTarget();
            textMesh.text = distance.ToString();

            // Look to camera
            transform.LookAt(Camera.main.transform);
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
