using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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

        private float SpriteScale { get { return 1f / AreaSubdevisions; } }

        private bool IsMovementAreaShowed { get; set; }

        private void Init()
        {
            MovingDistance = _movingDistance;

            DefineLists();
            InstantianteMovementAreaSprites((int)(Mathf.PI * MovingDistance * MovingDistance * AreaSubdevisions * AreaSubdevisions));
        }

        private void DefineLists()
        {
            MovementAreaPositions = new List<Vector3>();
            MovementAreaOutline = new List<Vector3>();
            MovementAreaSprites = new List<Transform>();
        }

        private void InstantianteMovementAreaSprites(int count)
        {
            MovementAreaSprite.transform.localScale = new Vector3(SpriteScale + SpriteScale / 3, SpriteScale, 1); // x needs to be 30% longer then y

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(MovementAreaSprite, Vector3.zero, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform;
                instance.parent = transform.GetChild(0);
                instance.gameObject.SetActive(false);
                MovementAreaSprites.Add(instance);
            }
        }

        private void HandleMovementAreaDisplay()
        {
            if (Unit.IsSelected && !Unit.IsMoving)
            {
                if (!IsMovementAreaShowed)
                {
                    HideAllMovementSprites();
                    CalculateAreaPositions();
                    ShowMovementSprites();

                    IsMovementAreaShowed = true;
                }
                return;
            }

            if (IsMovementAreaShowed)
            {
                HideAllMovementSprites();
                IsMovementAreaShowed = false;

                return;
            }
        }

        private void CalculateAreaPositions()
        {
            MovementAreaPositions.Clear();

            for (int x = (int)((-MovingDistance - 1) * AreaSubdevisions); x <= (MovingDistance + 1) * AreaSubdevisions; x++)
            {
                for (int y = (int)((-MovingDistance - 1) * AreaSubdevisions); y <= (MovingDistance + 1) * AreaSubdevisions; y++)
                {
                    // Change positions to hexagon order
                    var target = new Vector3(x * SpriteScale, 0, y * SpriteScale + (x % 2 == 0 ? 0 : SpriteScale / 2));

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
            return Vector3.Distance(transform.position, target) < MovingDistance;
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

            return pathLength < MovingDistance;
        }

        private void ShowMovementSprites()
        {
            for (int i = 0; i < MovementAreaPositions.Count; i++)
            {
                if (MovementAreaSprites.Count <= i)
                {
                    InstantianteMovementAreaSprites(1);
                }

                var sprite = MovementAreaSprites[i];
                sprite.transform.position = MovementAreaPositions[i] + transform.position;
                sprite.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                sprite.gameObject.SetActive(true);
            }
            //if(MovementAreaPositions.Count < MovementAreaSprites.Count)
            //{
            //    for(int i = MovementAreaPositions.Count; i < MovementAreaSprites.Count; i++)
            //    {
            //        MovementAreaSprites[i].gameObject.SetActive(false);
            //    }
            //}

            //ChangeColorOfMouseSprite();
        }

        private IEnumerator HideAllMovementSprites()
        {
            MovementAreaSprites.ForEach(spriteObject => spriteObject.gameObject.SetActive(false));

            return null;
        }

        private void ChangeColorOfMouseSprite()
        {
            if (Vector3.Distance(transform.position, MouseController.Instance.CurrentMousePosition) < MovingDistance + 0.1f) // Moving distance + a small extra distance
            {
                var nearestSprites = MovementAreaSprites.FindAll(s => Vector3.Distance(s.position, MouseController.Instance.CurrentMousePosition) < SpriteScale);

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
                var distanceToMousePosition = Vector3.Distance(sprite.position, MouseController.Instance.CurrentMousePosition);
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
