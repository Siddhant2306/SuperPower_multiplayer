using UnityEditor;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public Camera maincamera;
    public Camera debugcam;

    void Start()
    {
        maincamera.enabled = false;
        debugcam.enabled = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            maincamera.enabled = !maincamera.enabled;
            debugcam.enabled = !debugcam.enabled;
        }
    }
}