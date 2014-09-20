using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementArea : MonoBehaviour
    {
        public const float DefaultMovingDistance = 5;

        [SerializeField]
        private float _movingDistance = 2;
        [SerializeField]
        private int _areaSubdevisions = 0;

        [SerializeField]
        Transform _movementAreaSprite = null;

        public Transform MovementAreaSprite
        {
            get { return _movementAreaSprite; }
        }

        public int AreaSubdevisions
        {
            // Return how often one unit (step) will be devided
            // No divisions mean unit divide by 1
            get { return _areaSubdevisions + 1; }
        }

        public float MovingDistance
        {
            get { return _movingDistance; }
            set { _movingDistance = value; }
        }

        public Unit Unit { get { return GetComponentInParent<Unit>(); } }

        private List<Vector3> MovementAreaPositions { get; set; }

        private List<Transform> MovementAreaSprites { get; set; }

        private List<Vector3> MovementAreaOutline { get; set; }

        private float SpriteScale { get { return 0.5f; } }

        private void Init()
        {
            MovingDistance = _movingDistance;

            DefineLists();
        }

        private void DefineLists()
        {
            MovementAreaPositions = new List<Vector3>();
            MovementAreaOutline = new List<Vector3>();
            MovementAreaSprites = new List<Transform>();
        }

        private void HandleMovementAreaDisplay()
        {
            if (Unit.IsSelected && !Unit.IsMoving)
            {
                if (MovementAreaSprites.Count == 0)
                {
                    CalculateAreaPositions();
                    DrawMovementArea();
                }
                return;
            }

            if (MovementAreaSprites.Count > 0)
            {
                DestroyAllMovementSprites();
                return;
            }
        }

        private void CalculateAreaPositions()
        {
            MovementAreaPositions.Clear();

            for (float x = -MovingDistance * AreaSubdevisions; x <= MovingDistance * AreaSubdevisions; x++)
            {
                for (float y = -MovingDistance * AreaSubdevisions; y <= MovingDistance * AreaSubdevisions; y++)
                {
                    // Change positions to hexagon order
                    var target = new Vector3(x / AreaSubdevisions, 0, y / AreaSubdevisions + (x % 2 == 0 ? 0 : SpriteScale / AreaSubdevisions));

                    if (IsPathPossible(target + transform.position))
                    {
                        MovementAreaPositions.Add(target);
                    }
                }
            }
        }

        private void CalculateAreaOutline()
        {
            MovementAreaOutline.Clear();

            foreach (var position in MovementAreaPositions)
            {
                if (!MovementAreaPositions.Exists(p => p.z == position.z && p.x >= position.x - SpriteScale - 0.1f && p.x < position.x)
                    || !MovementAreaPositions.Exists(p => p.z == position.z && p.x <= position.x + SpriteScale + 0.1f && p.x > position.x)
                    || !MovementAreaPositions.Exists(p => p.x == position.x && p.z >= position.z - SpriteScale - 0.1f && p.z < position.z)
                    || !MovementAreaPositions.Exists(p => p.x == position.x && p.z <= position.z + SpriteScale + 0.1f && p.z > position.z))
                {
                    MovementAreaOutline.Add(position);
                }
            }
        }

        private bool IsPathPossible(Vector3 target)
        {
            if (!IsBeelineShorterMovingDistance(target))
            {
                return false;
            }

            NavMeshPath path = new NavMeshPath();
            Unit.NavMeshAgent.CalculatePath(target, path);

            if (!IsPathComplet(target, path))
            {
                return false;
            }

            if (!IsPathShorterMovingDistance(path))
            {
                return false;
            }

            return true;
        }

        private bool IsBeelineShorterMovingDistance(Vector3 target)
        {
            return Vector3.Distance(transform.position, target) < MovingDistance + 0.1f;
        }
        private bool IsPathComplet(Vector3 target, NavMeshPath path)
        {
            return path != null && path.status == NavMeshPathStatus.PathComplete
                   && path.corners[path.corners.Length - 1] == target;
        }
        private bool IsPathShorterMovingDistance(NavMeshPath path)
        {
            var pathLength = 0f;

            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            return pathLength < MovingDistance + 0.1f;
        }

        private void InstantianteSprite(Vector3 position)
        {
            MovementAreaSprites.Add(Instantiate(MovementAreaSprite, position, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform);
        }

        private void DrawMovementArea()
        {
            MovementAreaSprite.transform.localScale = new Vector3(SpriteScale + SpriteScale / 3, SpriteScale, 1); // x needs to be 30% longer then y

            DestroyAllMovementSprites();

            foreach (var position in MovementAreaPositions)
            {
                var instance = Instantiate(MovementAreaSprite, position + transform.position, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform;
                instance.parent = transform.GetChild(0);
                MovementAreaSprites.Add(instance);
            }

            ChangeColorOfMouseSprite();
        }

        private void DestroyAllMovementSprites()
        {
            MovementAreaSprites.ForEach(spriteObject => Destroy(spriteObject.gameObject));

            MovementAreaSprites.Clear();
        }

        private void ChangeColorOfMouseSprite()
        {
            if (Vector3.Distance(transform.position, MouseController.Instance.GetCurrentMousePosition()) < MovingDistance + 0.1f) // Moving distance + a small extra distance
            {
                var nearestSprites = MovementAreaSprites.FindAll(s => Vector3.Distance(s.position, MouseController.Instance.GetCurrentMousePosition()) < 0.5f);

                var nearestSprite = GetNearestMovementSprite(nearestSprites);

                if (nearestSprite)
                {
                    nearestSprite.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        private Transform GetNearestMovementSprite(List<Transform> spriteList)
        {
            var shortestDistance = 100f;
            Transform nearestSprite = null;

            foreach (var sprite in spriteList)
            {
                var distanceToMousePosition = Vector3.Distance(sprite.position, MouseController.Instance.GetCurrentMousePosition());
                if (shortestDistance > distanceToMousePosition)
                {
                    shortestDistance = distanceToMousePosition;
                    nearestSprite = sprite;
                }
            }

            return nearestSprite;
        }

        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        private void Update()
        {
            HandleMovementAreaDisplay();
        }
    }
}
