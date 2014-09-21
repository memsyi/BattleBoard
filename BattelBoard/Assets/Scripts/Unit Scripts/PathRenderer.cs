using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PathRenderer : MonoBehaviour
    {
        public Unit Unit { get { return GetComponentInParent<Unit>(); } }

        public LineRenderer PathInsideMovementArea { get; private set; }
        public LineRenderer PathOutOfMovementArea { get; private set; }

        public float DistanceToTarget { get { return Unit.GetDistanceToTarget(); } }

        private void Init()
        {
            PathInsideMovementArea = gameObject.AddComponent<LineRenderer>();
            PathInsideMovementArea.SetWidth(0.1f, 0.1f);
            PathInsideMovementArea.material = (Material)Resources.Load("Black");

            PathOutOfMovementArea = transform.GetChild(0).gameObject.AddComponent<LineRenderer>();
            PathOutOfMovementArea.SetWidth(0.1f, 0.1f);
            PathOutOfMovementArea.material = (Material)Resources.Load("Red");
        }

        private void HandleLineRenderer()
        {
            if (!Unit.IsSelected && !Unit.IsActive)
            {
                SetLineRenderToNull();
                return;
            }

            if (!Unit.IsMoving)
            {
                // TODO show path live while selected
                return;
            }
            if (DistanceToTarget > 0.5f)
            {
                SetLineRendererPositions(Unit.GetLineRendererPositions());
                return;
            }
        }

        private void SetLineRendererPositions(IList<Vector3> pointsOnLine)
        {
            PathInsideMovementArea.SetVertexCount(pointsOnLine.Count);
            for (var i = 0; i < pointsOnLine.Count; i++)
            {
                PathInsideMovementArea.SetPosition(i, pointsOnLine[i]);
            }
        }

        private void SetLineRenderToNull()
        {
            PathInsideMovementArea.SetVertexCount(0);
        }

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
    }
}
