using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoveableUnitBehaviour : MonoBehaviour
    {
        [SerializeField]
        bool _moveable = true;

        [SerializeField] 
        private Transform _mousePositionTarget = null;

        private NavMeshAgent _navMeshAgent;
        private SelectableUnitBehaviour _selectableUnitBehaviour;

        private bool IsSelected { get { return _selectableUnitBehaviour.IsSelected; } }

        public int MovingDistance { get; set; }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (_mousePositionTarget == null)
            {
                return;
            }
            if (IsSelected)
            {
                var destination = _mousePositionTarget.position;

                if (_moveable)
                {
                    SetMovementDestination(destination);
                }
            }
        }

        private void Initialize()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _selectableUnitBehaviour = GetComponent<SelectableUnitBehaviour>();

            MovingDistance = 50;
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
    }
}
