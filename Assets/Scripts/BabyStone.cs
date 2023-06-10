using UnityEngine;

public class BabyStone : MonoBehaviour
{
    // Ignore collision within baby stones
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BabyStone"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
        }
    }
    
}
