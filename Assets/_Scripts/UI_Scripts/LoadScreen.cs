using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LoadScreen : MonoBehaviour
{
    public static LoadScreen Instance {get; private set;}
    public Animator animator;
    public CanvasGroup canvas;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Image[] fillImages;
    [SerializeField] private GameObject[] loadScreens;
    [SerializeField] private float fillSpeed = 0.25f;

    private float targetFill = 0;
    private int randomIndex;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canvas.alpha = 0;
        LevelManager.Instance.OnLoadingProgress += UpdateLoadingUI;
    }

    private void Update()
    {
        if (fillImages[randomIndex] != null)
        {
            float currentFill = Mathf.MoveTowards(fillImages[randomIndex].fillAmount, targetFill, Time.deltaTime * fillSpeed);
            UpdatePercentageText(currentFill);
        }
    }

    private void UpdateLoadingUI(float progress)
    {
        targetFill = progress;
        SetLoadScreen();
    }

    private void SetLoadScreen()
    {
        randomIndex = Random.Range(0, loadScreens.Length);

        for (int i = 0; i < loadScreens.Length; i++)
        {
            if (loadScreens[i] != null)
            {
                loadScreens[i].SetActive(i == randomIndex);
            }
        }
    }

    private void UpdatePercentageText(float fillAmount)
    {
        fillImages[randomIndex].fillAmount = fillAmount;
        int percent = Mathf.RoundToInt(fillAmount * 100);
        percentageText.text = percent + "%";
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnLoadingProgress -= UpdateLoadingUI;
    }
}