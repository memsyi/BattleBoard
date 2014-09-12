using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoveableUnitBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool _isMoveable = true;

        [SerializeField]
        private Transform _mousePositionTarget = null;

        private NavMeshAgent _navMeshAgent;
        private SelectableUnitBehaviour _selectableUnitBehaviour;
        private ShowMouseClickPositionBehavior _showMouseClickPositionBehavior;

        public bool IsSelected { get { return _selectableUnitBehaviour.IsSelected; } }

        public bool IsMovable
        {
            get
            {
                return _isMoveable;
            }
            set
            {
                _isMoveable = value;
            }
        }

        public int MovingDistance
        {
            get
            {
                var movementArea = transform.GetComponentInChildren<MovementAreaBehaviour>();
                return movementArea.MovingDistance;
            }
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Initialize()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _selectableUnitBehaviour = GetComponent<SelectableUnitBehaviour>();
            _showMouseClickPositionBehavior = GameObject.FindGameObjectWithTag(Tags.Ground).GetComponent<ShowMouseClickPositionBehavior>();

            _showMouseClickPositionBehavior.MousePositionChanged += OnMousePositionChanged;
        }

        public IList<Vector3> GetLineRendererPositions()
        {
            var corners = _navMeshAgent.path.corners;
            return corners;
        }

        public float GetDistanceToTarget()
        {
            float pathLength = 0;
            var pathCorners = _navMeshAgent.path.corners;

            for (var i = 0; i < pathCorners.Length - 1; i++)
            {
                pathLength += Vector3.Distance(pathCorners[i + 1], pathCorners[i]);
            }

            return pathLength;
        }

        private void SetMovementDestination(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }

        private void OnMousePositionChanged(object mouseClickPosition, EventArgs e)
        {
            var destination = _mousePositionTarget.position;
            var heading = destination - transform.position;
            var distance = heading.magnitude;
            var direction = heading/distance;

            if (distance >= MovingDistance)
            {
                heading = direction*MovingDistance;
                destination = heading + transform.position;
            }
            if (IsSelected)
            {
                SetMovementDestination(destination);
            }
        }
    }
}
