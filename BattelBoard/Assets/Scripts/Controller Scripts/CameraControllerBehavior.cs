using UnityEngine;

namespace Assets.Scripts
{
    public class CameraControllerBehavior : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private bool _panByMousewheel = true;

        public bool PanByMousewheel
        {
            get { return _panByMousewheel; }
            set { _panByMousewheel = value; }
        }

        [SerializeField]
        private float
            _movementSpeed = 1f,
            _rotationSpeed = 1f,
            _zoomSpeed = 1f,

            _maximumZoom = 3f, _minimumZoom = 20f,
            _zoomDownAngle = 20f,
            _heightMultiplier = 1.3f;

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

        public float ZoomSpeed
        {
            get { return _zoomSpeed; }
            set { _zoomSpeed = value; }
        }

        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }

        public float MovementSpeed
        {
            get { return _movementSpeed; }
            set { _movementSpeed = value; }
        }

        [SerializeField]
        private int _borderWidth = 5;

        private int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        // Not serialized
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
        private void Start()
        {
            SetStartRotation();
        }

        private void SetStartRotation()
        {
            StartRotation = transform.rotation;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleKeyboardInput()
        {
            #region Movement
            if (Input.GetButton("Vertical"))
            {
                MoveForwardOrBackward(Input.GetAxis("Vertical") * GeneralMultiplier);
            }
            if (Input.GetButton("Horizontal"))
            {
                MoveRightOrLeft(Input.GetAxis("Horizontal") * GeneralMultiplier);
            } 
            #endregion

            #region Rotation
            if (Input.GetKey(KeyCode.E))
            {
                RotateRightOrLeft(1); // Right
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                RotateRightOrLeft(-1); // Left
            }
            #endregion

            #region Zoom
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                Zoom(1); // In
            }
            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                Zoom(-1); // Out
            }
            #endregion

            // Reset rotation
            if(Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Keypad0))
            {
                transform.rotation = StartRotation;
            }
        }

        private void HandleMouseInput()
        {
            #region Mousewheel Drag
            if (Input.GetMouseButton(2)) // Mousewheel
            {
                if(_panByMousewheel)
                {
                    // Rotation
                    RotateRightOrLeft(-Input.GetAxis("Mouse X"));
                    RotateUpOrDown(Input.GetAxis("Mouse Y"));

                    return;
                }

                // Movement
                MoveForwardOrBackward(-Input.GetAxis("Mouse Y"));
                MoveRightOrLeft(-Input.GetAxis("Mouse X"));

                return;
            }
            #endregion

            Zoom(Input.GetAxis("Mouse ScrollWheel"));

            #region Mouse screenposition
            if (_panByMousewheel)
            {
                #region Mouse screenposition - Movement
                if (Input.mousePosition.x < BorderWidth)
                {
                    MoveRightOrLeft(-GeneralMultiplier); // Left

                    return;
                }
                if (Input.mousePosition.x > Screen.width - BorderWidth)
                {
                    MoveRightOrLeft(GeneralMultiplier); // Right

                    return;
                }
                if (Input.mousePosition.y < BorderWidth)
                {
                    MoveForwardOrBackward(-GeneralMultiplier); // Backward

                    return;
                }
                if (Input.mousePosition.y > Screen.height - BorderWidth)
                {
                    MoveForwardOrBackward(GeneralMultiplier); // Forward

                    return;
                }
                #endregion

                return;
            }

            #region Mouse screenposition - Rotation
            if (Input.mousePosition.x < BorderWidth)
            {
                RotateRightOrLeft(-1); // Left

                return;
            }
            if (Input.mousePosition.x > Screen.width - BorderWidth)
            {
                RotateRightOrLeft(1); // Right

                return;
            }
            if (Input.mousePosition.y < BorderWidth)
            {
                RotateUpOrDown(-1); // Down

                return;
            }
            if (Input.mousePosition.y > Screen.height - BorderWidth)
            {
                RotateUpOrDown(1); // Up

                return;
            }
            #endregion 
            #endregion
        }

        #region Movement
        /// <summary>
        /// <para> positive value for forward, negativ value for backward </para>
        /// </summary>
        private void MoveForwardOrBackward(float movement)
        {
            var _yPostion = transform.position.y;

            transform.position += transform.forward * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;

            transform.position = new Vector3(transform.position.x, _yPostion, transform.position.z);
        }
        /// <summary>
        /// <para> positive value for right, negativ value for left </para>
        /// </summary>
        private void MoveRightOrLeft(float movement)
        {
            transform.position += transform.right * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;
        }
        #endregion

        #region Rotation
        /// <summary>
        /// <para> positive value for right, negativ value for left </para>
        /// </summary>
        private void RotateRightOrLeft(float rotation)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + RotationSpeed * rotation,
                0);
        }
        /// <summary>
        /// <para> positive value for up, negativ value for down </para>
        /// </summary>
        private void RotateUpOrDown(float rotation)
        {
            float _rotationX;
            if(rotation > 0)
            {
                _rotationX = transform.rotation.eulerAngles.x >= 5 ? transform.rotation.eulerAngles.x - RotationSpeed * rotation : 4.9f;
            }
            else
            {
                _rotationX = transform.rotation.eulerAngles.x <= 85 ? transform.rotation.eulerAngles.x - RotationSpeed * rotation : 85.1f;
            }

            transform.rotation = Quaternion.Euler(_rotationX, transform.rotation.eulerAngles.y, 0);
        }
        #endregion

        #region Zoom
        /// <summary>
        /// <para> positive value for in, negativ value for out </para>
        /// </summary>
        private void Zoom(float zoom)
        {
            if (transform.rotation.eulerAngles.x > ZoomDownAngle
             && ((zoom > 0 && transform.position.y > MaximumZoom)
             || (zoom < 0 && transform.position.y < MinimumZoom)))
            {
                transform.position += transform.forward * ZoomSpeed * HeightMultiplier * zoom;
            }
            else
            {
                transform.position += Vector3.down * ZoomSpeed * HeightMultiplier * zoom;
            }

            if (transform.position.y < MaximumZoom)
            {
                transform.position = new Vector3(transform.position.x, MaximumZoom, transform.position.z);
            }
            else if (transform.position.y > MinimumZoom)
            {
                transform.position = new Vector3(transform.position.x, MinimumZoom, transform.position.z);
            }
        }
        #endregion
    }
}
