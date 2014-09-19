using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameControllerBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private int _playerCount;

        public int PlayerCount
        {
            get { return _playerCount; }
            set { _playerCount = value; }
        }


        public List<UnitBehaviour> UnitsOnBattleField
        {
            get { return FindObjectsOfType<UnitBehaviour>().ToList(); }
        }

        public List<UnitBehaviour> UnitsOnScreen
        {
            get { return UnitsOnBattleField.Where(x => x.IsOnScreen).ToList(); }
        }

        public bool IsGuiSelected { get; set; }

        public Dictionary<int, Player> Players { get; set; }

        #endregion

        #region Methods

        #endregion

        #region MonoBehaviour Implementation

        // Use this for initialization
        void Start()
        {
            Players = new Dictionary<int, Player>();
            for (var i = 1; i <= PlayerCount; i++)
            {
                var player = new Player();
                Players.Add(i, player);
            }

            foreach (var player in Players)
            {
                var playerUnits = UnitsOnBattleField.Where(x => x.ControllingPlayer == player.Key).ToList();
                player.Value.Units = playerUnits;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
    }
}
