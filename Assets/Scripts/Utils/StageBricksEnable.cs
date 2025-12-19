using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBricksEnable : MonoBehaviour
{
    private void OnEnable()
    {
        ActiveAllChildObject(transform, true);
    }

    private void OnDisable()
    {
        ActiveAllChildObject(transform, false);
    }

    private void ActiveAllChildObject(Transform parentTransform, bool isActive)
    {
        parentTransform.gameObject.SetActive(isActive);

        foreach(Transform child in parentTransform)
        {
            ActiveAllChildObject(child, isActive);
        }
    }
}
