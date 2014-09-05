using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PathRendererBehavior : MonoBehaviour
    {
        //private SelectableUnitBehaviour _selectableUnitBehaviour;
        private MoveableUnitBehaviour _moveableUnitBehaviour;
        private LineRenderer _lineRenderer;

        //private bool IsSelected { get { return _selectableUnitBehaviour.IsSelected; } }
        private float DistaceToTarget { get { return _moveableUnitBehaviour.GetDistanceToTarget(); } }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (DistaceToTarget < 1)
            {
                SetLineRenderToNull();
                return;
            }

            SetLineRendererPositions(_moveableUnitBehaviour.GetLineRendererPositions());
        }

        private void Initialize()
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            //_selectableUnitBehaviour = gameObject.GetComponentInParent<SelectableUnitBehaviour>();
            _moveableUnitBehaviour = gameObject.GetComponentInParent<MoveableUnitBehaviour>();
        }

        private void SetLineRendererPositions(IList<Vector3> pointsOnLine)
        {
            _lineRenderer.SetVertexCount(pointsOnLine.Count);
            for (var i = 0; i < pointsOnLine.Count; i++)
            {
                _lineRenderer.SetPosition(i, pointsOnLine[i]);
            }
        }
        private void SetLineRenderToNull()
        {
            _lineRenderer.SetVertexCount(0);
        }
    }
}
