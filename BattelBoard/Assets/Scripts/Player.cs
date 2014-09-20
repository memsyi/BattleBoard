using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class Player
    {

        #region Variables

        public List<Unit> Units { get; set; }

        public bool AreUnitsOutOfMoves { get { return Units.TrueForAll(x => x.IsOutOfMoves); } }

        #endregion

        #region Methods


        #endregion
    }
}
