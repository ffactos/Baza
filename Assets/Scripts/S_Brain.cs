using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Brain : MonoBehaviour
{
    [SerializeField]
    Transform cam;

    Interactable interactable;

    public GameObject inventory;

    [SerializeField]
    float interactDistance;

    Ray hand;

    private void Start()
    {
        //inventory = GameObject.FindGameObjectWithTag("Inventory");
    }

    private void Update()
    {
        hand = new Ray(cam.transform.position, cam.transform.forward);

        Physics.Raycast(hand, out RaycastHit hitInfoo, interactDistance);

        if(hitInfoo.collider != null)
            Debug.Log(hitInfoo.collider.gameObject.name);

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (!inventory.activeInHierarchy)
        //        inventory.SetActive(true);
        //    else
        //        inventory.SetActive(false);
        //}

        if (Physics.Raycast(hand, out RaycastHit hitInfo, interactDistance))
        {
            if (hitInfo.collider.gameObject.GetComponent<Interactable>() != null)
            {
                if (hitInfo.collider.gameObject.GetComponent<Interactable>() != interactable)
                {
                    if (interactable != null)
                        interactable.DisableOutline();
                    interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
                    interactable.EnableOutline();
                }
            }
            else
            {
                if (interactable != null)
                    interactable.DisableOutline();
                interactable = null;
            }
        }
        else
        {
            if (interactable != null)
                interactable.DisableOutline();
            interactable = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(hand);
    }
}
