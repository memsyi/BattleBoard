using UnityEngine;

namespace Assets.Scripts
{
    public class CameraControllerBehavior : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private bool _panByMousewheel = true;

        [SerializeField]
        private float
            _movementSpeed = 1f,
            _rotationSpeed = 1f,
            _zoomSpeed = 1f,
            _minimumZoom = 20f,
            _maximumZoom = 3f,
            _zoomDownAngle = 20f,
            _heightMultiplier = 1.3f;

        [SerializeField]
        private int _borderWidth = 5;

        public const float GeneralMultiplier = 0.3f;

        public bool PanByMousewheel
        {
            get { return _panByMousewheel; }
            private set { _panByMousewheel = value; }
        }

        public float HeightMultiplier
        {
            get
            {
                var muliplier = transform.position.y * GeneralMultiplier * _heightMultiplier;
                return muliplier;
            }
            private set { _heightMultiplier = value; }
        }

        public float ZoomDownAngle
        {
            get { return _zoomDownAngle; }
            private set { _zoomDownAngle = value; }
        }

        public float MinimumZoom
        {
            get { return _minimumZoom; }
            private set { _minimumZoom = value; }
        }

        public float MaximumZoom
        {
            get { return _maximumZoom; }
            private set { _maximumZoom = value; }
        }

        public float ZoomSpeed
        {
            get { return _zoomSpeed; }
            private set { _zoomSpeed = value; }
        }

        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            private set { _rotationSpeed = value; }
        }

        public float MovementSpeed
        {
            get { return _movementSpeed; }
            private set { _movementSpeed = value; }
        }

        public int BorderWidth
        {
            get { return _borderWidth; }
            private set { _borderWidth = value; }
        }

        public Quaternion DefaultRotation { get; private set; }

        #endregion

        #region Methods

        private void Init()
        {
            DefaultRotation = transform.rotation;
        }

        private void HandleUserInput()
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleKeyboardInput()
        {
            //Movement
            if (Input.GetButton("Vertical"))
            {
                MoveForwardOrBackward(Input.GetAxis("Vertical") * GeneralMultiplier);
            }
            if (Input.GetButton("Horizontal"))
            {
                MoveRightOrLeft(Input.GetAxis("Horizontal") * GeneralMultiplier);
            }

            // Rotation
            if (Input.GetKey(KeyCode.E))
            {
                RotateRightOrLeft(1); // Right
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                RotateRightOrLeft(-1); // Left
            }

            // Zoom
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                Zoom(1); // In
            }
            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                Zoom(-1); // Out
            }

            // Rotation reset
            if (Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Keypad0))
            {
                transform.rotation = DefaultRotation;
            }
        }

        private void HandleMouseInput()
        {
            // MouseWheel
            if (Input.GetMouseButton(2)) // Mousewheel
            {
                if (PanByMousewheel)
                {
                    // Rotation
                    RotateRightOrLeft(-Input.GetAxis("Mouse X"));
                    RotateUpOrDown(Input.GetAxis("Mouse Y"));
                }
                else
                {
                    // Movement
                    MoveForwardOrBackward(-Input.GetAxis("Mouse Y"));
                    MoveRightOrLeft(-Input.GetAxis("Mouse X"));
                }
            }

            // Zoom
            Zoom(Input.GetAxis("Mouse ScrollWheel"));

            // Border Crossing
            if (PanByMousewheel)
            {
                if (Input.mousePosition.x < BorderWidth)
                {
                    MoveRightOrLeft(GeneralMultiplier); // Right
                }
                else if (Input.mousePosition.x > Screen.width - BorderWidth)
                {
                    MoveRightOrLeft(-GeneralMultiplier); // Left
                }
                else if (Input.mousePosition.y < BorderWidth)
                {
                    MoveForwardOrBackward(GeneralMultiplier); // Forward
                }
                else if (Input.mousePosition.y > Screen.height - BorderWidth)
                {
                    MoveForwardOrBackward(-GeneralMultiplier); // Back
                }
            }
            else
            {
                if (Input.mousePosition.x < BorderWidth)
                {
                    RotateRightOrLeft(-1); // Left
                }
                else if (Input.mousePosition.x > Screen.width - BorderWidth)
                {
                    RotateRightOrLeft(1); // Right
                }
                else if (Input.mousePosition.y < BorderWidth)
                {
                    RotateUpOrDown(-1); // Down
                }
                else if (Input.mousePosition.y > Screen.height - BorderWidth)
                {
                    RotateUpOrDown(1); // Up
                }
            }
        }

        /// <summary>
        /// <para> positive value for forward, negative value for backward </para>
        /// </summary>
        private void MoveForwardOrBackward(float movement)
        {
            /* Since we don't want the camera height to change when moving, we have to
             * save the starting height and reset it after moving.
             */
            var startingHeight = transform.position.y;

            transform.position += transform.forward * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;

            transform.position = new Vector3(transform.position.x, startingHeight, transform.position.z);
        }

        /// <summary>
        /// <para> positive value for right, negative value for left </para>
        /// </summary>
        private void MoveRightOrLeft(float movement)
        {
            transform.position += transform.right * MovementSpeed * GeneralMultiplier * HeightMultiplier * movement;
        }

        /// <summary>
        /// <para> positive value for right, negative value for left </para>
        /// </summary>
        private void RotateRightOrLeft(float rotation)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + RotationSpeed * rotation,
                0);
        }

        /// <summary>
        /// <para> positive value for up, negative value for down </para>
        /// </summary>
        private void RotateUpOrDown(float rotation)
        {
            float rotationX;
            if (rotation > 0)
            {
                rotationX = transform.rotation.eulerAngles.x >= 5 ? transform.rotation.eulerAngles.x - RotationSpeed * rotation : 4.9f;
            }
            else
            {
                rotationX = transform.rotation.eulerAngles.x <= 85 ? transform.rotation.eulerAngles.x - RotationSpeed * rotation : 85.1f;
            }

            transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, 0);
        }

        /// <summary>
        /// <para> positive value for in, negative value for out </para>
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

        #region MonoBehaviour Implementation
        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            HandleUserInput();
        }

        #endregion
    }
}
