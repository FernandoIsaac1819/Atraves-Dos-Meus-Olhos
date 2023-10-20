using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField] private SceneField m_EntranceIdentifier; 

    void Start()
    {
        string lastExit = PlayerPrefs.GetString("LastExit", string.Empty);

        if (lastExit == m_EntranceIdentifier)
        {
            PlayerMovement.Instance.transform.position = transform.position;
            PlayerMovement.Instance.transform.eulerAngles = transform.eulerAngles;
        }
    }
}
