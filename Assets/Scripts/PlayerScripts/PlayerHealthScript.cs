using UnityEngine;

public interface IHasHealth
{
    public void Hurt(int damage);
}

public class PlayerHealthScript : MonoBehaviour, IHasHealth
{
    [SerializeField] int health = 100;
       
    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log("I got Hurt");
        if (health < 0) Destroy(gameObject);
    }    
}
