using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteraction;
    public ParticleSystem psystem;
    public bool pickup;

    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public void EnableOutline()
    {
        outline.enabled = true;
    }


    public void DisableOutline()
    {
        outline.enabled = false;
    }
    public void Interact()
    {
        onInteraction.Invoke();

        if (pickup)
        {
            Destroy(gameObject);
            if (psystem != null)
                Instantiate(psystem, new Vector3(transform.position.x, transform.position.y - .75f, transform.position.z), psystem.transform.rotation);
        }
    }
}
