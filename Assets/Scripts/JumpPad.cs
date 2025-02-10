using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_JumpPad : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(0, collision.gameObject.GetComponent<S_Movement>().jumpForce * 3, 0);
            collision.gameObject.GetComponentInParent<S_Movement>().curJumps = collision.gameObject.GetComponentInParent<S_Movement>().maxJumps;
            collision.gameObject.GetComponentInParent<S_Movement>().jumpCounter.DOComplete();
            collision.gameObject.GetComponentInParent<S_Movement>().jumpCounter.gameObject.GetComponent<RectTransform>().DOComplete();
            collision.gameObject.GetComponentInParent<S_Movement>().jumpCounter.gameObject.GetComponent<RectTransform>().DOShakePosition(1.5f, 30, 50, 360, false, true, ShakeRandomnessMode.Harmonic);
            collision.gameObject.GetComponentInParent<S_Movement>().jumpCounter.fillRect.gameObject.GetComponent<Animation>().Play();
            collision.gameObject.GetComponentInParent<S_Movement>().jumpCounter.DOValue(collision.gameObject.GetComponentInParent<S_Movement>().maxJumps, 0f);

            foreach (var m in GetComponents<DOTweenAnimation>())
            {
                m.DORestart();
            }
        }
    }
}
