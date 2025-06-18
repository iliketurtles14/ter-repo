using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NoTurning : MonoBehaviour
{


    private Rigidbody2D RigidBody2D;


    void Start()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RigidBody2D.angularVelocity = 0;
        RigidBody2D.freezeRotation = true;
    }
}
