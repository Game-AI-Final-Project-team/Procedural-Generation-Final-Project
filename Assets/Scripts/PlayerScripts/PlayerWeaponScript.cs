using UnityEngine;

public interface IHasHealth
{
    public void Hurt(int damage);
}

public class PlayerWeaponScript : MonoBehaviour, IHasHealth
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
            Destroy(collision.gameObject);
    }


    public void Hurt(int damage)
    {

    }
}
