using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public const float ReturnSphereDuration = 5f;

    public float Speed = 10;

    public Sphere Sphere;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _chargeForceParts, _returnSphereParts;

    private Transform _feet;
    private Transform _hands;
    private Force _force1, _force2, _force3;

    private bool _chargingForce = false;
    private bool _performingForce = false;
    public bool PerformingForce
    {
        get
        {
            return _performingForce;
        }
    }

    private bool _waitingForSphere = false;

    private Force _currentForce = null;
    public Force CurrentForce
    {
        get
        {
            return _currentForce;
        }
    }

    private bool _usingController = false;
    private Vector3 _previousMousePosition;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _chargeForceParts = transform.Find("ChargeForce").GetComponent<ParticleSystem>();
        _returnSphereParts = transform.Find("ReturnSphere").GetComponent<ParticleSystem>();

        _feet = transform.Find("Feet");
        _hands = transform.Find("Hands");
        _hands.gameObject.SetActive(false);

        _force1 = transform.Find("Force1").GetComponent<Force>();
        _force2 = transform.Find("Force2").GetComponent<Force>();
        _force3 = transform.Find("Force3").GetComponent<Force>();
    }

    // Use this for initialization
    void Start () {
        SphereLost();
	}
	
	// Update is called once per frame
	void Update () {
        force();
        move();
        rotate();
    }
    
    public void SphereLost()
    {
        _returnSphereParts.Play();
        _waitingForSphere = true;
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
            Vector2 direction;

            float horizontal = Input.GetAxis("RHorizontal");
            float vertical = Input.GetAxis("RVertical");
            Vector2 controllerDirection = new Vector2(horizontal, -vertical);

            if (controllerDirection.sqrMagnitude > 0.5)
            {
                direction = controllerDirection;

                _usingController = true;
                _previousMousePosition = Input.mousePosition;
            }
            else
            {
                if (_usingController)
                {
                    if (Vector3.Distance(_previousMousePosition, Input.mousePosition) < 10)
                        return;
                    _usingController = false;
                }

                direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }

            float angle = Mathf.Atan2(direction.x, direction.y);

            transform.rotation = Quaternion.Euler(0f, 0f, -angle*Mathf.Rad2Deg);
            //_rigidbody.MoveRotation(-angle * Mathf.Rad2Deg);
        }
    }

    private void force()
    {
        if (_waitingForSphere)
        {
            if (!_returnSphereParts.isPlaying)
            {
                ParticleSystem.MainModule mm = _returnSphereParts.main;
                mm.duration = ReturnSphereDuration;
                _waitingForSphere = false;
                createSphere();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(performForce());
            }
        }
    }

    private void createSphere()
    {
        Sphere sphere = Instantiate(Sphere, transform);
        sphere.transform.localPosition = new Vector3(0f, 0.5f);
    }

    private IEnumerator performForce()
    {
        _chargingForce = true;

        float duration = 0f;
        int forceLevel = 1;

        _spriteRenderer.color = _force1.SpriteRenderer.color;

        ParticleSystem.MainModule particles = _chargeForceParts.main;

        particles.startColor = _force1.SpriteRenderer.color;
        particles.startLifetime = new ParticleSystem.MinMaxCurve(0.25f);
        particles.startSpeed = new ParticleSystem.MinMaxCurve(1f);

        _chargeForceParts.Play();

        while (Input.GetButton("Fire1"))
        {
            if (_waitingForSphere)
                break;

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

        _chargeForceParts.Stop();
        _chargingForce = false;

        if (_waitingForSphere)
        {
            _spriteRenderer.color = Color.white;
            yield break;
        }

        _performingForce = true;

        switch (forceLevel)
        {
            case 1:
                _currentForce = _force1;
                break;
            case 2:
                _currentForce = _force2;
                break;
            case 3:
                _currentForce = _force3;
                break;
        }

        _hands.gameObject.SetActive(true);
        yield return _currentForce.Activate(0.2f);
        _hands.gameObject.SetActive(false);

        _spriteRenderer.color = Color.white;

        _currentForce = null;
        _performingForce = false;

    }
}
