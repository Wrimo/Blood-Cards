using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyInfo : ScriptableObject
{
  public string name; 
  public string description; 
  public int health; 
  public int attack; 
  public int defense; 
    

}
