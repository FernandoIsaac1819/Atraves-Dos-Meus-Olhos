using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformationManager : MonoBehaviour
{
    public static TransformationManager Instance;

    [SerializeField] private Animator TransformationAnim;
    [SerializeField] private List<TransformationBase_SO> transformationForms = new List<TransformationBase_SO>();
    
    public static TransformationBase_SO currentForm;
    private TransformationBase_SO selectedForm;
    private int currentIndex = 0;

    private GameObject currentModel;  // ✅ The currently active model
    private Transform modelParent;    // ✅ Parent where models are stored

    private bool canTransform = true;
    [SerializeField] private float transformationCooldown = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentForm = transformationForms[0];
        selectedForm = currentForm;

        // ✅ Find where to store models (inside Player)
        modelParent = PlayerMovement.Instance.transform.Find("Visuals holder");
        if (modelParent == null)
        {
            modelParent = new GameObject("Visuals holder").transform;
            modelParent.SetParent(PlayerMovement.Instance.transform);
        }

        // ✅ Spawn the initial model
        SwapModel(currentForm);

        HandleInputs.Instance.OnTransform_Pressed += OnTransformPressed;
        HandleInputs.Instance.OnNextFormPressed += OnNextFormPressed;
    }

    public void ApplyTransformation()
    {
        if (!canTransform) return;
        canTransform = false;

        // ✅ Swap model & update player stats
        PlayerMovement.Instance.UpdateFormParameters(selectedForm);
        SwapModel(selectedForm);

        // ✅ Set cooldown
        Invoke(nameof(ResetCooldown), transformationCooldown);
    }

    private void SwapModel(TransformationBase_SO form)
    {
        if (currentModel != null) Destroy(currentModel); // ✅ Destroy the old model

        // ✅ Instantiate new model inside "ModelParent"
        currentModel = Instantiate(form.prefab, modelParent);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        // ✅ Rebind Animator to fix animation issue
        RebindAnimator(form);
    }

    private void RebindAnimator(TransformationBase_SO form)
    {
        Animator playerAnimator = PlayerMovement.Instance.GetComponent<Animator>();

        if (form.avatar != null)
        {
            playerAnimator.avatar = form.avatar;
        }

        if (form.animatorController != null)
        {
            playerAnimator.runtimeAnimatorController = form.animatorController;
        }

        // ✅ Force Unity to reset the animation bindings
        playerAnimator.Rebind();
        playerAnimator.Update(0);
    }

    public void CycleNextForm()
    {
        if (transformationForms.Count == 0) return;
        currentIndex = (currentIndex + 1) % transformationForms.Count;
        selectedForm = transformationForms[currentIndex];
    }

    private void ResetCooldown() => canTransform = true;
    private void OnNextFormPressed(object sender, EventArgs e) => CycleNextForm();
    private void OnTransformPressed(object sender, EventArgs e) => TransformationAnim.SetTrigger(AnimationHashCodes.Instance.TransformInto);
}
