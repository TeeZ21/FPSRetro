using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int _health = 5;
    public GameObject _explosion;
    [SerializeField] private float _playerRange = 10f;
    [SerializeField] Rigidbody2D _rb = null;
    [SerializeField] private float _moveSpeed;
    private bool _shouldShoot;
    [SerializeField] float _fireRate = .5f;
    private float _shotCounter;
    [SerializeField] private GameObject _bullet = null;
    [SerializeField] private Transform _firePoint = null;

    void Start()
    {
        _shouldShoot = true;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, PlayerController._playerInstance.transform.position) < _playerRange)
        {
            Vector3 playerDirection = PlayerController._playerInstance.transform.position - transform.position;
            _rb.velocity = playerDirection.normalized * _moveSpeed;

            if(_shouldShoot)
            {
                _shotCounter -= Time.deltaTime;

                if(_shotCounter <= 0)
                {
                    Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
                    _shotCounter = _fireRate;
                }
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
   
    public void TakeDamage()
    {
        _health--;
        if(_health <= 0)
        {
            Destroy(gameObject);
            Instantiate(_explosion, transform.position, transform.rotation);
        }
    }
}
