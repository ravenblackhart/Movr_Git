using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [SerializeField] private bool shouldRoam = true;
    [HideInInspector] public BlockManager blockManager;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float hopHeight;
    [SerializeField] private AnimationCurve hopCurve;
    [SerializeField] private AnimationCurve spinning;
    private Transform targetPos;
    private bool clockWise;
    [HideInInspector] public int nodeIndex;
    private Transform car;
    private bool isVisible = true;
    private bool shouldHide = false;
    private bool isHidden = false;
    private float time;
    [SerializeField] private float timeMultiplier = 1.5f;
    private bool canStart = true;
    private bool checkForDistance = true;

    private void Start()
    {
        time = Random.Range(0f, 1f);
        clockWise = Random.Range(0f, 1f) > 0.5f ? true : false;
        car = GameManager.instance.car;
        if (!EnableDisableCheck())
            TogglePedestrianVisibility(false);
        if (!shouldRoam) targetPos = car.transform;
    }

    private void Update()
    {
        if (EnableDisableCheck())
        {
            if (!isVisible)
                TogglePedestrianVisibility(true);
        }
        else
        {
            if (isVisible)
                TogglePedestrianVisibility(false);
            return;
        }

        if (HideCheck() && !shouldHide && !isHidden)
        {
            shouldHide = true;
        }
        else if(!HideCheck() && isHidden)
        {
            Debug.Log("Leave Underground");
            isHidden = false;
            StartCoroutine(ToggleUnderground(false));
        }

        if (isHidden) return;
        time += Time.deltaTime;
        if (time >= 1f / timeMultiplier)
        {
            time = 0f;
            if (shouldHide)
            {
                //Debug.Log("Go Underground");
                StartCoroutine(ToggleUnderground(true));
                shouldHide = false;
            }
        }
        transform.position = new Vector3(transform.position.x, hopCurve.Evaluate(time * timeMultiplier) * hopHeight, transform.position.z);
        if (!shouldRoam) return;
        transform.Translate(transform.forward * Time.deltaTime * walkSpeed, Space.World);
        DistanceToTarget();
    }
    private IEnumerator ToggleUnderground(bool state)
    {
        isHidden = state;
        float mTime = 0f;
        canStart = false;
        Vector3 startRot = transform.forward;
        Vector3 startPos = transform.position;
        Vector3 mTargetPos = new Vector3();

        if (state)
            mTargetPos = new Vector3(startPos.x, -3f, startPos.z);
        else
            mTargetPos = new Vector3(startPos.x, 0f, startPos.z);

        while (mTime < 1f)
        {
            mTime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, startRot.y + spinning.Evaluate(mTime) * 360f, 0f);
            transform.position = Vector3.Lerp(startPos, mTargetPos, mTime);
            yield return null;
        }
        transform.LookAt(targetPos);
        transform.position = mTargetPos;
        time = 0f;
        canStart = true;
    }
    public void SetTargetPos(Transform nodeA, Transform nodeB) 
    {
        targetPos = clockWise ? nodeB : nodeA;
        nodeIndex = clockWise ? nodeB.GetSiblingIndex() : nodeA.GetSiblingIndex();
        transform.LookAt(targetPos);
    }
    private bool HideCheck() => new Vector2(transform.position.x - car.position.x, transform.position.z - car.position.z).magnitude < 12f ? true : false;
    private bool EnableDisableCheck() => (transform.position - car.position).magnitude < 200f ? true : false;
    private void TogglePedestrianVisibility(bool state)
    {
        isVisible = state;

        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(state);
        }
    }
    private void ChangeTarget()
    {
        targetPos = blockManager.GetNextNode(nodeIndex, clockWise);
        nodeIndex = targetPos.GetSiblingIndex();
        transform.LookAt(targetPos);
    }
    private void DistanceToTarget()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z) - new Vector2(targetPos.position.x, targetPos.position.z);
        if(pos.magnitude < 2f && checkForDistance)
        {
            Invoke("ChangeTarget", Random.Range(0f, 1f));
            checkForDistance = false;
        }
        else if(pos.magnitude > 2f)
        {
            checkForDistance = true;
        }
    }
}
