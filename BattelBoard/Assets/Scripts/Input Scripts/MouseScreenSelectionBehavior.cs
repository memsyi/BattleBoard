using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MouseScreenSelectionBehavior : MonoBehaviour
    {
        [SerializeField]
        GUISkin _mouseDragSkin = null;

        private Rect _mouseSelectionArea;
        private Vector2 _mouseStartSelection;

        public bool IsDragging { get; private set; }

        public List<SelectableUnitBehaviour> UnitsOnScreenList { get; set; }
        public SelectableUnitBehaviour DirectSelectedUnit { get; private set; }

        bool _hasSelected;

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            UnitsOnScreenList = new List<SelectableUnitBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseSelection();
        }

        void OnGUI()
        {
            if (!IsDragging)
            {
                return;
            }

            GUI.Box(_mouseSelectionArea, "", _mouseDragSkin.box);
        }

        private void HandleMouseSelection()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                MouseEnter();
            }
            else if (IsDragging)
            {
                CalculateMouseDraggingRectangle();
                CheckMouseDraggingSelection();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                MouseExit();
            }
        }

        private void MouseEnter()
        {
            IsDragging = true;

            _mouseStartSelection.x = Input.mousePosition.x;
            _mouseStartSelection.y = Screen.height - Input.mousePosition.y;

            _mouseSelectionArea = new Rect(_mouseStartSelection.x, _mouseStartSelection.y, 0, 0);

            IsUnitHitByMouse();
        }

        private void CalculateMouseDraggingRectangle()
        {
            Vector2 currentMousePosition;
            currentMousePosition.x = Input.mousePosition.x;
            currentMousePosition.y = Screen.height - Input.mousePosition.y;

            _mouseSelectionArea.width = Mathf.Abs(currentMousePosition.x - _mouseStartSelection.x);

            if (currentMousePosition.x < _mouseStartSelection.x)
            {
                _mouseSelectionArea.x = currentMousePosition.x;
            }
            else
            {
                _mouseSelectionArea.x = _mouseStartSelection.x;
            }

            _mouseSelectionArea.height = Mathf.Abs(currentMousePosition.y - _mouseStartSelection.y);

            if (currentMousePosition.y < _mouseStartSelection.y)
            {
                _mouseSelectionArea.y = currentMousePosition.y;
            }
            else
            {
                _mouseSelectionArea.y = _mouseStartSelection.y;
            }
        }

        private void CheckMouseDraggingSelection()
        {
            var _selection = false;

            foreach (var unit in UnitsOnScreenList)
            {
                if (IsUnitWithinDraggingRectangle(unit.ScreenPosition) || unit == DirectSelectedUnit)
                {
                    if (!unit.IsSelected)
                    {
                        unit.IsSelected = true;
                    }

                    _selection = true;
                }
                else// if (!DirectSelectedUnit)
                {
                    if (unit.IsSelected)
                    {
                        unit.IsSelected = false;
                    }
                }
            }

            _hasSelected = _selection;
        }

        private bool IsUnitWithinDraggingRectangle(Vector2 screenPosition)
        {
            return screenPosition.x >= _mouseSelectionArea.x && screenPosition.x < _mouseSelectionArea.x + _mouseSelectionArea.width
                && screenPosition.y > Screen.height - _mouseSelectionArea.y - _mouseSelectionArea.height && screenPosition.y < Screen.height - _mouseSelectionArea.y;
        }

        private bool IsUnitHitByMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            float distance = 100f;

            if (Physics.Raycast(ray, out hitInfo, distance))
            {
                DirectSelectedUnit = hitInfo.transform.GetComponent<SelectableUnitBehaviour>();

                //if (DirectSelectedUnit)
                //{
                //    if (!DirectSelectedUnit.IsSelected)
                //    {
                //        DirectSelectedUnit.IsSelected = true;
                //    }
                //    else
                //    {
                //        DirectSelectedUnit.IsSelected = false;
                //    }
                //}
            }

            return false;
        }

        private void MouseExit()
        {
            if (!_hasSelected)
            {
                IsDragging = false;
                return;
            }

            StartCoroutine(StopDragging());
        }

        IEnumerator StopDragging()
        {
            yield return new WaitForEndOfFrame();

            IsDragging = false;
        }
    }
}
