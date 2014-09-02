using UnityEngine;
using System.Collections;

public class MoveToMousePosition : MonoBehaviour
{
    // Serializeable variables
    [SerializeField]
    Transform mousePositionTarget = null;

    // Navmesh variables
    NavMeshAgent navMeshAgent;

    // Line renderer variables
    LineRenderer lineRenderer;

    // Selcted variables
    bool selected;
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;

            if (value)
            {
                renderer.material.color *= 2f;
            }
            else
            {
                renderer.material.color *= 0.5f;
            }
        }
    }

    void Awake()
    {
        // Navmeshagent
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        // linerenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mousePositionTarget)
        {
            if (Selected)
            {
                // Set destination
                navMeshAgent.SetDestination(mousePositionTarget.position);
            }

            // update linerenderer
            SetLineRendererPositions(navMeshAgent.path.corners);
        }
    }

    void SetLineRendererPositions(Vector3[] pointsOnLine)
    {
        lineRenderer.SetVertexCount(pointsOnLine.Length);
        for (int i = 0; i < pointsOnLine.Length; i++)
        {
            lineRenderer.SetPosition(i, pointsOnLine[i]);
        }
    }

    public float GetDistanceToTarget()
    {
        float pathLenght = 0;
        var pathCorners = navMeshAgent.path.corners;

        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            pathLenght += Vector3.Distance(pathCorners[i + 1], pathCorners[i]);
        }

        return pathLenght;
    }
}
