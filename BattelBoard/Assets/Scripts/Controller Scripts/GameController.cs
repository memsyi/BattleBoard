using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField]
        private int _playerCount = 2;

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

        public bool IsGuiSelected { get; set; }

        public Dictionary<int, Player> Players { get; set; }

        public Text Text { get { return FindObjectOfType<Text>(); } }

        public void OnSkipButtonClick()
        {
            SetTurn();
        }

        private void UpdateGame()
        {
            if (CurrentPlayer.AreUnitsOutOfMoves)
            {
                SetTurn();
            }
        }

        private void SetTurn()
        {
            var nextId = CurrentPlayer.PlayerId % _playerCount + 1;

            if (nextId == 1)
            {
                _currentTurn++;
            }

            if (_currentTurn == 4)
            {
                print("quit");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            CurrentPlayer = Players[nextId];

            foreach (var p in Players)
            {
                if (p.Key == CurrentPlayer.PlayerId)
                {
                    CurrentPlayer.SetActive();
                    CameraController.SetCameraPositionAndRotation(CurrentPlayer.PlayerCameraPosition, CurrentPlayer.PlayerCameraRotation);
                }
                else
                {
                    p.Value.SetInactive();
                }
            }
            Text.text = "Round: " + _currentTurn + "\nActive turn: Player " + CurrentPlayer;
        }

        // Use this for initialization
        void Start()
        {
            Players = new Dictionary<int, Player>();
            for (var i = 1; i <= PlayerCount; i++)
            {
                var player = new Player(i);
                Players.Add(i, player);
            }

            CurrentPlayer = Players[2];
            SetTurn();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGame();
        }
    }
}
