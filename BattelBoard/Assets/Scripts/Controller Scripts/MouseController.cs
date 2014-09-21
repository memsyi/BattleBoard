using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class MouseController : Singleton<MouseController>
    {
        public event EventHandler MousePositionChanged;

        public Unit SelectedUnit { get; private set; }

        public Vector3 CurrentMousePosition { get { return transform.position; } }

        public ParticleSystem ParticleSystem { get { return GetComponent<ParticleSystem>(); } }

        private void Init()
        {
        }

        private void HandleMouseClick()
        {
            if (Input.GetMouseButtonDown(1)) // Right MouseButton
            {
                DeselectAllUnits();
            }
            if (Input.GetMouseButtonDown(0) && !GameController.Instance.IsGuiSelected) // Left MouseButton
            {
                HandleMouseCollision();
            }
        }

        private void HandleMouseCollision()
        {
            RaycastHit hitInfo;
            if(!IsMouseColliding(out hitInfo))
            {
                return;
            }

            var isRaycastCollidingWithGround = hitInfo.transform.tag == Tags.Ground;
            if (isRaycastCollidingWithGround)
            {
                SetMousePosition(hitInfo.point);
                ParticleSystem.Play();
            }

            var isRaycastCollidingWithUnit = hitInfo.transform.tag == Tags.Unit; 
            if (isRaycastCollidingWithUnit)
            {
                var unit = hitInfo.transform.GetComponent<Unit>();
                if (unit.IsActive)
                {
                    SelectUnit(unit);
                }
            }
        }

        private bool IsMouseColliding(out RaycastHit hitInfo)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var isRaycastColliding = Physics.Raycast(ray, out hitInfo);
            if (!isRaycastColliding)
            {
                return false;
            }

            return true;
        }

        private Vector3 GetMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            var isRaycastColliding = GameObject.FindGameObjectWithTag(Tags.Ground).collider.Raycast(ray, out hitInfo, 100f);

            return hitInfo.point;
        }

        private void SelectUnit(Unit unit)
        {
            DeselectAllUnits();
            SelectedUnit = unit;
            SelectedUnit.IsSelected = true;
        }

        private void DeselectAllUnits()
        {
            GameController.Instance.UnitsOnBattleField.ForEach(x => x.IsSelected = false);
        }

        private void SetMousePosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            InvokeMouseClickPositionChanged(new EventArgs());
        }

        private void InvokeMouseClickPositionChanged(EventArgs e)
        {
            if (MousePositionChanged != null)
                MousePositionChanged(this, e);
        }

        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseClick();
        }
    }
}