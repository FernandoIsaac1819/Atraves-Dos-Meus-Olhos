using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the scaling of a button when it's selected or deselected.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Settings")]
    [Tooltip("Scale of the button when it's selected.")]
    [SerializeField] private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);
    [Tooltip("Speed at which the button scales to its target size.")]
    [SerializeField] private float scaleSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Button button;

    /// <summary>
    /// Initializes the button and sets the original scale.
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    
    /// <summary>
    /// Smoothly scales the button towards the target scale.
    /// </summary>
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    public void OnSelect(BaseEventData eventData)
    {
        targetScale = selectedScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetScale = originalScale;
    }
}
