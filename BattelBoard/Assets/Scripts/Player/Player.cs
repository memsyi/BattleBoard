using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player
    {
        public Player(GameControllerBehaviour gameController)
        {
            GameController = gameController;
        }

        #region Variables

        public List<UnitBehaviour> Units { get; set; }

        public bool AreUnitsOutOfMoves { get { return Units.TrueForAll(x => x.IsOutOfMoves); } }

        public GameControllerBehaviour GameController { get; private set; }

        #endregion

        #region Methods


        #endregion
    }
}
