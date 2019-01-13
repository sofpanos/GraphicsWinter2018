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
        Debug.Log("Pressed primary button.");
        GameObject.Find("Torch").GetComponent<TorchScript>().fireEnabled = true;
        animator.SetBool("ActivateDown", true);
        activated = true;
        PlaySound();
    }

    public void PlaySound()
    {
        audio.Play();
    }
}
