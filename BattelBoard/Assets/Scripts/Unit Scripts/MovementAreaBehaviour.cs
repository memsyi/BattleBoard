using System;
using System.Configuration;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {

        private SelectableUnitBehaviour _selectableUnitBehaviour;
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private int _movingDistance = 0;

        public int MovingDistance
        {
            get { return _movingDistance; }
            set
            {
                _movingDistance = value;
                transform.localScale = new Vector3((float)Math.Sqrt(_movingDistance), 1, (float)Math.Sqrt(_movingDistance));
            }
        }

        // Use this for initialization
        void Start()
        {
            _selectableUnitBehaviour = gameObject.GetComponentInParent<SelectableUnitBehaviour>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            transform.localScale = new Vector3(MovingDistance, 1, MovingDistance);
        }

        // Update is called once per frame
        private void Update()
        {
            _spriteRenderer.enabled = _selectableUnitBehaviour.IsSelected;
        }
    }
}
