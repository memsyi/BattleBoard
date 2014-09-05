using System.Configuration;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {

        private SelectableUnitBehaviour _selectableUnitBehaviour;

        [SerializeField]
        private int _movingDistance = 10;

        public int MovingDistance
        {
            get { return _movingDistance; }
            set { _movingDistance = value; }
        }

        // Use this for initialization
        void Start()
        {
            _selectableUnitBehaviour = gameObject.GetComponentInParent<SelectableUnitBehaviour>();

            transform.localScale = new Vector3(MovingDistance, 0, MovingDistance);
        }

        // Update is called once per frame
        private void Update()
        {
            renderer.enabled = _selectableUnitBehaviour.IsSelected;
        }
    }
}
