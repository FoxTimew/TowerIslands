using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GenericAnimationEventListener : MonoBehaviour
{
    public string parameterName = "";
    public UnityEvent callback = new UnityEvent();
    
    void OnAnimationEvent(string parameter)
    {
        if(parameter == parameterName)
            callback.Invoke();
    }
}
