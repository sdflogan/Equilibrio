using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectBehaviour : MonoBehaviour
{
    public bool StopTime = false;

    private Animator animator;

    public float valueToStart = 0.10f;
    public float minCorrectValue = 0.20f;
    public float maxCorrectValue = 0.40f;
    private Vector3 startValue;

    private float timeDelayed;
    private bool flag;
    private void Start()
    {
        flag = false;
        timeDelayed = 0;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

        if (!SliderEventController.Instance.Enabled)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

        if (timeDelayed < 0)
        {
            timeDelayed = 0;
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            flag = true;
            animator.SetFloat("Speed", 1);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !StopTime)
        {
            flag = false;
            animator.SetFloat("Speed", -1);
        }

        if (flag)
        {
            timeDelayed += Time.deltaTime;
        }
        else
        {
            timeDelayed -= Time.deltaTime;
        }
    }
}
