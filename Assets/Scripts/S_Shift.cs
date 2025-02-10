using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class S_Shift : MonoBehaviour
{
    public Camera cam;
    public Rigidbody rb = null;

    public float shiftCD = 1f;
    public float shiftDuration;
    public float shiftSpeed;
    public float shiftTime;

    public Vector3 cameraDirFront;
    private Vector3 cameraDirRight;
    private Vector3 cameraRot;
    private Vector3 input;

    private float xAxis;
    private float zAxis;

    private float x;
    private float z;

    public int curShifts;
    public int maxShifts = 3;

    public bool isShifting;

    public UnityEngine.UI.Slider shiftCounter;
    public UnityEngine.UI.Slider background;

    private void Awake() 
    {
        curShifts = maxShifts;

        shiftCounter = GameObject.FindGameObjectWithTag("ShiftCounter").GetComponent<UnityEngine.UI.Slider>();
        background = GameObject.Find("Shift Count Background").GetComponent<UnityEngine.UI.Slider>();

        shiftCounter.maxValue = maxShifts;
        shiftCounter.value = curShifts;

        background.maxValue = maxShifts;
        background.value = curShifts;
    }

    private void LateUpdate()
    {

        //if(rb == null)
        rb = GetComponent<Rigidbody>();

        GetCameraRotation();

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (shiftCD > shiftTime)
            shiftTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && curShifts > 0)
        {
            curShifts--;
            shiftCounter.DOValue(curShifts, 1f, false).SetEase(Ease.OutExpo);
            background.DOComplete();
            shiftTime = 0f;
            xAxis = input.x;
            zAxis = input.z; 


            StartCoroutine(Shift());
        }

        background.value = shiftTime / shiftCD + curShifts;

        if (shiftCD <= shiftTime)
        {
            if (curShifts < maxShifts)
            {
                shiftTime = 0f;
                curShifts++;
                shiftCounter.DOValue(curShifts, 1f, false).SetEase(Ease.OutExpo);
            }
        }
    }

    private void FixedUpdate()
    {

        if (input.normalized.magnitude > .1f)
        {
            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        if (isShifting)
        {
            rb.linearVelocity = new Vector3(shiftSpeed * Time.deltaTime * input.x, 0, shiftSpeed * Time.deltaTime * input.z);
        }
    }

    IEnumerator Shift()
    {
        isShifting = true;
        yield return new WaitForSecondsRealtime(shiftDuration);
        isShifting = false;
    }

    private void GetCameraRotation()
    {
        cameraRot = cam.transform.rotation.eulerAngles;
        cameraDirFront = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        cameraDirRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);
        input = new Vector3(x, 0, z);
        input = cameraDirFront.normalized * input.z + cameraDirRight.normalized * input.x;
    }

}