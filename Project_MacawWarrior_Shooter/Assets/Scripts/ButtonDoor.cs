using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour, IInteractable
{
    public GameObject door;
    public bool activate = false;
    float speed = 1f;
    public Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (activate)
            door.transform.Translate(transform.up * speed * Time.deltaTime);

        if (door.transform.position.y >= 10)
            activate = false;


    }
    public void Interact()
    {
        animator.SetTrigger("Press");
        activate = true;
    }
}
