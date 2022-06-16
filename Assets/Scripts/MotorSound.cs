using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSound : MonoBehaviour
{
    public AudioSource waterSplashAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WaterCircle")
        {
            waterSplashAudio.Play();
        }
    }
}
