using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Idle = Animator.StringToHash("Idle");
    public GameObject animal;


    void Start()
    {
        _animator = animal.GetComponent<Animator>();
        _animator.SetTrigger(Walk);
    }
}
