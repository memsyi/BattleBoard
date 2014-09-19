using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathRendererBehavior : MonoBehaviour
    {
        #region Variables

        public UnitBehaviour Unit { get { return GetComponentInParent<UnitBehaviour>(); } }

        public LineRenderer LineRenderer { get { return GetComponent<LineRenderer>(); } }

        public float DistanceToTarget { get { return Unit.GetDistanceToTarget(); } }

        #endregion

        #region Methods

        private void Init()
        {
        }

        private void HandleLineRenderer()
        {
            if(Unit.IsSelected)
            {
                // TODO show path live while selected
                return;
            }
            if (DistanceToTarget > 0.5f)
            {
                SetLineRendererPositions(Unit.GetLineRendererPositions());
                return;
            }
            SetLineRenderToNull();
        }

        private void SetLineRendererPositions(IList<Vector3> pointsOnLine)
        {
            LineRenderer.SetVertexCount(pointsOnLine.Count);
            for (var i = 0; i < pointsOnLine.Count; i++)
            {
                LineRenderer.SetPosition(i, pointsOnLine[i]);
            }
        }
        private void SetLineRenderToNull()
        {
            LineRenderer.SetVertexCount(0);
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
            HandleLineRenderer();
        }

        #endregion
    }
}
