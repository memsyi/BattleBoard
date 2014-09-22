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

            SetLineRenderToNull();
            DisableLineRenderer();
        }

        private void HandleLineRenderer()
        {
            if (!Unit.IsSelected)
            {
                if (PathInsideMovementArea.enabled == false)
                {
                    return;
                }
                if (!Unit.IsMoving)
                {
                    SetLineRenderToNull();
                    DisableLineRenderer();
                }
                return;
            }

            if (!Unit.IsMoving)
            {
                PathInsideMovementArea.enabled = true;
                PathOutOfMovementArea.enabled = false;

                var pathLength = 0f;
                var destination = MouseController.Instance.CurrentMousePosition;
                NavMeshPath path = Unit.GetPathToDestination(destination);

                var isPathOutOfMovementArea = Unit.IsPathOutOfMovementArea(path, out destination, out pathLength);

                SetLineRendererPositions(PathInsideMovementArea, Unit.GetLineRendererPositions(Unit.GetPathToDestination(destination)));

                if (!isPathOutOfMovementArea)
                {
                    return;
                }

                PathOutOfMovementArea.enabled = true;

                var outOfAreaCorners = new List<Vector3>(path.corners);
                outOfAreaCorners[0] = destination;
                var checkPathLength = 0f;

                for (int i = 1; i < path.corners.Length; i++)
                {
                    checkPathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                    if (checkPathLength < pathLength)
                    {
                        outOfAreaCorners.Remove(path.corners[i]);
                        //return;
                    }
                }
                SetLineRendererPositions(PathOutOfMovementArea, outOfAreaCorners.ToArray());

                return;
            }
            if (DistanceToTarget > 0.5f)
            {
                PathInsideMovementArea.enabled = true;

                SetLineRendererPositions(PathInsideMovementArea, Unit.GetLineRendererPositions(Unit.NavMeshAgent.path));

                return;
            }
        }

        private void SetLineRendererPositions(LineRenderer lineRenderer, IList<Vector3> pointsOnLine)
        {
            if (!lineRenderer)
            {
                return;
            }

            lineRenderer.SetVertexCount(pointsOnLine.Count);
            for (var i = 0; i < pointsOnLine.Count; i++)
            {
                lineRenderer.SetPosition(i, pointsOnLine[i]);
            }
        }

        private void SetLineRenderToNull()
        {
            PathInsideMovementArea.SetVertexCount(0);
            PathOutOfMovementArea.SetVertexCount(0);
        }

        private void DisableLineRenderer()
        {
            PathInsideMovementArea.enabled = false;
            PathOutOfMovementArea.enabled = false;
        }

        private void EnableLineRenderer()
        {
            PathInsideMovementArea.enabled = true;
            PathOutOfMovementArea.enabled = true;
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
