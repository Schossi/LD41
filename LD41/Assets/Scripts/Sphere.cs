using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

    private Player _player;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;

    private int _forceLevel = 0;

    private bool _fired = false;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();

        _player = GetComponentInParent<Player>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward, (50f + _forceLevel * 150f) * Time.deltaTime);

        if (!_fired && _player != null && _player.CurrentForce != null)
            applyForce(_player.CurrentForce);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Force"))
        {
            applyForce(collision.GetComponent<Force>());
        }
        else if (collision.tag.StartsWith("Enemy"))
        {
            hitEnemy(collision.GetComponentInParent<Enemy>());
        }
        else if (collision.tag.StartsWith("Village"))
        {
            lost();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.collider.tag.StartsWith("Enemy"))
        {
            hitEnemy(collision.collider.GetComponentInParent<Enemy>());
        }
        else if (collision.collider.CompareTag("Bounds"))
        {
            if (_forceLevel > 1)
                setForceLevel(_forceLevel - 1);
        }
    }

    private void applyForce(Force force)
    {
        if (!_fired)
        {
            _fired = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            transform.SetParent(null);
            _particleSystem.Play();
        }

        setForceLevel(force.Level);

        _rigidbody2D.velocity = force.transform.up * force.Power;
    }

    private void setForceLevel(int level)
    {
        _forceLevel = level;

        gameObject.layer = LayerMask.NameToLayer("SphereL" + _forceLevel.ToString());

        _spriteRenderer.color = Force.GetForceColor(_forceLevel);

        ParticleSystem.MainModule particles = _particleSystem.main;
        particles.startColor = _spriteRenderer.color;

    }

    private void hitEnemy(Enemy enemy)
    {
        enemy.Hit(_forceLevel, transform);
    }

    private void lost()
    {
        _player.SphereLost();
        StartCoroutine(destroyAfter(2f));
    }
    private IEnumerator destroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
