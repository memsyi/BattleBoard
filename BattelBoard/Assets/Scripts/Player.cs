using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    [Serializable]
    public class Player
    {

        public Vector3 PlayerCameraPosition { get; private set; }

        public Vector3 PlayerCameraRotation { get; private set; }

        public int PlayerId { get; private set; }

        public List<Unit> Units { get; set; }

        public bool AreUnitsOutOfMoves { get { return Units.TrueForAll(x => x.IsOutOfMoves); } }

        public Player(int playerId)
        {
            PlayerId = playerId;
            Units = GameController.Instance.UnitsOnBattleField.Where(x => x.ControllingPlayer == PlayerId).ToList();
            AdjustCameraSettings();
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                Units.ForEach(x => x.Reset());
            }
            else
            {
                Units.ForEach(x =>
                {
                    x.SetActive(false);
                    x.IsSelected = false;
                });
            }
        }

        private void AdjustCameraSettings()
        {
            var cameraController = Object.FindObjectOfType<CameraController>();
            switch (PlayerId)
            {
                case 1:
                    PlayerCameraPosition = new Vector3(25, 4, 0);
                    PlayerCameraRotation = new Vector3(35, 0, 0);
                    break;
                case 2:
                    PlayerCameraPosition = new Vector3(25, 4, 50);
                    PlayerCameraRotation = new Vector3(35, 180, 0);
                    break;
            }
        }
    }
}
