using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Controls the face of the cat with the mouse object
/// </summary>
public class ControlCatFace : MonoBehaviour
{
    public RectTransform m_CatBody;
    public RectTransform m_CatFace;
    public Camera m_UICamera;

    [Header("Movement Range")]
    [SerializeField] private Vector2 m_MaxOffset = new Vector2(14, 14);
    [SerializeField] private float m_Sensitivity = 0.5f;
    [SerializeField] private float m_ReturnSpeed = 2f;
    public Vector2 m_CenterPosition;

    private void Start()
    {
        m_CenterPosition = m_CatFace.anchoredPosition;
    }

    private void Update()
    {
        Vector2 Input = HandleInputs.Instance.PlayerInputActions.UI.Cat.ReadValue<Vector2>();

        // Calculate the target position based on left stick input
        Vector2 targetPosition = new Vector2(
            Mathf.Lerp(-m_MaxOffset.x, m_MaxOffset.x, Input.x * m_Sensitivity),
            Mathf.Lerp(-m_MaxOffset.y, m_MaxOffset.y, Input.y * m_Sensitivity)
        );

        // Smoothly move the cat face towards the target position or back to the center position
        m_CatFace.anchoredPosition = Vector2.Lerp(m_CatFace.anchoredPosition, m_CenterPosition + targetPosition, Time.deltaTime * m_ReturnSpeed);

        // Clamp the cat face position within the boundaries
        m_CatFace.anchoredPosition = new Vector2(
            Mathf.Clamp(m_CatFace.anchoredPosition.x, m_CenterPosition.x - m_MaxOffset.x, m_CenterPosition.x + m_MaxOffset.x),
            Mathf.Clamp(m_CatFace.anchoredPosition.y, m_CenterPosition.y - m_MaxOffset.y, m_CenterPosition.y + m_MaxOffset.y)
        );
    }

}
