using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    // Start is called before the first frame update
    ActionBasedController contoller;
    public Hand hand;

    void Start()
    {
        contoller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(contoller.selectActionValue.action.ReadValue<float>());
        hand.SetTrigger(contoller.activateActionValue.action.ReadValue<float>());
    }
}