using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public float boostForce = 5f;
    public float duration = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CarController car = collision.GetComponent<CarController>();
            if (car != null)
            {
                car.Boost(boostForce, duration);
            }
        }
    }

}
