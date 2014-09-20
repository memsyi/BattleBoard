using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameControllerBehaviour : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private int _playerCount;

        [SerializeField]
        private Transform camera1;

        [SerializeField]
        private Transform camera2;

        private int _currentPlayer;

        private int _currentTurn = 0;
        public int PlayerCount
        {
            get { return _playerCount; }
            set { _playerCount = value; }
        }

        public Player CurrentPlayer { get; set; }

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

        public Text Text { get { return FindObjectOfType<Text>(); } }

        #endregion

        #region Methods

        public void OnSkipButtonClick()
        {
            _currentPlayer = _currentPlayer % 2 + 1;
            SetTurn(_currentPlayer);
        }
        private void UpdateGame()
        {
            if (CurrentPlayer.AreUnitsOutOfMoves)
            {
                _currentPlayer = _currentPlayer % 2 + 1;
                SetTurn(_currentPlayer);
                CurrentPlayer.Units.ForEach(x => x.Reset());
            }
        }

        private void SetTurn(int player)
        {
            if (_currentTurn / 2 == 3)
            {
                Application.Quit();
            }

            print(("set turn entry"));
            foreach (var p in Players.Where(p => p.Key == player))
            {
                CurrentPlayer = p.Value;
            }
            Text.text = "Active turn: Player " + player;
            print("player:" + player);

            if (player ==1)
            {
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
            }
            else
            {
                camera2.gameObject.SetActive(true);
                camera1.gameObject.SetActive(false);
            }
        }

        #endregion

        #region MonoBehaviour Implementation

        // Use this for initialization
        void Start()
        {
            Players = new Dictionary<int, Player>();
            for (var i = 1; i <= PlayerCount; i++)
            {
                var player = new Player(this);
                Players.Add(i, player);
            }

            foreach (var player in Players)
            {
                var playerUnits = UnitsOnBattleField.Where(x => x.ControllingPlayer == player.Key).ToList();
                player.Value.Units = playerUnits;
            }

            _currentPlayer = 1;
            print("init gamecontroller");

            SetTurn(_currentPlayer);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGame();
        }

        #endregion
    }
}
