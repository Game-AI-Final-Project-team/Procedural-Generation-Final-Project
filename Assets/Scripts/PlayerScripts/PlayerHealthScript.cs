using UnityEngine;

public interface IHasHealth
{
    public void Hurt(int damage);
}

public class PlayerHealthScript : MonoBehaviour, IHasHealth
{
    [SerializeField] int health = 100;
    [SerializeField] bool invincible = false;
       
    public void Hurt(int damage)
    {
        if (invincible) return;
        health -= damage;
        Debug.Log("I got Hurt");
        if (health < 0) Destroy(gameObject);
    }    
}
