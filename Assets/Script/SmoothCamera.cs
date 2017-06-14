using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    private Camera myCam;

    private bool smooth = true;


    [SerializeField]
    private float smoothSpeed = 0.125f;
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
    }

    //Zoom
    public void ZoomCamera(bool _state)
    {
        if (!_state)
        {
            if (myCam.orthographicSize <= 5f)
                myCam.orthographicSize += .01f;
        }
        else
        {
            if (myCam.orthographicSize >= 4.5f)
                myCam.orthographicSize -= .01f;
        }
    }
}
