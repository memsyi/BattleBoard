using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MouseScreenSelectionBehavior : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private GUISkin _mouseDragSkin;

        [SerializeField]
        private float _minimalDraggingDistance;

        [SerializeField]
        private bool _enableMassSelection;

        public Rect MouseSelectionArea { get; private set; }


        public float MinimalDraggingDistance { get { return _minimalDraggingDistance; } }

        public Vector2 MouseSelectionStartPosition { get; private set; }

        public UnitBehaviour DirectSelectedUnit { get; private set; }

        public List<UnitBehaviour> CurrentlySelectedUnits
        {
            get { return GameController.Instance.UnitsOnScreen.Where(x => x.IsSelected).ToList(); }
        }

        #endregion

        #region Methods

        private void Init()
        {
        }

        private void HandleMouseSelection()
        {
            if (Input.GetMouseButtonDown(1)) // Right MouseButton
            {
                CurrentlySelectedUnits.ForEach(x => x.IsSelected = false);
            }
            if (Input.GetMouseButtonDown(0)) // Left MouseButton
            {
                MouseEnter();
            }
            else if (IsMouseDragging())
            {
                if (_enableMassSelection)
                {
                    CalculateMouseSelectionArea();
                }
                HandleUnitSelection();
            }
        }

        private void MouseEnter()
        {
            if (_enableMassSelection)
            {
                MouseSelectionStartPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                MouseSelectionArea = new Rect(MouseSelectionStartPosition.x, MouseSelectionStartPosition.y, 0, 0);
            }

            HandleDirectUnitSelection();
        }

        private void CalculateMouseSelectionArea()
        {
            var currentX = Input.mousePosition.x;
            var currentY = Screen.height - Input.mousePosition.y;

            var newWidth = Mathf.Abs(currentX - MouseSelectionStartPosition.x);
            var newHeight = Mathf.Abs(currentY - MouseSelectionStartPosition.y);

            var newX = currentX < MouseSelectionStartPosition.x ? currentX : MouseSelectionStartPosition.x;
            var newY = currentY < MouseSelectionStartPosition.y ? currentY : MouseSelectionStartPosition.y;

            MouseSelectionArea = new Rect(newX, newY, newWidth, newHeight);

            
        }

        private void HandleUnitSelection()
        {
            foreach (var unit in GameController.Instance.UnitsOnScreen)
            {
                if (unit == DirectSelectedUnit || IsUnitWithinDraggingRectangle(unit))
                {
                    if (!unit.IsSelected)
                    {
                        unit.IsSelected = true;
                    }
                }
                else
                {
                    if (unit.IsSelected)
                    {
                        unit.IsSelected = false;
                    }
                }
            }
        }

        private void DrawSelectionArea()
        {
            if (IsMouseDragging())
            {
                GUI.Box(MouseSelectionArea, "", _mouseDragSkin.box);
            }
        }

        private bool IsUnitWithinDraggingRectangle(UnitBehaviour unit)
        {
            return MouseSelectionArea.Contains(unit.ScreenPosition);
        }

        private void HandleDirectUnitSelection()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                DirectSelectedUnit = hitInfo.transform.GetComponent<UnitBehaviour>();
            }
        }

        public bool IsMouseDragging()
        {
            var currentPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            var distanceToStartPosition = Vector2.Distance(currentPosition, MouseSelectionStartPosition);
            return distanceToStartPosition >= MinimalDraggingDistance
                && Input.GetMouseButton(0); // Left MouseButton
        }

        #endregion

        #region MonoBehaviour Implementation

        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseSelection();
        }

        void OnGUI()
        {
            DrawSelectionArea();
        }

        #endregion
    }
}
