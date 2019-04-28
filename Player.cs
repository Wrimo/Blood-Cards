using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    public int Block;

    public Queue<GameObject> DrawPile;
    public List<GameObject> DiscardPile;
    public GameObject StabCard;
    public GameObject BlockCard;
    //  public GameObject TestingCard;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI BlockText;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI MutiplierText;
    public Slider MultiplySlider;
    public Animator animator;
    public List<GameObject> Loot;
    public int Multiplier;
    public int SliderMutiplier;
    public bool SelectedLoot;
    public int HealPerTurn;
    public int SelfDamage;

    public bool PlayerHasGone;
    public bool PlayerTurnOver;



    void Start()
    {

        SliderMutiplier = 1;
        SelfDamage = 0;
        SelectedLoot = false;
        StartPlayerTurn();
        PlayerTurnOver = false;
        Multiplier = 0;
        DiscardPile = new List<GameObject>();
        HealPerTurn = 1;

        DrawPile = new Queue<GameObject>();


        for (int i = 0; i < 6; i++)
        {
            DiscardPile.Add(StabCard);
        }
        for (int i = 0; i < 4; i++)
        {
            DiscardPile.Add(BlockCard);
        }

        MaxHealth = 20;
        Health = MaxHealth;
        GameManager.ClearedRooms = 0;
        ShuffleCards();

    }
    void Update()
    {
        HealthText.text = Health.ToString();
        BlockText.text = Block.ToString();
        HealthControll();
        EndGame();
        CloseGame(); 
    }
    public void ShuffleCards()
    {
        DiscardPile = DiscardPile.OrderBy(a => Guid.NewGuid()).ToList();
        foreach (GameObject card in DiscardPile)
        {

            DrawPile.Enqueue(card);
        }
        DiscardPile = new List<GameObject>();

    }

    public void DealCard()
    {
        if (!PlayerHasGone && !Camera.main.GetComponent<EnemyManager>().IsEnemyTurn)
        {
            DrawCards(6);
        }

    }
    public void DrawCards(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            if (DrawPile.Count == 0)
                ShuffleCards();


            GameObject card = DrawPile.Dequeue();

            DiscardPile.Add(card);
            card.GetComponent<CardMovement>().origPos = new Vector3(-8 + (1.5f * i), -3, 0);
            card.GetComponent<CardMovement>().IsLoot = false;

            GameObject.Instantiate(card);
            PlayerTurnOver = false;
            PlayerHasGone = true;


        }
    }
    public void StartPlayerTurn()
    {
        SelfDamage = 0;
        Multiplier = 1;
        Camera.main.GetComponent<Player>().SelectedLoot = false;
        PlayerTurnOver = false;
        TurnText.text = "Your Turn";
        animator.SetBool("NewTurn", true);
        Block = 0;
        Invoke("ResetTurnText", 1f);
    }
    public void EndPlayerTurn()
    {
        if (!Camera.main.GetComponent<EnemyManager>().IsEnemyTurn && !CheckIfAllEnemiesAreDead())
        {

            PlayerTurnOver = true;
            Multiplier = 0;
            TurnText.text = "Enemy Turn";
            animator.SetBool("NewTurn", true);
            Camera.main.GetComponent<EnemyManager>().IsEnemyTurn = true;
            Camera.main.GetComponent<EnemyManager>().EnemyTurn();

            Health += HealPerTurn;

            Invoke("ResetTurnText", 1f);
        }
        else if (CheckIfAllEnemiesAreDead())
        {
            PlayerTurnOver = true;
            ShowLoot();
        }
    }
    public void ResetTurnText()
    {
        animator.SetBool("NewTurn", false);
    }
    public void DamagePlayer(int damageDealt)
    {
        int random = UnityEngine.Random.Range(1, 6);
        int damageToDeal = damageDealt - Block;

        if (damageToDeal <= 0)
        {
            Health += random;
            damageToDeal = 0;
        }

        Health -= damageToDeal;
    }
    private void HealthControll()
    {
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
    private bool CheckIfAllEnemiesAreDead()
    {
        int nulls = 0;
        foreach (GameObject enemy in Camera.main.GetComponent<EnemyManager>().aliveEnemies)
        {
            if (enemy == null)
                nulls++;
        }
        if (nulls == Camera.main.GetComponent<EnemyManager>().aliveEnemies.Count)
        {
            GameManager.ClearedRooms++;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ShowLoot()
    {

        GameObject loot1 = Loot.ElementAt(UnityEngine.Random.Range(0, Loot.Count));
        loot1.transform.position = new Vector3(-3, 0, 1);
        loot1.GetComponent<CardMovement>().IsLoot = true;
        loot1.GetComponent<CardMovement>().IsEnemyDrop = false;
        GameObject.Instantiate(loot1);

        GameObject loot2 = Loot.ElementAt(UnityEngine.Random.Range(0, Loot.Count));
        loot2.transform.position = new Vector3(0, 0, 1);
        loot2.GetComponent<CardMovement>().IsEnemyDrop = false;
        loot2.GetComponent<CardMovement>().IsLoot = true;

        GameObject.Instantiate(loot2);

        GameObject loot3 = Loot.ElementAt(UnityEngine.Random.Range(0, Loot.Count));
        loot3.transform.position = new Vector3(3, 0, 1);
        loot3.GetComponent<CardMovement>().IsLoot = true;
        loot3.GetComponent<CardMovement>().IsEnemyDrop = false;

        GameObject.Instantiate(loot3);

    }
    public void SliderChange()
    {
        SliderMutiplier = (int)MultiplySlider.value;
        MutiplierText.text = SliderMutiplier + "x";
    }
    public void CloseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void EndGame()
    {
        if (GameManager.ClearedRooms == 15)
        {
            SceneManager.LoadScene("Victory");
        }
        if (Health <= 0)
        {
            SceneManager.LoadScene("Failure");
        }
    }
}


