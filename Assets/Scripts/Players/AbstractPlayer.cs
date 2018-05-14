using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players
{
    public abstract class AbstractPlayer : ControlSetup
    {
        public string PlayerNumber;
        public float FallMultiplier = 1.5f;
        public float LowJumpMultiplier = 1f;
        public float HorizontalVelocityModifier = 700f;
        public float JumpForceModifier = 200f;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private readonly List<Collider2D> _groundTouched = new List<Collider2D>();
        private bool _jumpRequest;
        protected PlayerType playerType;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            _sr = GetComponent<SpriteRenderer>();
            setController(PlayerNumber);
        }

        void Update()
        {
            // Jump is pressed and player is grounded
            if (Input.GetButtonDown(_jump) && _groundTouched.Count != 0)
            {
                _jumpRequest = true;
            }

            FlipSprite();
        }

        private void FixedUpdate()
        {
            if (_jumpRequest)
            {
                _rb.AddForce(Vector2.up * Input.GetAxis(_jump) * JumpForceModifier, ForceMode2D.Impulse);
                _jumpRequest = false;
            }


            var horizontalInput = Input.GetAxis(_horizontal);

            var modifier = _groundTouched.Count > 0 ? HorizontalVelocityModifier : HorizontalVelocityModifier / 2;

            if (_rb.velocity.x <= 7 && _rb.velocity.x >= -7)
            {
                _rb.AddForce(Vector2.right * modifier * horizontalInput, ForceMode2D.Force);
            }


            // Better Jump
            // Makes Jump as long as player holds jump button down. 
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * FallMultiplier * Time.fixedDeltaTime;
            }
            else if (_rb.velocity.y > 0 && Input.GetAxis(_jump).Equals(0))
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * LowJumpMultiplier * Time.fixedDeltaTime;
            }
        }

        private void FlipSprite()
        {
            _sr.flipX = _rb.velocity.x > 0.01f;
        }

        private void OnCollisionEnter2D(Collision2D hit)
        {
            if (!hit.gameObject.tag.Equals("platform")) return;

            var points = new ContactPoint2D[2];
            hit.GetContacts(points);
            if (points.Any(p => p.normal == Vector2.up && !_groundTouched.Contains(hit.collider)))
            {
                _groundTouched.Add(hit.collider);
            }
        }

        private void OnCollisionExit2D(Collision2D hit)
        {
            if (_groundTouched.Contains(hit.collider))
            {
                _groundTouched.Remove(hit.collider);
            }
        }
    }
}