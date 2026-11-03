using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            Destroy(collision.gameObject);
    }
}
