using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float Speed = 10;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;

    private Transform _feet;
    private Force _force1, _force2, _force3;

    private bool _chargingForce = false;
    private bool _performingForce = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();

        _feet = transform.Find("Feet");

        _force1 = transform.Find("Force1").GetComponent<Force>();
        _force2 = transform.Find("Force2").GetComponent<Force>();
        _force3 = transform.Find("Force3").GetComponent<Force>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        force();
        move();
        rotate();
    }

    private void move()
    {
        if (_chargingForce || _performingForce)
        {
            _feet.localPosition = Vector2.zero;
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 direction = new Vector2(horizontal, vertical);

            if (direction.sqrMagnitude < 0.1f)
            {
                _feet.localPosition = Vector2.zero;
                return;
            }

            direction = direction.normalized * Mathf.Min(direction.magnitude, 1.0f);

            _rigidbody.MovePosition(_rigidbody.position + direction * Speed * Time.deltaTime);

            _feet.localPosition = transform.worldToLocalMatrix.MultiplyVector(-direction) * 0.2f;
        }
    }
    private void rotate()
    {
        if (_performingForce)
        {

        }
        else
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            float angle = Mathf.Atan2(direction.x, direction.y);

            _rigidbody.MoveRotation(-angle * Mathf.Rad2Deg);
        }
    }

    private void force()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(performForce());
        }
    }

    private IEnumerator performForce()
    {
        _chargingForce = true;

        float duration = 0f;
        int forceLevel = 1;

        _spriteRenderer.color = _force1.SpriteRenderer.color;

        ParticleSystem.MainModule particles = _particleSystem.main;

        particles.startColor = _force1.SpriteRenderer.color;
        particles.startLifetime = new ParticleSystem.MinMaxCurve(0.25f);
        particles.startSpeed = new ParticleSystem.MinMaxCurve(1f);

        _particleSystem.Play();

        while (Input.GetButton("Fire1"))
        {
            duration += Time.deltaTime;

            if (forceLevel==1 && duration > 0.5f)
            {
                forceLevel = 2;
                _spriteRenderer.color = _force2.SpriteRenderer.color;

                particles.startColor = _force2.SpriteRenderer.color;
                particles.startLifetime = new ParticleSystem.MinMaxCurve(0.35f);
                particles.startSpeed = new ParticleSystem.MinMaxCurve(1.5f);
            }

            if (forceLevel==2 && duration > 1f)
            {
                forceLevel = 3;
                _spriteRenderer.color = _force3.SpriteRenderer.color;

                particles.startColor = _force3.SpriteRenderer.color;
                particles.startLifetime = new ParticleSystem.MinMaxCurve(0.45f);
                particles.startSpeed = new ParticleSystem.MinMaxCurve(2f);
            }

            yield return null;
        }

        _particleSystem.Stop();
        _chargingForce = false;
        _performingForce = true;

        Force force = null;
        switch (forceLevel)
        {
            case 1:
                force = _force1;
                break;
            case 2:
                force = _force2;
                break;
            case 3:
                force = _force3;
                break;
        }

        yield return force.Activate(0.2f);

        _spriteRenderer.color = Color.white;

        _performingForce = false;

    }

}
