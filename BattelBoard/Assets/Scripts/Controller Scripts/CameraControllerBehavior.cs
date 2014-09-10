using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class CameraControllerBehavior : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        float
            _movementSpeed = 1f,
            _rotationSpeed = 1f,
            _zoomSpeed = 1f,

            _maximumZoom = 3f, _minimumZoom = 20f,
            _zoomDownAngle = 20f,
            _heightMultiplier = 4;

        private float HeightMultiplier
        {
            get
            {
                var _muliplier = transform.position.y * GeneralMultiplier * _heightMultiplier;

                return _muliplier;
            }
            set { _heightMultiplier = value; }
        }

        private float ZoomDownAngle
        {
            get { return _zoomDownAngle; }
            set { _zoomDownAngle = value; }
        }

        private float MinimumZoom
        {
            get { return _minimumZoom; }
            set { _minimumZoom = value; }
        }

        private float MaximumZoom
        {
            get { return _maximumZoom; }
            set { _maximumZoom = value; }
        }

        private float ZoomSpeed
        {
            get { return _zoomSpeed; }
            set { _zoomSpeed = value; }
        }

        private float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        private float MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

        [SerializeField]
        int _borderWidth = 25;

        private int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        private float GeneralMultiplier
        {
            get { return 0.3f; }
        }

        private Quaternion StartRotation
        {
            get;
            set;
        } 
        #endregion

        // Use this for initialization
        void Start()
        {
            StartRotation = transform.rotation;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleKeyboardInput()
        {
            #region Movement
            if (Input.GetButton("Vertical"))
            {
                MoveForwardBackward(Input.GetAxis("Vertical") * GeneralMultiplier);
            }
            if (Input.GetButton("Horizontal"))
            {
                MoveRightLeft(Input.GetAxis("Horizontal") * GeneralMultiplier);
            } 
            #endregion

            #region Rotation
            if (Input.GetKey(KeyCode.Q))
            {
                RotateLeft();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                RotateRight();
            } 
            #endregion

            #region Zoom
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                ZoomIn();
            }
            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                ZoomOut();
            }
            #endregion

            if(Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Keypad0))
            {
                transform.rotation = StartRotation;
            }
        }

        private void HandleMouseInput()
        {
            #region Mousewheel - Movement and Zoom
            if (Input.GetMouseButton(2)) // Mousewheel
            {
                MoveForwardBackward(-Input.GetAxis("Mouse Y"));
                MoveRightLeft(-Input.GetAxis("Mouse X"));

                return;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ZoomIn();

                return;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ZoomOut();

                return;
            }
            #endregion

            #region Mouse screenposition - Rotation
            if (Input.mousePosition.x < BorderWidth)
            {
                RotateLeft();

                return;
            }
            if (Input.mousePosition.x > Screen.width - BorderWidth)
            {
                RotateRight();

                return;
            }
            if (Input.mousePosition.y < BorderWidth)
            {
                RotateDown();

                return;
            }
            if (Input.mousePosition.y > Screen.height - BorderWidth)
            {
                RotateUp();

                return;
            } 
            #endregion
        }

        #region Movement
        private void MoveForwardBackward(float movement)
        {
            var _yPostion = transform.position.y;

            transform.position += Vector3.forward * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;

            transform.position = new Vector3(transform.position.x, _yPostion, transform.position.z);
        }
        private void MoveRightLeft(float movement)
        {
            transform.position += Vector3.right * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;
        }
        #endregion

        #region Rotation
        private void RotateLeft()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y - RotationSpeed,
                0);
        }
        private void RotateRight()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + RotationSpeed,
                0);
        }
        private void RotateUp()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x >= 5 ? transform.rotation.eulerAngles.x - RotationSpeed : 4.9f,
                transform.rotation.eulerAngles.y,
                0);
        }
        private void RotateDown()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x <= 85 ? transform.rotation.eulerAngles.x + RotationSpeed : 85.1f,
                transform.rotation.eulerAngles.y,
                0);
        }
        #endregion

        #region Zoom
        private void ZoomIn()
        {

            if (transform.rotation.eulerAngles.x > ZoomDownAngle
             && transform.position.y > MaximumZoom)
            {
                transform.position += transform.forward * ZoomSpeed * HeightMultiplier;
            }
            else
            {
                transform.position += Vector3.down * ZoomSpeed * HeightMultiplier;
            }

            if (transform.position.y < MaximumZoom)
            {
                transform.position = new Vector3(transform.position.x, MaximumZoom, transform.position.z);
            }
        }
        private void ZoomOut()
        {

            if (transform.rotation.eulerAngles.x > ZoomDownAngle
             && transform.position.y < MinimumZoom)
            {
                transform.position -= transform.forward * ZoomSpeed * HeightMultiplier;
            }
            else
            {
                transform.position += Vector3.up * ZoomSpeed * HeightMultiplier;
            }

            if (transform.position.y > MinimumZoom)
            {
                transform.position = new Vector3(transform.position.x, MinimumZoom, transform.position.z);
            }
        }
        #endregion
    }
}
