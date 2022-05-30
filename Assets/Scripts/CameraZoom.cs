using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera camera;
    Vector3 touchStart;
    [SerializeField] float zoomOutMin = 5;
    [SerializeField] float zoomOutMax = 25;
    [SerializeField] float xMax;
    [SerializeField] float xMin;
    [SerializeField] float yMax;
    [SerializeField] float yMin;
    private Touch touchZero;
    private Touch touchOne;
    private Vector2 touchZeroPrevPos;
    private Vector2 touchOnePrevPos;
    private float prevMagnitude;
    private float currentMagnitude;
    private float difference;
    private Vector3 camPos;
    private Vector3 direction;
	
    // Update is called once per frame
    void Update () {
        camPos = camera.transform.position;
        camPos.z = -10;
        camera.transform.position = camPos;
        if (Utils.IsPointerOverUI()) return;
        if(Input.GetMouseButtonDown(0))
        {
            touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.touchCount == 2){
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.02f);
        }else if(Input.GetMouseButton(0)){
            direction = touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
        camPos = camera.transform.position;
        camPos.x = Mathf.Clamp(camPos.x, xMin, xMax);
        camPos.y = Mathf.Clamp(camPos.y, yMin, yMax);
        camera.transform.position = camPos;
    }

    void zoom(float increment){
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    public void DeZoomAnim()
    {
        float difference = 15 - camera.orthographicSize;
        float timeStep = 0.5f / 0.01f;
        float step = difference / timeStep;
        StartCoroutine(DeZoom(0.5f,step));
    }

    private WaitForSeconds dezoomTime = new WaitForSeconds(0.01f);
    IEnumerator DeZoom(float t,float step)
    {
        if (t < 0) yield break;
        camera.orthographicSize += step;
        t -= 0.01f;
        yield return dezoomTime;
        StartCoroutine(DeZoom(t,step));
    }
}