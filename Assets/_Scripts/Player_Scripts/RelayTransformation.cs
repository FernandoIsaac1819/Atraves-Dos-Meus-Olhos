using System;
using UnityEngine;

public class RelayTransformation : MonoBehaviour
{
    void TransformInto() 
    {
        TransformationManager.Instance.ApplyTransformation();   
    }

    void ChangeAvatar() 
    {
        PlayerMovement.Instance.PlayerAnimator.avatar = TransformationManager.Instance.currentForm.avatar;
    }
}
