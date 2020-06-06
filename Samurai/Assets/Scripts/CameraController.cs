using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    float speed = 5.0f;
    public Transform target;
    private Vector3 gamePosition = new Vector3(0f, 12f, -19f);
    private Quaternion gameRotation = Quaternion.Euler(30f, 0f, 0f);

    private Vector3 menuPosition = new Vector3(-2f, 3f, -10f);
    private Quaternion menuRotation = Quaternion.Euler(10f, -12f, 0f);

    public Camera cam;

    public Color bgLight; //#F7F7F7
    public Color bgDark; //#3F3F44

    //public Transform ui;
    //private UIController uiScript;
    //public Transform system;
    //private SystemsController systemScript;

    void Start()
    {
        //uiScript = ui.GetComponent<UIController>();
        //systemScript = system.GetComponent<SystemsController>();

        //m_EventSystem = EventSystem.current;
        OpenMainMenu();
    }

    void Update()
    {

        transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Xbox X") * speed);
        if (Input.GetMouseButton(2))
        {
            transform.RotateAround(target.position, Vector3.up, Input.GetAxis("Mouse X") * speed);
        }
    }

    public void StartGame()
    {
        cam.orthographic = true;
        SetBGLight();
        transform.position = gamePosition;
        transform.rotation = gameRotation;
    }

    /*IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.1f);
    }*/

    public void ResumeGame()
    {
        cam.orthographic = true;
        transform.position = gamePosition;
        transform.rotation = gameRotation;
    }

    public void OpenMainMenu()
    {
        transform.position = menuPosition;
        transform.rotation = menuRotation;
        cam.backgroundColor = bgLight;
    }

    public void SetBGLight()
    {
        cam.backgroundColor = bgLight;
    }

    public void SetBGDark()
    {
        cam.backgroundColor = bgDark;
    }

    public void LerpBGDark(float t)
    {
        cam.backgroundColor = Color.Lerp(bgLight, bgDark, t);
    }

    public Transform GetCamTransform()
    {
        return cam.transform;
    }
}