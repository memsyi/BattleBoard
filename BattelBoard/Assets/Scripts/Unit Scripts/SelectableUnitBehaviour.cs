using UnityEngine;

namespace Assets.Scripts
{
    public class SelectableUnitBehaviour : MonoBehaviour
    {
        private MouseScreenSelectionBehavior _mouseScreenSelectionBehavior;

        public bool IsOnScreen { get; private set; }

        public Vector2 ScreenPosition { get { return Camera.main.WorldToScreenPoint(transform.position); } }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }

            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    renderer.material.color *= 2f;
                    return;
                }
                renderer.material.color *= 0.5f;
            }
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mouseScreenSelectionBehavior = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<MouseScreenSelectionBehavior>();
        }


        // Update is called once per frame
        void Update()
        {
            CheckWhetherIsOnScreen();
        }

        private bool IsUnitWithinScreenSpace()
        {
            Vector2 _screenPosition = ScreenPosition;

            return _screenPosition.x > 0 && _screenPosition.x < Screen.width
                && _screenPosition.y > 0 && _screenPosition.y < Screen.height;
        }

        private void CheckWhetherIsOnScreen()
        {
            if (_mouseScreenSelectionBehavior)
            {
                if (!IsOnScreen && IsUnitWithinScreenSpace())
                {
                    _mouseScreenSelectionBehavior.UnitsOnScreenList.Add(this);
                    IsOnScreen = true;
                }
                else if(IsOnScreen && !IsUnitWithinScreenSpace())
                {
                    _mouseScreenSelectionBehavior.UnitsOnScreenList.Remove(this);
                    IsOnScreen = false;
                }
            }
        }
    }
}
