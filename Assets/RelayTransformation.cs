using System;
using UnityEngine;

public class RelayTransformation : MonoBehaviour
{
    void TransformInto() 
    {
        TransformationManager.Instance.ApplyTransformation();
    }

    void ApplyAvatar() 
    {
        PlayerMovement.m_Animator.avatar = TransformationManager.currentForm.avatar;
    }
}
