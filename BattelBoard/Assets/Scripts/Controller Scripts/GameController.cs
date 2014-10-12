using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField]
        private int _maxTurns;

        [SerializeField]
        private int _playerCount = 2;

        [SerializeField]
        private bool _showMovementAreaOfUnits = false;

        public bool ShowMovementAreaOfUnits
        {
            get { return _showMovementAreaOfUnits; }
            set { _showMovementAreaOfUnits = value; }
        }

        private int _currentTurn = 0;

        private Text _winText;

        private Text _loseText;
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

        public Text Text { get { return FindObjectsOfType<Text>().First(x => x.tag == Tags.Text); } }

        public bool IsCurrentPlayerWinner { get; private set; }

        public bool IsGameOver { get; private set; }

        public void OnSkipButtonClick()
        {
            if (!IsGameOver)
            {
                SetTurn();
            }
        }

        public void OnWinButtonClick()
        {
            if (IsGameOver)
            {
                return;
            }
            SetGameOverCameraPosition();
            IsCurrentPlayerWinner = true;
            IsGameOver = true;
        }

        public void OnLoseButtonClick()
        {
            if (IsGameOver)
            {
                return;
            }
            SetGameOverCameraPosition();
            IsCurrentPlayerWinner = false;
            IsGameOver = true;
        }

        private void SetGameOverCameraPosition()
        {
            var playerUnit = CurrentPlayer.Units.First();
            var cam = CameraController.GetComponentInChildren<Camera>();
            var rotationMultiplier = CurrentPlayer.PlayerId == 2 ? -1 : 1;
            cam.transform.position = playerUnit.transform.position + Vector3.up * 2 + Vector3.back * 5 * rotationMultiplier;
        }

        private void HandleGameOver()
        {
            if (!IsGameOver)
            {
                return;
            }

            if (IsCurrentPlayerWinner)
            {
                _winText.enabled = true;
                _loseText.enabled = false;

                ShowWinningScreen();
            }
            else
            {
                _winText.enabled = false;
                _loseText.enabled = true;

                ShowLosingScreen();
            }
        }


        private void ShowWinningScreen()
        {
            var playerUnit = CurrentPlayer.Units.First();
            CameraController.GetComponentInChildren<Camera>().transform.RotateAround(playerUnit.transform.position, Vector3.up, 0.5f);
        }

        private void ShowLosingScreen()
        {
            var playerUnit = CurrentPlayer.Units.First();
            CameraController.GetComponentInChildren<Camera>().transform.RotateAround(playerUnit.transform.position, Vector3.up, -0.1f);
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

            if (_currentTurn == _maxTurns + 1)
            {
                print("quit");
                //UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }

            CurrentPlayer = Players[nextId];

            foreach (var p in Players)
            {
                if (p.Key == CurrentPlayer.PlayerId)
                {
                    CurrentPlayer.SetActive(true);
                    CameraController.SetCameraPositionAndRotation(CurrentPlayer.PlayerCameraPosition, CurrentPlayer.PlayerCameraRotation);
                }
                else
                {
                    p.Value.SetActive(false);
                }
            } Text.text = "Round: " + _currentTurn + "\nActive turn: Player " + CurrentPlayer.PlayerId;
        }

        // Use this for initialization
        void Start()
        {
            _winText = GameObject.Find("WinText").GetComponent<Text>();
            _winText.enabled = false;

            _loseText = GameObject.Find("LoseText").GetComponent<Text>();
            _loseText.enabled = false;

            Players = new Dictionary<int, Player>();
            for (var i = 1; i <= PlayerCount; i++)
            {
                var player = new Player(i);
                Players.Add(i, player);
            }

            foreach (var player in Players)
            {
                var playerUnits = UnitsOnBattleField.Where(x => x.ControllingPlayer == player.Key).ToList();
                player.Value.Units = playerUnits;
            }

            CurrentPlayer = Players[2];

            SetTurn();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGame();
        }

        void FixedUpdate()
        {
            HandleGameOver();
        }
    }
}
