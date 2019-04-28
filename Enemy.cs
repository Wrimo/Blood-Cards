using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System; 

public class Enemy : MonoBehaviour
{
    public int Health;
    private int maxHealth;
    public int HealPower;

    public int Damage;
    public int Block;
    Color origColor;
    public EnemyInfo EnemyInfo;
    private bool hasAttacked;
    public bool Stunned;
    public string Type;




    void Start()
    {
        maxHealth = Health;
        Stunned = false;

        origColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    void OnMouseEnter()
    {
        Camera.main.GetComponent<EnemyManager>().DamageText.text = "Damage: " + Damage.ToString();
        Camera.main.GetComponent<EnemyManager>().HealthText.text = "Health: " + Health.ToString();
        if (Stunned)
        {
            Camera.main.GetComponent<EnemyManager>().StatusText.text = "State: Stunned";
        }
        else
        {
            Camera.main.GetComponent<EnemyManager>().StatusText.text = "State: Idle";
        }
        Camera.main.GetComponent<EnemyManager>().TypeText.text = "Type: " + Type;

    }
    void OnMouseExit()
    {
        Camera.main.GetComponent<EnemyManager>().DamageText.text = "Damage: ";
        Camera.main.GetComponent<EnemyManager>().HealthText.text = "Health: ";
        Camera.main.GetComponent<EnemyManager>().StatusText.text = "State: ";
        Camera.main.GetComponent<EnemyManager>().TypeText.text = "Type: ";
    }
    void Update()
    {
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
    }
    public void DamageEnemy(int damage)
    {

        if (damage != 0)
        {
            if (Stunned)
            {
                Health -= (int)Math.Floor(damage * 1.25);
            }
            else
            {
                Health -= damage;
            }

            FlashOntHit();
        }


        if (Health <= 0)
        {
            Camera.main.GetComponent<Player>().Health += UnityEngine.Random.Range(1, maxHealth);
            DropLoot();
            Destroy(this.gameObject);
        }
    }
    private void DropLoot()
    {
        int random = UnityEngine.Random.Range(1, 7);

        Camera.main.GetComponent<Player>().SelectedLoot = false;
        if (random == 1)
        {
            GameObject loot = Camera.main.GetComponent<EnemyManager>().EnemyLoot.ElementAt(UnityEngine.Random.Range(0, Camera.main.GetComponent<EnemyManager>().EnemyLoot.Count));
            loot.transform.position = this.gameObject.transform.position;
            loot.GetComponent<CardMovement>().IsLoot = true;
            loot.GetComponent<CardMovement>().IsEnemyDrop = true;
            GameObject.Instantiate(loot);
        }
    }

    void FlashOntHit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.07f);
    }
    void ResetColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = origColor;

    }
}
