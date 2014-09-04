using System.Configuration;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {

        private SelectableObjectBehaviour _selectableObjectBehaviour;

        // Use this for initialization
        void Start()
        {
            _selectableObjectBehaviour = gameObject.GetComponentInParent<SelectableObjectBehaviour>();
            var movingDistance = _selectableObjectBehaviour.MovingDistance;
            transform.localScale = new Vector3(movingDistance,0,movingDistance);
        }

        // Update is called once per frame
        private void Update()
        {
            renderer.enabled = _selectableObjectBehaviour.IsSelected;
        }
    }
}
