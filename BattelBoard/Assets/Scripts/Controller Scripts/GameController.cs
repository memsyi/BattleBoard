using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField]
        private int _playerCount = 2;

        private int _currentPlayer;

        private int _currentTurn = 0;
        public int PlayerCount
        {
            get { return _playerCount; }
            set { _playerCount = value; }
        }

        public CameraController CameraController { get { return FindObjectOfType<CameraController>(); } }

        public Player CurrentPlayer { get; set; }

        public List<Unit> UnitsOnBattleField
        {
            get { return FindObjectsOfType<Unit>().ToList(); }
        }

        public List<Unit> UnitsOnScreen
        {
            get { return UnitsOnBattleField.Where(x => x.IsOnScreen).ToList(); }
        }

        public bool IsGuiSelected { get; set; }

        public Dictionary<int, Player> Players { get; set; }

        public Text Text { get { return FindObjectOfType<Text>(); } }

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
                print("quit");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            foreach (var p in Players)
            {
                if (p.Key == player)
                {
                    CurrentPlayer = p.Value;
                    CurrentPlayer.Units.ForEach(x => x.SetActive(true));
                }
                else
                {
                    p.Value.Units.ForEach(x =>
                    {
                        x.SetActive(false);
                        x.IsSelected = false;
                    });
                    
                }
            }
            Text.text = "Active turn: Player " + player;
            _currentTurn++;

            if (player == 1)
            {
                CameraController.SetCameraPositionAndRotation(CameraController.CameraPositionPlayer1, CameraController.CameraRotationPlayer1);
            }
            else
            {
                CameraController.SetCameraPositionAndRotation(CameraController.CameraPositionPlayer2, CameraController.CameraRotationPlayer2);
            }
        }

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

            _currentPlayer = 1;

            SetTurn(_currentPlayer);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGame();
        }
    }
}
