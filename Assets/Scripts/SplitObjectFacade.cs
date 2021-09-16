using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitObjectFacade
{
    private SplitObject _spliteObject;
    public SplitObjectFacade(SplitObject splitObject)
    {
        _spliteObject = splitObject;
    }

    public void ExplodeObject(Action onComplete)
    {
        _spliteObject.AllPartsDisappeared += onComplete;
        _spliteObject.EnableGameObject();
        _spliteObject.AddForceToParts();
        _spliteObject.ScaleDownParts();
    }
}
