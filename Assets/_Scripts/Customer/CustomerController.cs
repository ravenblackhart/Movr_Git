using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField] Animator[] animators;

    [SerializeField] Transform modelParent;
    [SerializeField] GameObject children;

    CustomerIdentity identity;
    bool hasKids;

    public void Initialize(CustomerIdentity identity, bool hasKids)
    {
        this.identity = identity;
        this.hasKids = hasKids;

        for (int i = 0; i < modelParent.childCount; i++)
        {
            modelParent.GetChild(i).gameObject.SetActive(i == (int)identity);

            children.SetActive(hasKids);
        }
    }

    public void UpdateAnimations(int animationState)
    {
        animators[(int)identity].SetInteger("SittingState", animationState);

        if (hasKids)
        {
            animators[3].SetInteger("SittingState", animationState);

            animators[4].SetInteger("SittingState", animationState);
        }
    }
}
