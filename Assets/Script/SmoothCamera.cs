using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    private Camera myCam;

    private bool smooth = true;
    private bool blurState = false;

    [SerializeField]
    private GameObject blur;

    [SerializeField]
    private float smoothSpeed = 0.125f;
    private float blurTime = 0f;

    private Vector3 offset = new Vector3(0, 0, -10f);

    private void Start()
    {
        myCam = this.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;

        if (smooth)
        {
            this.transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }

        if(blurState)
        {
            blurTime += Time.deltaTime;

            if(blurTime >= .5f)
            {
                OffBlur();
                blurTime = 0f;
            }
        }
    }

    //Zoom
    public void ZoomCamera(bool _state)
    {
        if (!_state)
        {
            if (myCam.orthographicSize >= 4f)
                myCam.orthographicSize -= .02f;
        }
        else
        {
            if (myCam.orthographicSize <= 6f)
                myCam.orthographicSize += .05f;
        }

        float blurSize = myCam.orthographicSize * 0.2f;
        blur.transform.localScale = new Vector3(blurSize, blurSize, 1f);
    }

    public void OnBlur()
    {
        blurState = true;
        blur.SetActive(true);
    }

    private void OffBlur()
    {
        blur.transform.FindChild("1").GetComponent<TweenAlpha>().ResetToBeginning();
        blur.transform.FindChild("2").GetComponent<TweenAlpha>().ResetToBeginning();
        blur.transform.FindChild("1").GetComponent<TweenAlpha>().Play();
        blur.transform.FindChild("2").GetComponent<TweenAlpha>().Play();

        blur.SetActive(false);
        blurState = false;
    }
}
