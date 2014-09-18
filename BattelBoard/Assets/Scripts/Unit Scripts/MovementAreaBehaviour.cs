using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private int _movingDistance, _areaSubdevisions = 0;

        [SerializeField]
        Transform _movementAreaSprite = null;

        public Transform MovementAreaSprite
        {
            get { return _movementAreaSprite; }
        }

        public int AreaSubdevisions
        {
            get { return _areaSubdevisions + 1; }
        }

        public int MovingDistance
        {
            get { return _movingDistance; }
            set
            {
                _movingDistance = value;
                SpriteRenderer.transform.localScale = new Vector3(_movingDistance * 2, _movingDistance * 2, 1);
            }
        }

        public UnitBehaviour Unit { get { return GetComponentInParent<UnitBehaviour>(); } }

        public SpriteRenderer SpriteRenderer { get { return GetComponentInChildren<SpriteRenderer>(); } }

        public LineRenderer LineRenderer { get { return GetComponent<LineRenderer>(); } }

        private List<Vector3> MovementAreaPoints { get; set; }

        private List<Transform> MovementAreaSprites { get; set; }

        private List<Vector3> MovementAreaOutline { get; set; }

        private float SpriteScale { get { return 0.5f; } }

        #endregion

        #region Methods

        private void Init()
        {
            MovingDistance = _movingDistance;
            MovementAreaPoints = new List<Vector3>();
            MovementAreaOutline = new List<Vector3>();
            MovementAreaSprites = new List<Transform>();
        }

        private void HandleMovementAreaDisplay()
        {
            //SpriteRenderer.transform.localScale = new Vector3(MovingDistance, MovingDistance, 1);
            //SpriteRenderer.enabled = Unit.IsSelected;

            CalculateAreaPoints();

            CalculateAreaOutline();
            DrawAreaOutline();
            //ShowAreaPoints();
        }

        private void CalculateAreaPoints()
        {
            MovementAreaPoints.Clear();

            for (float x = -MovingDistance * AreaSubdevisions; x <= MovingDistance * AreaSubdevisions; x++)
            {
                for (float y = -MovingDistance * AreaSubdevisions; y <= MovingDistance * AreaSubdevisions; y++)
                {
                    var target = new Vector3(transform.position.x + x / AreaSubdevisions, transform.position.y, transform.position.z + y / AreaSubdevisions);

                    if (GetPathIsPossible(target))
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

            MovementAreaOutline.GroupBy(p => p.z);//.GroupBy(p => p.z);

            MovementAreaOutline.Add(transform.position);
        }

        private bool GetPathIsPossible(Vector3 target)
        {
            if (Vector3.Distance(transform.position, target) > MovingDistance + 0.1f)
            {
                return false;
            }

            NavMeshPath path = new NavMeshPath();
            Unit.NavMeshAgent.CalculatePath(target, path);

            if (path.status != NavMeshPathStatus.PathComplete
                || path.corners[path.corners.Length - 1] != target)
            {
                return false;
            }

            var pathLength = 0f;

            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            if (pathLength > MovingDistance + 0.1f)
            {
                return false;
            }

            return true;
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

            //foreach (var point in MovementAreaPoints)
            //{
            //    Debug.DrawLine(transform.position, point, Color.red); // TODO y position => ground high + transform.position.y
            //}
        }

        private void DrawAreaOutline()
        {
            var meshFilter = gameObject.GetComponent<MeshFilter>();

            meshFilter.mesh = CreateMesh();

            //LineRenderer.SetVertexCount(MovementAreaOutline.Count);
            //for (var i = 0; i < MovementAreaOutline.Count; i++)
            //{
            //    LineRenderer.SetPosition(i, MovementAreaOutline[i]);
            //}

            //MovementAreaSprite.transform.localScale = new Vector3(SpriteScale, SpriteScale, 1);

            //foreach (var spriteObject in MovementAreaSprites)
            //{
            //    Destroy(spriteObject.gameObject);
            //}

            //MovementAreaSprites.Clear();

            //foreach (var point in MovementAreaOutline)
            //{
            //    MovementAreaSprites.Add(Instantiate(MovementAreaSprite, point, Quaternion.Euler(new Vector3(90, 0, 0))) as Transform);
            //}
        }


        private Mesh CreateMesh()
        {
            var mesh = new Mesh();

            //Vertices
            //var vertices = new Vector3[MovementAreaOutline.Count];
            //for (int i = 0; i < MovementAreaOutline.Count; i++)
            //{
            //    vertices[i] = MovementAreaOutline[i] - transform.position;
            //}
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
            var C1 = 0;
            var C2 = 0;
            var C3 = 0;

            if (MovementAreaOutline.Count > 2)
            {
                //    C1 = 0;
                //    C2 = 1;
                //    C3 = 2;

                //    for (int i = 0; i < tris.Length; i += 3)
                //    {
                //        tris[i] = C1;
                //        tris[i + 1] = C2;
                //        tris[i + 2] = C3;

                //        C2++;
                //        C3++;
                //    }
                //}
                //else
                //{
                C1 = MovementAreaOutline.Count -1;
                C2 = 1;
                C3 = 2;

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
            mesh.name = "MyMesh";

            //Return the mesh
            return mesh;
        }

        #endregion

        #region MonoBehaviour Implementation

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

        #endregion
    }
}
