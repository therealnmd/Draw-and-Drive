using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float jumpForce = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CarController car = collision.GetComponent<CarController>();
            if (car != null)
            {
                car.JumpPad(jumpForce);
            }
        }
    }

}
