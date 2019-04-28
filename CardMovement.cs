using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
class CardMovement : MonoBehaviour
{
    private Color mouseOverColor = Color.blue;
    private Color originalColor = Color.yellow;
    private bool dragging = false;
    private float distance;
    public GameObject Target;
    public Vector3 origPos;
    public Card CardInfo;
    public bool IsLoot;
    public bool IsEnemyDrop;
    private bool isSelectedLoot;




    void OnMouseEnter()
    {

        gameObject.transform.localScale = new Vector3(5, 5, 1);
    }

    void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(4, 4, 1);

    }

    void OnMouseDown()
    {
        if (!IsLoot)
        {
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
        }
        else if (IsLoot)
        {
            LootSelected();

        }
    }

    void OnMouseUp()
    {
        dragging = false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Target = col.gameObject;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        Target = null;
    }
    void Update()
    {
        Dragging();
        PlayerEndTurn();
        ClearCards();
    }


    private void PlayerEndTurn()
    {
        if (Camera.main.GetComponent<Player>().PlayerTurnOver && !IsLoot)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.5f, 1), 5.0f * Time.deltaTime);
            if (transform.localScale.x < 0.7f)
            {
                Destroy(this.gameObject);
            }
        }
    }


    private void Dragging()
    {
        if (!IsLoot)
        {
            if (dragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(distance);
                transform.position = rayPoint;
            }
            else if (Target == null)
            {
                transform.position = Vector3.Lerp(transform.position, origPos, 5.0f * Time.deltaTime);
                if (Vector3.Distance(origPos, transform.position) > 0.5f)
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }

            }
            else if (Target.layer == 9)
            {
                GameObject enemy = Target;
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.5f, 1), 5.0f * Time.deltaTime);
                if (transform.localScale.x < 0.7f)
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    if (Camera.main.GetComponent<Player>().Multiplier == 0)
                        Camera.main.GetComponent<Player>().Multiplier = 1;

                    Camera.main.GetComponent<Player>().Block += CardInfo.defense * Camera.main.GetComponent<Player>().Multiplier * Camera.main.GetComponent<Player>().SliderMutiplier;

                    if (!CardInfo.affectAllEnemies)
                    {
                        enemy.GetComponent<Enemy>().DamageEnemy(CardInfo.attack * Camera.main.GetComponent<Player>().Multiplier * Camera.main.GetComponent<Player>().SliderMutiplier);


                    }
                    else if (CardInfo.affectAllEnemies)
                    {

                        foreach (GameObject baddy in Camera.main.GetComponent<EnemyManager>().aliveEnemies)
                        {
                            if (baddy != null)
                                baddy.GetComponent<Enemy>().DamageEnemy(CardInfo.attack * Camera.main.GetComponent<Player>().Multiplier * Camera.main.GetComponent<Player>().SliderMutiplier);
                        }
                    }

                    if (CardInfo.attack == 0 && CardInfo.defense == 0)
                    {
                        Camera.main.GetComponent<Player>().Health -= CardInfo.healthCost;
                    }
                    else
                    {
                        Camera.main.GetComponent<Player>().Health -= CardInfo.healthCost * Camera.main.GetComponent<Player>().SliderMutiplier;
                    }




                    if (CardInfo.stunEnemy)
                    {
                        enemy.GetComponent<Enemy>().Stunned = true;
                    }
                    if (CardInfo.drawCard > 0)
                    {
                        Camera.main.GetComponent<Player>().DrawCards(CardInfo.drawCard);
                    }
                    if (CardInfo.effectMutiplier > 0)
                    {
                        Camera.main.GetComponent<Player>().Multiplier = 2;
                    }

                    Destroy(this.gameObject);
                }
            }
        }
    }
    private void LootSelected()
    {

        isSelectedLoot = true;
        if (CardInfo.name == "Divine Blesssing")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(0));
        }
        else if (CardInfo.name == "Health Upgrade")
        {
            Camera.main.GetComponent<Player>().MaxHealth += CardInfo.healthUpgrade;
            Camera.main.GetComponent<Player>().Health = Camera.main.GetComponent<Player>().MaxHealth;
        }
        else if (CardInfo.name == "New Shield")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(3).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(3));
        }
        else if (CardInfo.name == "Smoke Bomb")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(4).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(4));
        }
        else if (CardInfo.name == "Holy Sword")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(5).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(5));
        }
        else if (CardInfo.name == "Double Edged Sword")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(6).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(6));
        }
        else if (CardInfo.name == "Heath per Turn Increase")
        {
            Camera.main.GetComponent<Player>().HealPerTurn += CardInfo.HealthPerTurn;
        }
        else if (CardInfo.name == "Blade Barrage")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(9).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(9));
        }
        else if (CardInfo.name == "Sword")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(8).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(8));
        }
        else if (CardInfo.name == "Sacrifice for Revenge")
        {
            Camera.main.GetComponent<Player>().Loot.ElementAt(9).transform.position = new Vector3(8, -3);
            Camera.main.GetComponent<Player>().Loot.ElementAt(0).transform.localScale = new Vector3(1, 1, 1);
            Camera.main.GetComponent<Player>().DiscardPile.Add(Camera.main.GetComponent<Player>().Loot.ElementAt(9));
        }
        else if (CardInfo.name == "Small Health Upgrade")
        {
            Camera.main.GetComponent<Player>().MaxHealth += 3;
            Camera.main.GetComponent<Player>().Health += Camera.main.GetComponent<Player>().MaxHealth;
        }

        Camera.main.GetComponent<Player>().SelectedLoot = true;
    }
    private void ClearCards()
    {
        if (isSelectedLoot && !IsEnemyDrop)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(5f, 5f, 1), 5.0f * Time.deltaTime);
            if (transform.localScale.x > 4.5f)
            {
                Camera.main.GetComponent<Player>().PlayerHasGone = false;
                Camera.main.GetComponent<Player>().PlayerTurnOver = false;
                Camera.main.GetComponent<EnemyManager>().IsEnemyTurn = false;
                Camera.main.GetComponent<EnemyManager>().CreateNewChallenge();
                Destroy(this.gameObject);
            }

        }
        else if (isSelectedLoot && IsEnemyDrop)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(5f, 5f, 1), 5.0f * Time.deltaTime);
            Camera.main.GetComponent<Player>().SelectedLoot = false;
            if (transform.localScale.x > 4.5f)
            {
                Destroy(this.gameObject);
            }
        }
        else if (IsLoot && Camera.main.GetComponent<Player>().SelectedLoot)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.5f, 1), 5.0f * Time.deltaTime);
            if (transform.localScale.x < 1f)
            {
                Destroy(this.gameObject);
            }
        }

    }
}