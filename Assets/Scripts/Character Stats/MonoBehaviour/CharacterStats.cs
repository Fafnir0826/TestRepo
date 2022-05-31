using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int>UpdateHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    // Start is called before the first frame update
    [HideInInspector]
   public bool isCritical;

   void Awake()
   {
    if(templateData !=null)
        characterData = Instantiate(templateData);
    templateData.currentHealth = characterData.maxHealth;
   }

   #region Readd from Data_SO
   public int MaxHealth
   {
     get
     {
        if(characterData != null)
            return characterData.maxHealth;
        else return 0;
     }
     set
     {
         characterData.maxHealth = value;
     }
   }

   public int CurrentHealth
   {
     get
     {
        if(characterData != null)
            return characterData.currentHealth;
        else return 0;
     }
     set
     {
         characterData.currentHealth = value;
     }
   }

   public int MaxHunger
   {
     get
     {
        if(characterData != null)
            return characterData.maxHunger;
        else return 0;
     }
     set
     {
         characterData.maxHunger = value;
     }
   }

   public int CurrentHunger
   {
     get
     {
        if(characterData != null)
            return characterData.currentHunger;
        else return 0;
     }
     set
     {
         characterData.currentHunger = value;
     }
   }
   public int MaxMana
   {
     get
     {
        if(characterData != null)
            return characterData.maxMana;
        else return 0;
     }
     set
     {
         characterData.maxMana = value;
     }
   }

   public int CurrentMana
   {
     get
     {
        if(characterData != null)
            return characterData.currentMana;
        else return 0;
     }
     set
     {
         characterData.currentMana = value;
     }
   }
   public int MaxWarmth
   {
     get
     {
        if(characterData != null)
            return characterData.maxWarmth;
        else return 0;
     }
     set
     {
         characterData.maxWarmth = value;
     }
   }

   public int CurrentWarmth
   {
     get
     {
        if(characterData != null)
            return characterData.currentWarmth;
        else return 0;
     }
     set
     {
         characterData.currentWarmth = value;
     }
   }
  public int MaxStamina
   {
     get
     {
        if(characterData != null)
            return characterData.maxStamina;
        else return 0;
     }
     set
     {
         characterData.maxStamina = value;
     }
   }

   public int CurrentStamina
   {
     get
     {
        if(characterData != null)
            return characterData.currentStamina;
        else return 0;
     }
     set
     {
         characterData.currentStamina = value;
     }
   }
   #endregion
   #region  Character Combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage(), 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage,0);

        if(attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        //TODO:Update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
           
    }

    public void TakeDamage(int damage,CharacterStats defender)
    {
        int currentDamage = damage;
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage,0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);

    }

    private int CurrentDamage()
    {
       float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);

       if(isCritical)
       {
           coreDamage *= attackData.criticalMultiplier;
          // Debug.Log("critical"+coreDamage);
       }
       return (int)coreDamage;
    }
    #endregion
}
