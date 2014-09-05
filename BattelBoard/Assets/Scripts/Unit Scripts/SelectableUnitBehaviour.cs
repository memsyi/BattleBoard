using UnityEngine;

namespace Assets.Scripts
{
    public class SelectableUnitBehaviour : MonoBehaviour
    {
        void Awake()
        {

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HandleSelection();
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }

            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    renderer.material.color *= 2f;
                    return;
                }
                renderer.material.color *= 0.5f;
            }
        }

        private void HandleSelection()
        {
            if (!Input.GetButtonUp("Fire1"))
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            float distance = 100f;

            if (transform.collider.Raycast(ray, out hitInfo, distance))
            {
                IsSelected = !IsSelected;
            }
            else if (IsSelected)
            {
                IsSelected = false;
            }
        }
    }
}
