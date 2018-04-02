using UnityEngine;

namespace Players
{
	public abstract class AbstractPlayer : ControlSetup
	{

		public string PlayerNumber;
		private Rigidbody2D _rb;

		private bool _landed;
		protected PlayerType playerType;

		public void Awake ()
		{
			_rb = GetComponent<Rigidbody2D> ();
			setController (PlayerNumber);
			_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}


		private void FixedUpdate ()
		{
			// Fazer a programaão do player
			var horizontalInput = Input.GetAxis (_horizontal);

			var modifier = _landed ? 10.0f : 5.0f;

			_rb.AddForce (Vector2.right * modifier * horizontalInput, ForceMode2D.Force);
			
			if (!_landed) return;
			
			if (Input.GetButtonDown (_jump)) {
				_rb.AddForce (Vector2.up * 7f, ForceMode2D.Impulse);
			}
		}

		private void OnCollisionEnter2D (Collision2D collider)
		{
			if (!collider.gameObject.tag.Equals("platform")) return;
			
			_landed = true;
			Debug.Log ("Landed changed to " + _landed);
		}

		private void OnCollisionExit2D (Collision2D collider)
		{
			if (!collider.gameObject.tag.Equals("platform")) return;
			
			_landed = false;
			Debug.Log ("Landed changed to " + _landed);
		}
	}
}
