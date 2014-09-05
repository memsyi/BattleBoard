using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class CameraControllerBehavior : MonoBehaviour
    {
        [SerializeField]
        float
        _movementSpeed = 0.3f,
        _rotationSpeed = 1f,
        _zoomSpeed = 1f,

        _maximumZoom = 3f;

        [SerializeField]
        int _borderWidth = 25;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetButton("Horizontal"))
            {
                transform.position += transform.right * Input.GetAxis("Horizontal") * _movementSpeed;
            }
            if (Input.GetButton("Vertical"))
            {
                float _yPostion = transform.position.y;

                transform.position += transform.forward * Input.GetAxis("Vertical") * _movementSpeed;

                transform.position = new Vector3(transform.position.x, _yPostion, transform.position.z);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                RotateLeft();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                RotateRight();
            }
        }

        private void HandleMouseInput()
        {
            if (Input.mousePosition.x < _borderWidth)
            {
                RotateLeft();
            }
            else if (Input.mousePosition.x > Screen.width - _borderWidth)
            {
                RotateRight();
            }

            if (Input.mousePosition.y < _borderWidth)
            {
                RotateDown();
            }
            else if (Input.mousePosition.y > Screen.height - _borderWidth)
            {
                RotateUp();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ZoomIn();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ZoomOut();
            }
        }

        #region Rotation
        private void RotateLeft()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y - _rotationSpeed,
                0);
        }
        private void RotateRight()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + _rotationSpeed,
                0);
        }
        private void RotateUp()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x - _rotationSpeed,
                transform.rotation.eulerAngles.y,
                0);
        }
        private void RotateDown()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x + _rotationSpeed,
                transform.rotation.eulerAngles.y,
                0);
        }
        #endregion

        #region Zoom
        private void ZoomIn()
        {
            if (transform.position.y > _maximumZoom)
            {
                transform.position += transform.forward * _zoomSpeed;
            }
        }
        private void ZoomOut()
        {
            transform.position -= transform.forward * _zoomSpeed;
        }
        #endregion
    }
}
