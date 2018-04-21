using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.collider.tag.StartsWith("Enemy"))
        {
            hitEnemy(collision.collider.GetComponentInParent<Enemy>());
        }
    }

    private void applyForce(Force force)
    {
        if (!_fired)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            transform.SetParent(null);
            _particleSystem.Play();
        }

        gameObject.layer = LayerMask.NameToLayer("SphereL" + force.Level.ToString());

        _forceLevel = force.Level;

        _spriteRenderer.color = force.SpriteRenderer.color;

        ParticleSystem.MainModule particles = _particleSystem.main;
        particles.startColor = _spriteRenderer.color;

        _rigidbody2D.velocity = force.transform.up * force.Power;
    }

    private void hitEnemy(Enemy enemy)
    {
        enemy.Hit(_forceLevel, transform);
    }
}
