using UnityEngine;

namespace Assets.Scripts
{
    public class ShowDistanceToTaget : MonoBehaviour
    {
        // Textmesh variables
        TextMesh textMesh;
        MoveableUnitBehaviour moveToMousePosition;

        void Start()
        {
            textMesh = GetComponent<TextMesh>();
            moveToMousePosition = GetComponentInParent<MoveableUnitBehaviour>();
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
}
