using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MouseWorldSelectionBehavior : MonoBehaviour
    {
        public bool IsDragging { get; private set; }

        public List<SelectableUnitBehaviour> UnitsOnScreenList { get; set; }

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

        private void HandleMouseSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseEnter();
            }
            else if (IsDragging)
            {
                CalculateMouseDraggingRectangle();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseExit();
            }
        }

        private void MouseEnter()
        {

        }

        private void CalculateMouseDraggingRectangle()
        {

        }

        private void MouseExit()
        {

        }
    }
}
