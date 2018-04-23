using UnityEngine;

namespace Players
{
    public abstract class AbstractPlayer : ControlSetup
    {
        public string PlayerNumber;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        public bool _landed;
        public float HorizontalVelocityModifier = 700f;
        public float JumpForceModifier = 200f;
        protected PlayerType playerType;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            _sr = GetComponent<SpriteRenderer>();
            setController(PlayerNumber);
        }


        private void FixedUpdate()
        {
            var horizontalInput = Input.GetAxis(_horizontal);


            var modifier = _landed ? HorizontalVelocityModifier : HorizontalVelocityModifier / 2;

            if (_rb.velocity.x <= 7 && _rb.velocity.x >= -7)
            {
                _rb.AddForce(Vector2.right * modifier * horizontalInput, ForceMode2D.Force);
            }


            if (_landed)
            {
                _rb.AddForce(Vector2.up * Input.GetAxis(_jump) * JumpForceModifier, ForceMode2D.Impulse);
            }

            if (_rb.velocity.x > 0.01f)
            {
                if (_sr.flipX)
                {
                    _sr.flipX = false;
                }
            }
            else if (_rb.velocity.x < -0.01f)
            {
                if (!_sr.flipX)
                {
                    _sr.flipX = true;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D hit)
        {
            if (!hit.gameObject.tag.Equals("platform")) return;

            _landed = true;
        }

        private void OnCollisionExit2D(Collision2D hit)
        {
            if (!hit.gameObject.tag.Equals("platform")) return;

            _landed = false;
        }
    }
}