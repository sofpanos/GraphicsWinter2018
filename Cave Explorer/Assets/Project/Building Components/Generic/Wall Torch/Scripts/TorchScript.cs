using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    public ParticleSystem firePS;
    public Light fireLight;

    public bool fireEnabled;

    // Start is called before the first frame update
    void Start()
    {
        firePS.Stop();
        fireLight.enabled = false;
        fireEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireEnabled)
        {
            if (!firePS.isPlaying)
            {
                firePS.Play();
                fireLight.enabled = true;
            }
        }
    }
}
