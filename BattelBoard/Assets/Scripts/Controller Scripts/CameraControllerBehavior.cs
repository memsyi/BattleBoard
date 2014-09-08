using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class CameraControllerBehavior : MonoBehaviour
    {
        [SerializeField]
        float
            _movementSpeed = 1f,
            _rotationSpeed = 1f,
            _zoomSpeed = 1f,

            _maximumZoom = 3f, _minimumZoom = 20f,
            _zoomDownAngle = 20f;

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
                transform.position += transform.right * Input.GetAxis("Horizontal") * _movementSpeed * 0.3f * HeightMultiplier();
            }
            if (Input.GetButton("Vertical"))
            {
                float _yPostion = transform.position.y;

                transform.position += transform.forward * Input.GetAxis("Vertical") * _movementSpeed * 0.3f * HeightMultiplier();

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

        private float HeightMultiplier()
        {
            float _heightMuliplier = transform.position.y / _minimumZoom * 4;

            return _heightMuliplier;
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
                transform.rotation.eulerAngles.x >= 5 ? transform.rotation.eulerAngles.x - _rotationSpeed : 4.9f,
                transform.rotation.eulerAngles.y,
                0);
        }
        private void RotateDown()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x <= 85 ? transform.rotation.eulerAngles.x + _rotationSpeed : 85.1f,
                transform.rotation.eulerAngles.y,
                0);
        }
        #endregion

        #region Zoom
        private void ZoomIn()
        {

            if (transform.rotation.eulerAngles.x > _zoomDownAngle
             && transform.position.y > _maximumZoom)
            {
                transform.position += transform.forward * _zoomSpeed;
            }
            else
            {
                transform.position += Vector3.down * _zoomSpeed;
            }

            if (transform.position.y < _maximumZoom)
            {
                transform.position = new Vector3(transform.position.x, _maximumZoom, transform.position.z);
            }
        }
        private void ZoomOut()
        {

            if (transform.rotation.eulerAngles.x > _zoomDownAngle
             && transform.position.y < _minimumZoom)
            {
                transform.position -= transform.forward * _zoomSpeed;
            }
            else
            {
                transform.position += Vector3.up * _zoomSpeed;
            }

            if (transform.position.y > _minimumZoom)
            {
                transform.position = new Vector3(transform.position.x, _minimumZoom, transform.position.z);
            }
        }
        #endregion
    }
}
