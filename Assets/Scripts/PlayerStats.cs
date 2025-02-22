using DG.Tweening;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStats : MonoBehaviour
{
    float healElapse;
    public float dmg;
    public float heal;
    public float healSpeed;
    public float speed = 3.5f;
    public float attackInterval;
    public float attackDistance;
    public float armor;
    public float locateDistance;

    public float timeAfterGettingDamaged;
    public float timeToStartHeal;

    public bool canHeal = true;

    public float maxHealth = 100f;
    public float health;

    public float lastCa;
    public bool right;
    ChromaticAberration ca;

    public UnityEngine.UI.Slider healthCounter;
    public UnityEngine.UI.Slider background;

    //Currency
    public int blueCrystals;

    private void Start()
    {
        health = maxHealth;
        healthCounter = GameObject.FindGameObjectWithTag("HealthCounter").GetComponent<UnityEngine.UI.Slider>();
        background = GameObject.Find("HP Count Background").GetComponent<UnityEngine.UI.Slider>();
        healthCounter.maxValue = maxHealth;
        healthCounter.value = health;
        background.maxValue = maxHealth;
        background.value = health;
        //ca = GameObject.FindGameObjectWithTag("Virtual Camera").GetComponent<CinemachineVolumeSettings>().Profile.components.Last().GetComponent<ChromaticAberration>();
        }

        private void Update()
    {
        background.value = Mathf.Lerp(background.value, health, .005f);

        healElapse += Time.deltaTime;
        timeAfterGettingDamaged += Time.deltaTime;
        if (timeAfterGettingDamaged > timeToStartHeal)
            canHeal = true;
        if (healElapse > healSpeed / 10)
        {
            healElapse = 0;
            Heal(heal / 10);
        }


        if (health < maxHealth / 100 * 20)
        {
            if (right)
            {
                ca.intensity.value = Mathf.Lerp(ca.intensity.value, ca.intensity.value + .5f, .1f);
                if (ca.intensity.value > 1 + maxHealth / 100 * 20 - health)
                {
                    right = false;
                }
            }
            else
            {
                ca.intensity.value = Mathf.Lerp(ca.intensity.value, ca.intensity.value - .5f, .1f);
                if (ca.intensity.value < .1f)
                {
                    right = true;
                }
            }
        }
        else
        {
            //ca.intensity.value = 0f;
        }
    }

    public void Damage(float dmg)
    {
        if (0 > health - dmg)
        {
            Die();
            health = 0;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }
        else
        {
            health -= dmg;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }

        healthCounter.gameObject.GetComponent<RectTransform>().DOComplete();
        healthCounter.gameObject.GetComponent<RectTransform>().DOShakePosition(.5f, 15f, 1000, 180, false, true, ShakeRandomnessMode.Harmonic);

        timeAfterGettingDamaged = 0f;
        canHeal = false;

        Debug.Log("Got hit");
    }

    public void Heal(float heal)
    {
        if (canHeal)
        {
            if (maxHealth < heal + health)
            {
                if (heal + health > background.value)
                {
                    background.value = health;
                    background.DOValue(maxHealth, 1f, false).SetEase(Ease.OutExpo);
                }
                health = maxHealth;
                healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
            }
            else
            {
                if (heal + health > background.value)
                {
                    background.value = health;
                    background.DOValue(health + heal, 1f, false).SetEase(Ease.OutExpo);
                }
                health += heal;
                healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
            }
        }
    }

    public void Die()
    {
        Debug.Log("Dead");
    }

}
