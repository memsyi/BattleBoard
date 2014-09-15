using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameControllerBehaviour : MonoBehaviour
    {
        #region Variables

        public List<UnitBehaviour> SelectableUnits
        {
            get { return FindObjectsOfType<UnitBehaviour>().ToList(); }
        }

        public List<UnitBehaviour> UnitsOnScreen
        {
            get { return SelectableUnits.Where(x => x.IsOnScreen).ToList(); }
        }

        #endregion

        #region Methods

        #endregion

        #region MonoBehaviour Implementation

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
