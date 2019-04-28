using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
public class EnemyManager : MonoBehaviour
{
    public List<GameObject> Enemies;
    public List<GameObject> aliveEnemies;

    private Vector3 origScale;
    public bool IsEnemyTurn;
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI TypeText;
    public List<GameObject> EnemyLoot;
    private int totalAttackDamage;

    void Start()
    {
        aliveEnemies = new List<GameObject>();
        CreateNewChallenge();
    }

    public void CreateNewChallenge()
    {
        int maxHealth = 0;
        int maxDam = 0;
        if (GameManager.ClearedRooms < 3)
        {
            maxHealth = 4;
            maxDam = 1;
        }
        else if (GameManager.ClearedRooms < 6)
        {
            maxHealth = 8;
            maxDam = 4;
        }
        else if (GameManager.ClearedRooms < 10)
        {
            maxHealth = 12;
            maxDam = 6;
        }
        else if (GameManager.ClearedRooms < 16)
        {
            maxHealth = 16;
            maxDam = 8;
        }
        IsEnemyTurn = false;
        int random = UnityEngine.Random.Range(2, 3 + GameManager.ClearedRooms);
        if (random > 4)
            random = 4;
        int numOfHeals = 0;
        int numOfShields = 0;
        for (int i = 0; i < random; i++)
        {
            GameObject enemy = Enemies.ElementAt(UnityEngine.Random.Range(0, Enemies.Count));
            enemy.transform.position = new Vector3(-9 + 3.5f * i + 1, 0, 1);


            int ranNum = UnityEngine.Random.Range(1, 11);
            if (ranNum == 1 && numOfHeals < 2)
            {
                enemy.GetComponent<Enemy>().Type = "Healer";
            }
            else if (ranNum == 2 && numOfShields < 2)
            {
                enemy.GetComponent<Enemy>().Type = "Tank";
            }
            else
            {
                enemy.GetComponent<Enemy>().Type = "Grunt";
            }



            int ranHealth = UnityEngine.Random.Range(2 + (int)Math.Ceiling(GameManager.ClearedRooms / 2f), maxHealth);
            if (enemy.GetComponent<Enemy>().Type == "Healer")
            {
                ranHealth += UnityEngine.Random.Range(1, 3);
            }
            else if (enemy.GetComponent<Enemy>().Type == "Tank")
            {
                ranHealth += UnityEngine.Random.Range(3, 7);
            }
            else if (enemy.GetComponent<Enemy>().Type == "Grunt")
            {
                ranHealth += UnityEngine.Random.Range(1, 3);
            }
            if (ranHealth <= 0)
            {
                ranHealth = 1;
            }

            int ranDam = UnityEngine.Random.Range(1 + (int)Math.Ceiling(GameManager.ClearedRooms / 4f), maxDam);
            if (enemy.GetComponent<Enemy>().Type == "Healer")
            {
                ranDam += UnityEngine.Random.Range(-2, 3);
            }
            else if (enemy.GetComponent<Enemy>().Type == "Tank")
            {
                ranDam += UnityEngine.Random.Range(-2, 3);
            }
            else if (enemy.GetComponent<Enemy>().Type == "Grunt")
            {
                ranDam += UnityEngine.Random.Range(1, 5);
            }

            if (ranDam <= 0)
            {
                ranDam = 1;
            }
            if (GameManager.ClearedRooms > 2 && enemy.GetComponent<Enemy>().Type == "Healer")
            {
                enemy.GetComponent<Enemy>().HealPower = UnityEngine.Random.Range(2, GameManager.ClearedRooms);
            }
            else if (enemy.GetComponent<Enemy>().Type == "Healer")
            {
                enemy.GetComponent<Enemy>().HealPower = 2;
            }
            else
            {
                enemy.GetComponent<Enemy>().HealPower = 0;
            }


            enemy.GetComponent<Enemy>().Health = ranHealth;
            enemy.GetComponent<Enemy>().Damage = ranDam;
            aliveEnemies.Add(GameObject.Instantiate(enemy));
        }
    }







    public void EnemyTurn()
    {
        Invoke("EnemyAttacks", 2.0f);
    }

    private void EnemyAttacks()
    {
        totalAttackDamage = 0;
        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            if (aliveEnemies.ElementAt(i) != null && !aliveEnemies.ElementAt(i).GetComponent<Enemy>().Stunned)
            {
                totalAttackDamage += aliveEnemies.ElementAt(i).GetComponent<Enemy>().Damage;
                if (aliveEnemies.ElementAt(i).GetComponent<Enemy>().Type == "Healer")
                {
                    foreach (GameObject friendly in aliveEnemies)
                    {
                        if (friendly != null)
                        {
                            friendly.GetComponent<Enemy>().Health += aliveEnemies.ElementAt(i).GetComponent<Enemy>().HealPower;
                        }
                    }
                }
                origScale = aliveEnemies.ElementAt(i).transform.localScale;
                aliveEnemies.ElementAt(i).transform.localScale = new Vector3(transform.localScale.x + 4, transform.localScale.y + 4, transform.localScale.z);
                Invoke("ResetSize", 0.25f);
            }
            else if (aliveEnemies.ElementAt(i) != null)
            {
                aliveEnemies.ElementAt(i).GetComponent<Enemy>().Stunned = false;
            }
        }
        Camera.main.GetComponent<Player>().DamagePlayer(totalAttackDamage);
        IsEnemyTurn = false;
        Camera.main.GetComponent<Player>().PlayerHasGone = false;
        Camera.main.GetComponent<Player>().StartPlayerTurn();

    }
    private void ResetSize()
    {
        for (int i = 0; i < aliveEnemies.Count; i++)
        {
            if (aliveEnemies.ElementAt(i) != null)
                aliveEnemies.ElementAt(i).transform.localScale = origScale;
        }
    }


}
