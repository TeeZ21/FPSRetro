using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private int _ammoAmount = 20;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController._playerInstance._currentAmmo += _ammoAmount;
            PlayerController._playerInstance.UpdateAmmoUI();

            Destroy(gameObject);
        }
    }
}
