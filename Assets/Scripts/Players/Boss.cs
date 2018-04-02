using UnityEngine;

namespace Players
{
	public class Boss : AbstractPlayer {

		// Use this for initialization
		private void Start () {
			playerType = PlayerType.Boss;
			Debug.Log ("Type: " + playerType);
		}
	
	}
}
