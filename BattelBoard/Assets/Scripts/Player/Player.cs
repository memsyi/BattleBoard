using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        #region Variables

        public List<UnitBehaviour> Units { get; set; }

        public GameControllerBehaviour GameController { get { return FindObjectOfType<GameControllerBehaviour>(); } }

        #endregion

        #region Methods



        #endregion
    }
}
