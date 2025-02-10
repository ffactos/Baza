using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CodePad : MonoBehaviour
{
    public int[] code = new int[4];
    int[] num = new int[4];
    int n = 0;

    bool isReady = true;

    Color check = Color.white;

    [SerializeField]
    TextMeshProUGUI monitorText;
    [SerializeField]
    AudioSource sound;

    private void Start()
    {
        isReady = true;
        monitorText.text = ""; 

        for (int i = 0; i < code.Length; i++)
        {
            code[i] = UnityEngine.Random.Range(1, 9);
        }
    }

    public void EnterNumber(int number)
    {

        sound.Play();

        if (n < num.Length)
        {
            if (isReady)
            {
                num[n] = number;
                monitorText.text = monitorText.text + num[n];
                n++;
            }
        }
    }

    public void EnterCode()
    {

        sound.Play();
        StopAllCoroutines();
        isReady = true;
        monitorText.GetComponentInParent<MeshRenderer>().material.color = Color.black;
        monitorText.text = "";

        check = Color.white;
        for (int i = 0;i < code.Length;i++)
        {
            if (code[i] == num[i])
            {
                check = Color.green;
            }
            else
            {
                check = Color.red;
                break;
            }
        }
        StopAllCoroutines();
        StartCoroutine(Say());
        num = new int[4];
        n = 0;
    }

    IEnumerator Say()
    {
        monitorText.text = "";
        isReady = false;
        monitorText.GetComponentInParent<MeshRenderer>().material.color = check;
        yield return new WaitForSeconds(1f);
        isReady = true;
        monitorText.GetComponentInParent<MeshRenderer>().material.color = Color.black;
        monitorText.text = "";
    }

}
