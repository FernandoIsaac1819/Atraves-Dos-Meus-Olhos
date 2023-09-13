using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public PlayerMovement m_PlayerMovement;

    void Start() 
    {
        m_PlayerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision other) 
    {

    }
}
