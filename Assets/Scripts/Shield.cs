using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shield : MonoBehaviour
{
    private GameObject leftController;
    private ActionBasedController leftActionBasedController;
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        leftController = GameObject.Find("LeftHand Controller");
        leftActionBasedController = leftController.GetComponent<ActionBasedController>();
        socketInteractor = leftController.GetComponent<XRSocketInteractor>();
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject stone = other.gameObject;
        if (
            stone.CompareTag("stone")
            && stone.GetComponent<Rigidbody>().velocity.magnitude > 0.5)
        {
            leftActionBasedController.SendHapticImpulse(1.0f, 0.15f);
        }
    }
}
