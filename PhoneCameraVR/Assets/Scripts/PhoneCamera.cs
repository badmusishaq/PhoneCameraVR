using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    public TextMeshProUGUI debugText;

    private string message;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            message = "No Camera Detected!";
            debugText.text = message;
            Debug.Log(message);
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            message = "Unable to find a back camera!";
            debugText.text = message;
            Debug.Log(message);
            return;
        }
        
        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
        {
            /*message = Application.targetFrameRate.ToString();
            debugText.text = message;
            Debug.Log(message);*/
            return;
        }

        float ratio = (float) backCam.width / (float) backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1, scaleY, 1);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
