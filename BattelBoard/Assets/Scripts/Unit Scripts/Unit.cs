using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private int _controllingPlayer;

        [SerializeField]
        private string _name;

        [SerializeField]
        private float _health;

        [SerializeField]
        private float _defense;

        [SerializeField]
        private float _armor;

        [SerializeField]
        private int _movingPoints;

        [SerializeField]
        private int _actionPoints;

        [SerializeField]
        private List<Utility> _weapons;

        [SerializeField]
        private List<Utility> _abilities;

        [SerializeField]
        private bool _isHero;

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public float Health
        {
            get { return _health; }
            private set { _health = value; }
        }

        public float Defense
        {
            get { return _defense; }
            private set { _defense = value; }
        }

        public float Armor
        {
            get { return _armor; }
            private set { _armor = value; }
        }

        public int MovingPoints
        {
            get { return _movingPoints; }
            private set { _movingPoints = value; }
        }

        public int ActionPoints
        {
            get { return _actionPoints; }
            private set { _actionPoints = value; }
        }

        public List<Utility> Weapons
        {
            get { return _weapons; }
            private set { _weapons = value; }
        }

        public List<Utility> Abilities
        {
            get { return _abilities; }
            private set { _abilities = value; }
        }

        public bool IsHero
        {
            get { return _isHero; }
            private set { _isHero = value; }
        }

        public int ControllingPlayer
        {
            get { return _controllingPlayer; }
            private set { _controllingPlayer = value; }
        }

        public NavMeshAgent NavMeshAgent { get { return GetComponent<NavMeshAgent>(); } }

        public MovementArea MovementArea { get { return GetComponentInChildren<MovementArea>(); } }

        public bool IsOutOfMoves
        {

            get { return MovingDistance.Equals(0) && !IsMoving || !IsActive; }
        }

        public float MovingDistance
        {
            get { return MovementArea.MovingDistance; }
            set
            {
                MovementArea.MovingDistance = value <= 0.5 ? 0 : value;
                if (IsOutOfMoves)
                {
                    SetActive(false);
                }
            }
        }

        public Vector3 ScreenPosition
        {
            get
            {
                /* 
                 * Since the y-Axis on WorldToScreenPoint is flipped, 
                 * we have to substract the value from the actual screen height
                 */
                var worldToScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                worldToScreenPoint.y = Screen.height - worldToScreenPoint.y;
                return worldToScreenPoint;
            }
        }

        public bool IsOnScreen
        {
            get
            {
                return ScreenPosition.x > 0
                       && ScreenPosition.x < Screen.width
                       && ScreenPosition.y > 0
                       && ScreenPosition.y < Screen.height;
            }
        }

        public bool IsActive { get; private set; }

        private bool _hasAlreadyMoved;

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                {
                    return;
                }
                if (_hasAlreadyMoved)
                {
                    SetActive(false);
                    MovingDistance = 0;
                }

                _isSelected = value;
                HandleColorChange();
            }
        }

        public void SetActive(bool state)
        {
            IsActive = state;
        }

        public bool IsMoving
        {
            get { return NavMeshAgent.hasPath || NavMeshAgent.pathPending; }
        }

        public void Reset()
        {
            MovingDistance = 5;
            IsActive = true;
            _hasAlreadyMoved = false;
        }

        private void Init()
        {
            MouseController.Instance.MousePositionChanged += OnMousePositionChanged;
        }

        public List<Vector3> GetLineRendererPositions(NavMeshPath path)
        {
            var corners = path.corners.ToList();
            return corners;
        }

        public float GetDistanceToTarget()
        {
            float pathLength = 0;
            var pathCorners = NavMeshAgent.path.corners;

            for (var i = 0; i < pathCorners.Length - 1; i++)
            {
                pathLength += Vector3.Distance(pathCorners[i + 1], pathCorners[i]);
            }

            return pathLength;
        }

        private void SetMovementDestination(Vector3 destination)
        {
            NavMeshAgent.SetDestination(destination);
        }

        private void OnMousePositionChanged(object mouseClickPosition, EventArgs e)
        {
            if (IsSelected && IsActive && !IsMoving)
            {
                float pathLength;
                SetMovementDestination(GetMovementDestination(out pathLength));
                MovingDistance -= pathLength;
            }
        }

        public Vector3 GetMovementDestination(out float pathLength)
        {
            var destination = MouseController.Instance.transform.position;

            var path = GetPathToDestination(destination);
            IsPathOutOfMovementArea(path, out destination, out pathLength);

            return destination;
        }

        public bool IsPathOutOfMovementArea(NavMeshPath path, out Vector3 destination, out float pathLength)
        {
            pathLength = 0f;
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                destination = transform.position;
                return false;
            }
            destination = path.corners[path.corners.Length - 1];

            for (var i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);

                if (pathLength > MovingDistance)
                {
                    destination = Vector3.MoveTowards(path.corners[i], path.corners[i - 1], pathLength - MovingDistance);
                    pathLength = MovingDistance;
                    return true;
                }
            }

            return false;
        }

        public NavMeshPath GetPathToDestination(Vector3 destination)
        {
            var path = new NavMeshPath();
            NavMeshAgent.CalculatePath(destination, path);

            return path;
        }

        private void HandleColorChange()
        {
            if (IsSelected)
            {
                renderer.material.color *= 2f;
            }
            else
            {
                renderer.material.color *= 0.5f;
            }
        }

        // Use this for initialization
        public virtual void Start()
        {
            Init();
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
}
