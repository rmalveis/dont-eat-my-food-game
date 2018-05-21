using System.Runtime.InteropServices;
using Players;

namespace GameController
{
    public class PlayerGameController
    {
        public int Points { get; set; }

        public bool PowerUpEnabled { get; set; }
        public int LastPowerUpAt { get; set; }

        public PlayerType Type { get; protected set; }


        public PlayerGameController(PlayerType type)
        {
            Type = type;
        }

        public void PowerUpFired()
        {
            PowerUpEnabled = false;
            LastPowerUpAt = Points;
        }
    }
}