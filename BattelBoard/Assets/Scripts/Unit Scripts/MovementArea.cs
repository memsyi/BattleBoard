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
            get { return 1; }//_areaSubdevisions + 1; }
        }

        public float MovingDistance
        {
            get { return _movingDistance; }
            set
            {
                _movingDistance = value;
                SpriteRenderer.transform.localScale = new Vector3(_movingDistance * 2, _movingDistance * 2, 1);
            }
        }

        public Unit Unit { get { return GetComponentInParent<Unit>(); } }

        public SpriteRenderer SpriteRenderer { get { return GetComponentInChildren<SpriteRenderer>(); } }

        public LineRenderer LineRenderer { get { return GetComponent<LineRenderer>(); } }

        private List<Vector3> MovementAreaPoints { get; set; }

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
            MovementAreaPoints = new List<Vector3>();
            MovementAreaOutline = new List<Vector3>();
            MovementAreaSprites = new List<Transform>();
        }

        private void HandleMovementAreaDisplay()
        {
            //SpriteRenderer.transform.localScale = new Vector3(MovingDistance, MovingDistance, 1);
            //SpriteRenderer.enabled = Unit.IsSelected;

            if (Unit.IsSelected)
            {
                CalculateAreaPoints();
                //CalculateAreaOutline();
                DrawMovementArea();
                return;
            }

            if (MovementAreaSprites.Count > 0)
            {
                DestroyAllMovementSprites();
                return;
            }
        }

        private void CalculateAreaPoints()
        {
            MovementAreaPoints.Clear();

            for (float x = -MovingDistance * AreaSubdevisions; x <= MovingDistance * AreaSubdevisions; x++)
            {
                for (float y = -MovingDistance * AreaSubdevisions; y <= MovingDistance * AreaSubdevisions; y++)
                {
                    // Change positions to hexagon order
                    var target = new Vector3(x / AreaSubdevisions, 0, y / AreaSubdevisions + (x % 2 == 0 ? 0 : SpriteScale));

                    if (IsPathPossible(target + transform.position))
                    {
                        MovementAreaPoints.Add(target);
                    }
                }
            }
        }

        private void CalculateAreaOutline()
        {
            MovementAreaOutline.Clear();

            foreach (var point in MovementAreaPoints)
            {
                if (!MovementAreaPoints.Exists(p => p.z == point.z && p.x >= point.x - SpriteScale - 0.1f && p.x < point.x)
                    || !MovementAreaPoints.Exists(p => p.z == point.z && p.x <= point.x + SpriteScale + 0.1f && p.x > point.x)
                    || !MovementAreaPoints.Exists(p => p.x == point.x && p.z >= point.z - SpriteScale - 0.1f && p.z < point.z)
                    || !MovementAreaPoints.Exists(p => p.x == point.x && p.z <= point.z + SpriteScale + 0.1f && p.z > point.z))
                {
                    MovementAreaOutline.Add(point);
                }
            }

            //MovementAreaOutline.GroupBy(p => p.z);//.GroupBy(p => p.z);

            MovementAreaOutline.Add(transform.position);
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
            return path != null || path.status == NavMeshPathStatus.PathComplete
                   || path.corners[path.corners.Length - 1] == target;
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

        private void ShowAreaPoints()
        {
            MovementAreaSprite.transform.localScale = new Vector3(SpriteScale, SpriteScale, 1);

            foreach (var spriteObject in MovementAreaSprites)
            {
                Destroy(spriteObject.gameObject);
            }

            MovementAreaSprites.Clear();

            foreach (var point in MovementAreaPoints)
            {
                MovementAreaSprites.Add(Instantiate(MovementAreaSprite, point, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform);
            }
        }

        private void DrawMovementArea()
        {
            // MESH
            //var meshFilter = gameObject.GetComponent<MeshFilter>();

            //meshFilter.mesh = CreateMesh();

            // OUTLINE BY LINERENDERER
            //LineRenderer.SetVertexCount(MovementAreaOutline.Count);
            //for (var i = 0; i < MovementAreaOutline.Count; i++)
            //{
            //    LineRenderer.SetPosition(i, MovementAreaOutline[i]);
            //}

            // SPRITES
            MovementAreaSprite.transform.localScale = new Vector3(2 * SpriteScale + 2 * SpriteScale / 3, 2 * SpriteScale, 1); // x needs to be 30% longer then y

            DestroyAllMovementSprites();

            foreach (var point in MovementAreaPoints)
            {
                var instance = Instantiate(MovementAreaSprite, point + transform.position, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform;
                instance.parent = transform.GetChild(0);
                MovementAreaSprites.Add(instance);
            }

            ChangeColorOfMouseSprite();
        }

        private void DestroyAllMovementSprites()
        {
            foreach (var spriteObject in MovementAreaSprites)
            {
                Destroy(spriteObject.gameObject);
            }

            MovementAreaSprites.Clear();
        }

        private void ChangeColorOfMouseSprite()
        {
            //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hitInfo;

            //    var isRaycastColliding = Physics.Raycast(ray, out hitInfo);
            //    if (!isRaycastColliding)
            //    {
            //        return;
            //    }
            //    var colliderIsGround = hitInfo.transform.tag == Tags.MovementArea;
            //    if (colliderIsGround)
            //    {
            //        hitInfo.transform.GetComponent<SpriteRenderer>().color = Color.red;
            //    }
            //}
            if (Vector3.Distance(transform.position, GetMousePosition()) < MovingDistance + 0.1f) // Moving distance + a small extra distance
            {
                var nearestSprites = MovementAreaSprites.FindAll(s => Vector3.Distance(s.position, GetMousePosition()) < 0.5f);

                if (nearestSprites.Count > 0)
                {
                    var shortestDistance = 100f;
                    Transform nearestSprite = null;

                    foreach (var sprite in nearestSprites)
                    {
                        var distanceToMousePosition = Vector3.Distance(sprite.position, GetMousePosition());
                        if (shortestDistance > distanceToMousePosition)
                        {
                            shortestDistance = distanceToMousePosition;
                            nearestSprite = sprite;
                        }
                    }

                    nearestSprite.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }

        private Vector3 GetMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            var isRaycastColliding = GameObject.FindGameObjectWithTag(Tags.Ground).renderer.collider.Raycast(ray, out hitInfo, 100);
            if (!isRaycastColliding)
            {
                return new Vector3(1000, 1000);
            }
            return hitInfo.point;
        }

        private Mesh CreateMesh()
        {
            var mesh = new Mesh();

            //UVs
            var uvs = new Vector2[MovementAreaOutline.Count];
            for (int i = 0; i < uvs.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    uvs[i] = new Vector2(0, 0);
                }
                else
                {
                    uvs[i] = new Vector2(1, 1);
                }
            }
            //Triangles
            int[] tris = new int[3 * (MovementAreaOutline.Count - 2)];    //3 verts per triangle * num triangles
            var C1 = MovementAreaOutline.Count - 1;
            var C2 = 0;
            var C3 = 1;

            if (MovementAreaOutline.Count > 2)
            {
                for (int i = 0; i < tris.Length; i += 3)
                {
                    tris[i] = C1;
                    tris[i + 1] = C2;
                    tris[i + 2] = C3;

                    C2 = C3;
                    C3++;
                }
            }

            //Assign data to mesh
            mesh.vertices = MovementAreaOutline.ToArray();
            mesh.uv = uvs;
            mesh.triangles = tris;

            //Recalculations
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();

            //Name the mesh
            mesh.name = "MovementAreaMesh";

            //Return the mesh
            return mesh;
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
