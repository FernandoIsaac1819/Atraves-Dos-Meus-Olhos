using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField] private string m_EntranceIdentifier; 

    void Start()
    {
        string lastExit = PlayerPrefs.GetString("LastExit", string.Empty);

        if (lastExit == m_EntranceIdentifier)
        {
            PlayerMovement.instance.transform.position = transform.position;
            PlayerMovement.instance.transform.eulerAngles = transform.eulerAngles;
        }
    }
}
