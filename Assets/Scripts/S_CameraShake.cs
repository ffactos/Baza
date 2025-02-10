using Unity.Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using System.Linq;

public class S_CameraShake : MonoBehaviour
{

    [SerializeField]
    S_Movement player;

    CanvasGroup inventory;

    float lastDutch;
    [SerializeField]
    float changeDutch = .5f;
    [SerializeField]
    float powerDutch = 5f;

    float lastFOV;
    [SerializeField]
    float changeFOV = .5f;

    float zoomOffset = 0f;
    [SerializeField]
    float zoomAccelerationPower = 2.5f;

    CinemachineInputAxisController pov;
    CinemachineCamera cinemachine;

    
    private void Update()
    {
        cinemachine.Lens.Dutch = Mathf.Lerp(lastDutch, -player.xAxis * powerDutch, changeDutch);
        lastDutch = Mathf.Lerp(lastDutch, -player.xAxis * powerDutch, changeDutch);

        if (inventory.interactable)
            foreach (var item in pov.Controllers)
                item.Enabled = false;
        else
            foreach(var item in pov.Controllers)
                item.Enabled = true;

        if (Input.mouseScrollDelta.y > 0 && zoomOffset < 30f)
        {
            zoomOffset += 5f;
        }
        if (Input.mouseScrollDelta.y < 0 && zoomOffset > -10f)
        {
            zoomOffset -= 5f;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            cinemachine.Lens.FieldOfView = Mathf.Lerp(lastFOV, 50f, changeFOV);
            lastFOV = Mathf.Lerp(lastFOV, 50f - zoomOffset, changeFOV);
            if(zoomAccelerationPower != 0)
            {
                cinemachine.GetComponent<CinemachineInputAxisController>().Controllers.Last().Driver.AccelTime = zoomOffset / zoomAccelerationPower;
                cinemachine.GetComponent<CinemachineInputAxisController>().Controllers.Last().Driver.DecelTime = zoomOffset / zoomAccelerationPower;
            }
        }

        else
        {
            cinemachine.Lens.FieldOfView = Mathf.Lerp(lastFOV, 90 + player.rb.linearVelocity.magnitude, changeFOV);
            lastFOV = Mathf.Lerp(lastFOV, 90 + player.rb.linearVelocity.magnitude, changeFOV);
            zoomOffset = 0f;
            cinemachine.GetComponent<CinemachineInputAxisController>().Controllers.Last().Driver.AccelTime = 0f;
            cinemachine.GetComponent<CinemachineInputAxisController>().Controllers.Last().Driver.DecelTime = 0f;

        }
    }

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>();
        cinemachine = GetComponent<CinemachineCamera>();
        cinemachine.Target.TrackingTarget = GameObject.Find("Camera System").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform.gameObject.GetComponent<S_Movement>();
        lastDutch = -player.xAxis * 5f;
        pov = cinemachine.GetComponent<CinemachineInputAxisController>();

    }

}
