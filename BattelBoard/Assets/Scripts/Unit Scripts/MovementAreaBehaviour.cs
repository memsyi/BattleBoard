using UnityEngine;

namespace Assets.Scripts
{
    public class MovementAreaBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private int _movingDistance;

        public int MovingDistance
        {
            get { return _movingDistance; }
            set
            {
                _movingDistance = value;
                SpriteRenderer.transform.localScale = new Vector3(_movingDistance, _movingDistance, 1);
            }
        }

        public UnitBehaviour Unit { get { return GetComponentInParent<UnitBehaviour>(); } }

        public SpriteRenderer SpriteRenderer { get { return GetComponentInChildren<SpriteRenderer>(); } }

        #endregion

        #region Methods

        private void Init()
        {
        }

        private void HandleMovementAreaDisplay()
        {
            SpriteRenderer.transform.localScale = new Vector3(MovingDistance, MovingDistance, 1);
            SpriteRenderer.enabled = Unit.IsSelected;
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
