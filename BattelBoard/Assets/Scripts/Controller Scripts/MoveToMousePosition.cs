using UnityEngine;
using System.Collections;

public class MoveToMousePosition : MonoBehaviour
{
    // Serializeable variables
    [SerializeField]
    Transform mousePositionTarget = null;

    // Navmesh variables
    NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.baseOffset = 0.3f;
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
            // Set destination
            navMeshAgent.SetDestination(mousePositionTarget.position);
        }
    }
}
