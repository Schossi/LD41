using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

    private Player _player;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;

    private Transform _sprite;

    private int _forceLevel = 0;

    private bool _fired = false;
    
    private AudioSource[] _audioSources;

    void Awake()
    {
        _sprite = transform.Find("Sprite");

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = _sprite.GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();

        _player = GetComponentInParent<Player>();

        _audioSources = GetComponents<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _sprite.Rotate(Vector3.forward, (50f + _forceLevel * 150f) * Time.deltaTime);

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
        if (collision.collider.CompareTag("Boss"))
        {
            hitBoss(collision.collider.GetComponentInParent<Boss>());
        }
        else if (collision.collider.CompareTag("Bounds"))
        {
            playSound(0);
            if (_forceLevel > 1)
                ApplyForce(_forceLevel - 1, _rigidbody2D.velocity.normalized);
        }
    }

    private void applyForce(Force force)
    {
        ApplyForce(force.Level, force.transform.up);
    }

    public void ApplyForce(int level,Vector3 direction)
    {
        if (!_fired)
        {
            _fired = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            transform.SetParent(null);
            transform.localRotation = Quaternion.identity;
            _particleSystem.Play();
        }

        _forceLevel = level;

        gameObject.layer = LayerMask.NameToLayer("SphereL" + _forceLevel.ToString());

        _spriteRenderer.color = global::Force.GetForceColor(_forceLevel);

        ParticleSystem.MainModule particles = _particleSystem.main;
        particles.startColor = _spriteRenderer.color;

        _rigidbody2D.velocity = direction * global::Force.GetForcePower(_forceLevel);
    }

    private void hitEnemy(Enemy enemy)
    {
        if (enemy.Hit(_forceLevel, transform))
            playSound(1);
        else
            playSound(0);
    }

    private void hitBoss(Boss boss)
    {
        if (_forceLevel == 3)
        {
            boss.Hit();
            playSound(1);
        }
        else
        {
            playSound(0);
        }
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
    
    private void playSound(int index)
    {
        if (GameManager.Instance.SoundEnabled)
            _audioSources[index].Play();
    }
}
