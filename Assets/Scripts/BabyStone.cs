using UnityEngine;

public class BabyStone : Stone
{
    // Ignore collision within baby stones
    protected new void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);

        if (other.gameObject.CompareTag("BabyStone"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
        }
    }
}
