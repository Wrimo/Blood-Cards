using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
  public string name; 
  public string description; 
  public int healthCost; 
  public int attack; 
  public int defense; 
  public int drawCard; 
  public int effectMutiplier; 
  public bool affectAllEnemies; 
  public bool stunEnemy; 
  public int healthUpgrade;
  public int HealthPerTurn; 
  public bool randomTarget;
  public bool returnDamage; 
  public bool selfDamagePercentage; 
  public bool affectBySlider; 
  

}
