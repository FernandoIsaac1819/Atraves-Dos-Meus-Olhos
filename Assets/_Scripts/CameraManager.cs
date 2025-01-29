using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [System.Serializable]
    public class CameraZone
    {
        public string zoneName;
        public CinemachineVirtualCamera virtualCamera;
        public Transform zoneCenter; // Center of the camera zone
        public float activationRadius;
    }

    public static CameraManager Instance;

    public List<CameraZone> cameraZones; // List of all camera zones

    private CinemachineVirtualCamera currentCamera;

    

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
        player = GameObject.FindGameObjectWithTag("Player");
        // Deactivate all cameras except the default one
        foreach (CameraZone zone in cameraZones)
        {
            zone.virtualCamera.Priority = 0;
        }

        if (cameraZones.Count > 0)
        {
            cameraZones[0].virtualCamera.Priority = 10; // Activate the first camera by default
            currentCamera = cameraZones[0].virtualCamera;
        }
    }

    private void Update()
    {
        CameraZone closestCamZone = GetClosestZone();

        if (closestCamZone != null && closestCamZone.virtualCamera != currentCamera)
        {
            SwitchCamera(closestCamZone.virtualCamera);
        }
    }

    private CameraZone GetClosestZone()
    {
        CameraZone closestZone = null;

        float closestDistance = float.MaxValue;

        foreach (CameraZone cam in cameraZones)
        {
            float distance = Vector3.Distance(player.transform.position, cam.zoneCenter.position);

            if (distance < cam.activationRadius && distance < closestDistance)
            {
                closestZone = cam;
                closestDistance = distance;
            }
        }

        return closestZone;
    }

    private void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        if (currentCamera != null)
        {
            currentCamera.Priority = 0; // Deactivate the current camera
        }

        newCamera.Priority = 10; // Activate the new camera
        currentCamera = newCamera; // Update the reference
    }

    private void OnDrawGizmos()
    {
        if (cameraZones == null) return;

        foreach (CameraZone zone in cameraZones)
        {
            if (zone.zoneCenter != null)
            {
                // Set Gizmo color for each zone
                Gizmos.color = Color.green;

                // Draw a wireframe sphere for the activation radius
                Gizmos.DrawWireSphere(zone.zoneCenter.position, zone.activationRadius);
            }
        }
    }
}