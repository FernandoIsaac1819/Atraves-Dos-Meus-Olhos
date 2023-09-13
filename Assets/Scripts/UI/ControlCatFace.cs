using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCatFace : MonoBehaviour
{
    public RectTransform catBody; // Reference to the cat body RectTransform
    public RectTransform catFace; // Reference to the cat face RectTransform

    [Header("Movement Range")]
    public Vector2 maxOffset = new Vector2(40f, 18f); // Maximum offset from the initial position

    private Vector2 initialFacePosition; // Initial position of the cat face relative to cat body

    private void Start()
    {
        initialFacePosition = catFace.anchoredPosition; // Store the initial position of the cat face
    }

    private void Update()
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to local coordinates within the cat body's RectTransform
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(catBody, mousePosition, null, out localMousePosition);

        // Normalize the local mouse position within the cat body's size
        Vector2 normalizedPosition = new Vector2(
            Mathf.Clamp01((localMousePosition.x + catBody.rect.width * 0.5f) / catBody.rect.width),
            Mathf.Clamp01((localMousePosition.y + catBody.rect.height * 0.5f) / catBody.rect.height)
        );

        // Calculate the new position for the cat face within the user-defined movement range
        Vector2 newPosition = new Vector2(
            Mathf.Lerp(-maxOffset.x, maxOffset.x, normalizedPosition.x),
            Mathf.Lerp(-maxOffset.y, maxOffset.y, normalizedPosition.y)
        );

        // Update the anchored position of the cat face
        catFace.anchoredPosition = initialFacePosition + newPosition;
    }

}
