using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MouseScreenSelectionBehavior : MonoBehaviour
    {
        [SerializeField]
        GUISkin _mouseDragSkin = null;

        [SerializeField]
        private float _minimalDraggingDistance;

        public float MinimalDraggingDistance
        {
            get { return _minimalDraggingDistance; }
        }

        private Rect _mouseSelectionArea;
        public Vector2 MouseSelectionStartPosition { get; private set; }

        public List<SelectableUnitBehaviour> UnitsOnScreen { get; set; }

        public List<SelectableUnitBehaviour> CurrentlySelectedUnits
        {
            get
            {
                return UnitsOnScreen.Where(x => x.IsSelected).ToList();
            }
        }

        public SelectableUnitBehaviour DirectSelectedUnit { get; private set; }

        bool _hasSelected;

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            UnitsOnScreen = new List<SelectableUnitBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseSelection();
        }

        void OnGUI()
        {
            if (!IsDragging())
            {
                return;
            }

            GUI.Box(_mouseSelectionArea, "", _mouseDragSkin.box);
        }



        private void HandleMouseSelection()
        {
            if (Input.GetButtonDown("Fire2"))
            {
                CurrentlySelectedUnits.ForEach(x => x.IsSelected = false);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                MouseEnter();
            }
            else if (IsDragging())
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
            MouseSelectionStartPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

            _mouseSelectionArea = new Rect(MouseSelectionStartPosition.x, MouseSelectionStartPosition.y, 0, 0);

            IsUnitHitByMouse();
        }

        private void CalculateMouseDraggingRectangle()
        {
            Vector2 currentMousePosition;
            currentMousePosition.x = Input.mousePosition.x;
            currentMousePosition.y = Screen.height - Input.mousePosition.y;

            _mouseSelectionArea.width = Mathf.Abs(currentMousePosition.x - MouseSelectionStartPosition.x);

            if (currentMousePosition.x < MouseSelectionStartPosition.x)
            {
                _mouseSelectionArea.x = currentMousePosition.x;
            }
            else
            {
                _mouseSelectionArea.x = MouseSelectionStartPosition.x;
            }

            _mouseSelectionArea.height = Mathf.Abs(currentMousePosition.y - MouseSelectionStartPosition.y);

            if (currentMousePosition.y < MouseSelectionStartPosition.y)
            {
                _mouseSelectionArea.y = currentMousePosition.y;
            }
            else
            {
                _mouseSelectionArea.y = MouseSelectionStartPosition.y;
            }
        }

        private void CheckMouseDraggingSelection()
        {
            var selection = false;

            foreach (var unit in UnitsOnScreen)
            {
                if (IsUnitWithinDraggingRectangle(unit.ScreenPosition) || unit == DirectSelectedUnit)
                {
                    if (!unit.IsSelected)
                    {
                        unit.IsSelected = true;
                    }
                    selection = true;
                }
                else// if (!DirectSelectedUnit)
                {
                    if (unit.IsSelected)
                    {
                        unit.IsSelected = false;
                    }
                }
            }

            _hasSelected = selection;
        }

        private bool IsUnitWithinDraggingRectangle(Vector2 screenPosition)
        {
            return screenPosition.x >= _mouseSelectionArea.x && screenPosition.x < _mouseSelectionArea.x + _mouseSelectionArea.width
                && screenPosition.y > Screen.height - _mouseSelectionArea.y - _mouseSelectionArea.height && screenPosition.y < Screen.height - _mouseSelectionArea.y;
        }

        private Void IsUnitHitByMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            var distance = 100f;

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
        }

        private void MouseExit()
        {
            if (!_hasSelected)
            {
                return;
            }

            StartCoroutine(StopDragging());
        }

        IEnumerator StopDragging()
        {
            yield return new WaitForEndOfFrame();

        }

        public bool IsDragging()
        {
            var currentPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            return Vector2.Distance(currentPosition, MouseSelectionStartPosition) >= MinimalDraggingDistance
                && Input.GetButton("Fire1");
        }
    }
}
