using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSwitchScript : MonoBehaviour
{
    private bool activated;
    private Animator animator;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLeverClick()
    {
        if (!activated)
        {
            animator.SetBool("ActivateDown", true);
            PlaySound();

            foreach (TorchScript script in transform.parent.GetComponentsInChildren<TorchScript>())
            {
                script.fireEnabled = true;
            }
            
            activated = true;
        }
    }

    public void PlaySound()
    {
        audio.Play();
    }
}
