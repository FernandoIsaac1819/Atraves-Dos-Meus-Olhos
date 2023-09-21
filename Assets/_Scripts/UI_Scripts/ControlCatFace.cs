using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the face of the cat with the mouse object
/// </summary>
public class ControlCatFace : MonoBehaviour
{
    public RectTransform catBody; 
    public RectTransform catFace; 
    public Camera uiCamera;

    [Header("Movement Range")]
    [SerializeField] private Vector2 maxOffset = new Vector2(40f, 18f); 
    [SerializeField] private float m_Sensitivity = 0.5f;
    private Vector2 initialFacePosition; 

    private void Start()
    {
        initialFacePosition = catFace.anchoredPosition;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(catBody, mousePosition, uiCamera, out localMousePosition);

        Vector2 normalizedPosition = new Vector2(
            Mathf.Clamp01((localMousePosition.x + catBody.rect.width * m_Sensitivity) / catBody.rect.width),
            Mathf.Clamp01((localMousePosition.y + catBody.rect.height * m_Sensitivity) / catBody.rect.height)
        );

        Vector2 newPosition = new Vector2(
            Mathf.Lerp(-maxOffset.x, maxOffset.x, normalizedPosition.x),
            Mathf.Lerp(-maxOffset.y, maxOffset.y, normalizedPosition.y)
        );

        catFace.anchoredPosition = initialFacePosition + newPosition;
    }
}
