using System;
using System.Text.RegularExpressions;
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

            /* For testing the sample scene
            GameObject.Find("RockWallWithTorch").GetComponent<TorchScript>().fireEnabled = true;
            int i = 1;
            while (true)
            {
                GameObject tempObj = GameObject.Find("RockWallWithTorch (" + i + ")");
                if (tempObj == null)
                    break;
                tempObj.GetComponent<TorchScript>().fireEnabled = true;
                i++;
            }
            GameObject.Find("MountainWallWithTorch").GetComponent<TorchScript>().fireEnabled = true;*/
        }
    }

    public void PlaySound()
    {
        audio.Play();
    }
}
