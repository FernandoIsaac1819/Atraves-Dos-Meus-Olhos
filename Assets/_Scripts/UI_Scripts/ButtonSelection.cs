using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //Gets the button component and it controls what it does when it is selected
    [Header("Settings")]

    [Tooltip("Scale of the button when it's selected.")]
    [SerializeField] private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);

    [Tooltip("Speed at which the button scales to its target size.")]
    [SerializeField] private float scaleSpeed = 10f;


    private Vector3 originalScale;
    private Vector3 targetScale;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        targetScale = originalScale;
    }
    
    private void Update()
    {
        // Smoothly scale the button towards the target scale
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
