using System;
using UnityEngine;

public class RelayTransformation : MonoBehaviour
{
    void TransformInto() 
    {
        TransformationManager.Instance.ApplyTransformation();
    }
}
