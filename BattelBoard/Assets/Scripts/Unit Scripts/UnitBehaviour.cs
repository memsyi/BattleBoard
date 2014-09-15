using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnitBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private bool _isMoveable;

        public Transform MousePositionTarget { get { return FindObjectOfType<MouseClickPositionBehavior>().transform; } }

        public MouseClickPositionBehavior MouseClickPositionBehavior
        {
            get { return FindObjectOfType<MouseClickPositionBehavior>(); }
        }

        public NavMeshAgent NavMeshAgent { get { return GetComponent<NavMeshAgent>(); } }

        public bool IsMovable
        {
            get { return _isMoveable; }
            set { _isMoveable = value; }
        }

        public int MovingDistance
        {
            get
            {
                var movementArea = transform.GetComponentInChildren<MovementAreaBehaviour>();
                return movementArea.MovingDistance;
            }
        }

        public Vector3 ScreenPosition
        {
            get
            {
                // Since the y-Axis on WorldToScreenPoint is flipped, we have to substract the value from the actual screen height
                var worldToScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                worldToScreenPoint.y = Screen.height - worldToScreenPoint.y;
                return worldToScreenPoint;
            }
        }

        public bool IsOnScreen
        {
            get
            {
                return ScreenPosition.x > 0
                       && ScreenPosition.x < Screen.width
                       && ScreenPosition.y > 0
                       && ScreenPosition.y < Screen.height;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                HandleColorChange();
            }
        }

        #endregion

        #region Methods

        private void Init()
        {
            MouseClickPositionBehavior.MousePositionChanged += OnMousePositionChanged;
        }

        public List<Vector3> GetLineRendererPositions()
        {
            var corners = NavMeshAgent.path.corners.ToList();
            return corners;
        }

        public float GetDistanceToTarget()
        {
            float pathLength = 0;
            var pathCorners = NavMeshAgent.path.corners;

            for (var i = 0; i < pathCorners.Length - 1; i++)
            {
                pathLength += Vector3.Distance(pathCorners[i + 1], pathCorners[i]);
            }

            return pathLength;
        }

        private void SetMovementDestination(Vector3 destination)
        {
            NavMeshAgent.SetDestination(destination);
        }

        private void OnMousePositionChanged(object mouseClickPosition, EventArgs e)
        {
            var destination = MousePositionTarget.position;
            var heading = destination - transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            if (distance >= MovingDistance)
            {
                heading = direction * MovingDistance;
                destination = heading + transform.position;
            }
            if (IsSelected)
            {
                SetMovementDestination(destination);
            }
        }

        private void HandleColorChange()
        {
            if (IsSelected)
            {
                renderer.material.color *= 2f;
            }
            else
            {
                renderer.material.color *= 0.5f;
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
        void Update()
        {
        }

        #endregion
    }
}
