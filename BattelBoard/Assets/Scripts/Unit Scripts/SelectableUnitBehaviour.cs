using System.Collections;
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
                }
                else
                {
                    renderer.material.color *= 0.5f;
                }
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
            CheckIfUnitIsOnScreen();
        }

        private bool IsUnitWithinScreenSpace()
        {
            var screenPosition = ScreenPosition;

            return screenPosition.x > 0 && screenPosition.x < Screen.width
                && screenPosition.y > 0 && screenPosition.y < Screen.height;
        }

        private void CheckIfUnitIsOnScreen()
        {
            if (_mouseScreenSelectionBehavior == null)
            {
                return;
            }

            if (!IsOnScreen && IsUnitWithinScreenSpace())
            {
                _mouseScreenSelectionBehavior.UnitsOnScreen.Add(this);
                IsOnScreen = true;
            }
            else if (IsOnScreen && !IsUnitWithinScreenSpace())
            {
                _mouseScreenSelectionBehavior.UnitsOnScreen.Remove(this);
                IsOnScreen = false;
            }
        }
    }
}
