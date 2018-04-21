using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static Color Level1Color = Color.green;
    public static Color Level2Color = Color.blue;
    public static Color Level3Color = Color.red;

    public int Level;
    public static float Speed = 0.1f;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private Transform _bloodPivot;
    private ParticleSystem _bloodParticles;

    private bool _standing = true;
    private bool _dead = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _bloodPivot = transform.Find("BloodPivot");
        _bloodParticles = _bloodPivot.GetComponentInChildren<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {
        //switch (Level)
        //{
        //    case 1:
        //        _spriteRenderer.color = Level1Color;
        //        break;
        //    case 2:
        //        _spriteRenderer.color = Level2Color;
        //        break;
        //    case 3:
        //        _spriteRenderer.color = Level3Color;
        //        break;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead || _standing)
            return;

        move();
    }

    private void move()
    {
        transform.position += -transform.up * Speed * Time.deltaTime;

        //_rigidbody.MovePosition(_rigidbody.position + movement);
    }

    public void StartMoving()
    {
        _standing = false;

        if (_animator != null)
            _animator.SetBool("move", true);
    }

    public void Hit(int level, Transform origin)
    {
        if (level >= Level)
        {
            die(origin);
        }
    }

    private void die(Transform origin)
    {
        _dead = true;

        Vector2 direction = (transform.position - origin.position).normalized;

        _rigidbody.simulated = false;

        //_bloodPivot.LookAt(origin.position);//particles dont turn with pivot :(
        _bloodParticles.Play();

        _animator.SetBool("dead", true);
        StartCoroutine(Fade(direction));
    }

    private IEnumerator Fade(Vector2 direction)
    {
        float current = 0f;

        while (current <= 1f)
        {
            transform.position = transform.position + (Vector3)direction * Time.deltaTime;
            _spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, current));
            current += Time.deltaTime * 2f;
            yield return null;
        }

        Destroy(gameObject);
    }
}
