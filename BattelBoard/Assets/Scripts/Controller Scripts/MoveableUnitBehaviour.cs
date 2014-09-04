using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoveableUnitBehaviour : MonoBehaviour
    {
        [SerializeField] 
        Transform _mousePositionTarget = null;

        private NavMeshAgent _navMeshAgent;
        private SelectableObjectBehaviour _selectableObjectBehaviour;
        private LineRenderer _lineRenderer;

        public bool IsSelected { get { return _selectableObjectBehaviour.IsSelected; } }

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
            if (_selectableObjectBehaviour.IsSelected)
            {
                var destination = _mousePositionTarget.position;
                SetMovementDestination(destination);

            }

            var corners = _navMeshAgent.path.corners;
            SetLineRendererPositions(corners);
        }

        private void Initialize()
        {
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _selectableObjectBehaviour = gameObject.GetComponent<SelectableObjectBehaviour>();

            MovingDistance = 50;
        }

        private void SetLineRendererPositions(IList<Vector3> pointsOnLine)
        {
            _lineRenderer.SetVertexCount(pointsOnLine.Count);
            for (var i = 0; i < pointsOnLine.Count; i++)
            {
                _lineRenderer.SetPosition(i, pointsOnLine[i]);
            }
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
