using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Settings")]
    [SerializeField] private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private float scaleSpeed = 2f;

    private Vector3 originalScale;
    private bool isSelected;
    private float pingPongTime;

    private void Awake()
    {
        originalScale = transform.localScale;
        isSelected = false;
    }

    private void Update()
    {
        if (isSelected)
        {
            pingPongTime += Time.deltaTime * scaleSpeed;
            float scaleValue = Mathf.PingPong(pingPongTime, 1f);
            transform.localScale = Vector3.Lerp(originalScale, selectedScale, scaleValue);
        }
        else
        {
            // Reset the button scale to the original scale when deselected
            transform.localScale = originalScale;
            pingPongTime = 0f;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }
}

