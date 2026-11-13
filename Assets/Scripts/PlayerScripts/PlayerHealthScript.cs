using UnityEngine;

public interface IHasHealth
{
    public void Hurt(int damage);
}

public class PlayerHealthScript : MonoBehaviour, IHasHealth
{
    [SerializeField] int health = 10;
       
    public void Hurt(int damage)
    {
        health -= damage;
        if (health < 0) Destroy(gameObject);
    }    
}
