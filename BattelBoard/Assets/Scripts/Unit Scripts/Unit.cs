﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private int _controllingPLayer;

        public int ControllingPlayer
        {
            get { return _controllingPLayer; }
            set { _controllingPLayer = value; }
        }

        private Color _defaultColor;

        public NavMeshAgent NavMeshAgent { get { return GetComponent<NavMeshAgent>(); } }

        public MovementArea MovementArea { get { return GetComponentInChildren<MovementArea>(); } }

        public bool IsOutOfMoves
        {
            get { return MovingDistance == 0; }
        }

        public float MovingDistance
        {
            get
            {
                return MovementArea.MovingDistance;
            }
            set
            {
                MovementArea.MovingDistance = value <= 0 ? 0 : value;
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
                // Since the y-Axis on WorldToScreenPoint is flipped, we have to substract the value from the actual screen height
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
            get { return NavMeshAgent.velocity.magnitude > 0; }
        }

        public void Reset()
        {
            MovingDistance = 5;
            renderer.material.color = _defaultColor;
        }

        private void Init()
        {
            MouseController.Instance.MousePositionChanged += OnMousePositionChanged;
            _defaultColor = renderer.material.color;
        }

        public List<Vector3> GetLineRendererPositions()
        {
            var corners = NavMeshAgent.path.corners.ToList();
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
            var destination = MouseController.Instance.transform.position;
            var heading = destination - transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            if (distance >= MovingDistance)
            {
                heading = direction * MovingDistance;
                destination = heading + transform.position;
            }
            if (IsSelected && IsActive)
            {
                SetMovementDestination(destination);
                MovingDistance -= distance;
            }
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
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}