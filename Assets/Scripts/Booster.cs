using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Booster : MonoBehaviour
{

    public string type;
    [SerializeField]
    GameObject child;
    GameObject enteredObject;
    Animation boostedAnim;

    public bool isActive = true;

    public float cooldown = 7f;

    private void OnTriggerEnter(Collider other)
    {

        if (isActive)
        {

            switch (type)
            {

                case "Jumper":
                    if (other.gameObject.GetComponentInParent<S_Shift>() != null)
                    {
                        other.gameObject.GetComponentInParent<S_Shift>().curShifts = other.gameObject.GetComponentInParent<S_Shift>().maxShifts;
                        DOTween.CompleteAll();
                        other.gameObject.GetComponentInParent<S_Shift>().shiftCounter.gameObject.GetComponent<RectTransform>().DOShakePosition(1.5f, 20, 20, 360, false, true, ShakeRandomnessMode.Harmonic);
                        other.gameObject.GetComponentInParent<S_Shift>().shiftCounter.fillRect.gameObject.GetComponent<Animation>().Play();
                        other.gameObject.GetComponentInParent<S_Shift>().shiftCounter.DOValue(other.gameObject.GetComponentInParent<S_Shift>().maxShifts, 0f);

                        StartCoroutine(Cooldown(cooldown));

                    }
                    break;

            }

        }

        
    }

    public IEnumerator Cooldown(float sec)
    {
        child.GetComponent<Renderer>().material.DOColor(new Color(1, 1, 1, .3f), 0f);
        isActive = false;
        yield return new WaitForSecondsRealtime(sec);
        child.GetComponent<Renderer>().material.DOColor(new Color(1, 1, 1, 1f), 0f);
        isActive = true;
    }

}
