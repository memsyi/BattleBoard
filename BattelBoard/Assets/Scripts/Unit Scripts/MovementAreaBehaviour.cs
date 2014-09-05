using System.Configuration;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {

        private SelectableUnitBehaviour _selectableUnitBehaviour;

        // Use this for initialization
        void Start()
        {
            _selectableUnitBehaviour = gameObject.GetComponentInParent<SelectableUnitBehaviour>();
            var movingDistance = _selectableUnitBehaviour.MovingDistance;
            transform.localScale = new Vector3(movingDistance,0,movingDistance);
        }

        // Update is called once per frame
        private void Update()
        {
            renderer.enabled = _selectableUnitBehaviour.IsSelected;
        }
    }
}
