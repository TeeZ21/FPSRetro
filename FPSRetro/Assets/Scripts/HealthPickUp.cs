using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private int _healthAmount = 20;
    [SerializeField] private Animator _healthPickUp;
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && PlayerController._playerInstance._currentHealth != PlayerController._playerInstance._maxHealth)
        {
            PlayerController._playerInstance.AddHealth(_healthAmount);
            _healthPickUp.SetTrigger("GetHeal");
            Destroy(gameObject, .2f);
        }
    }
}
