using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum PROFESSION{HUNTER,PRIEST,CHEF,SAILOR,SHOOTER}
public class PlayerController : MonoBehaviour
{
   public PROFESSION profession;
   private Animator anim;
   private NavMeshAgent agent;

   protected CharacterStats characterStats;

   public GameObject myBag;
   bool isOpen;

   void Awake()
   {
      anim = GetComponent<Animator>();
      agent = GetComponent<NavMeshAgent>();
      Profession();
   }

   void Start() 
   {
       Profession();
   }
  
   void Update()
   {
       SwitchAnimation();
       OpenMyBag();
    
   }

   void SwitchAnimation()
   {
       anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
   }
   void Profession()
   {
        switch(profession)
        {
            case PROFESSION.HUNTER:
                HunterSkill();          
                break;
            case PROFESSION.CHEF:
                ChefSKill();
                break;
            case PROFESSION.PRIEST:
                PriestSkill();
                break;
            case PROFESSION.SAILOR:
                Sailor();
                break;
            case PROFESSION.SHOOTER:
                ShooterSkill();
                break;    
        }

   }

   void OpenMyBag()
   {
       if(Input.GetKeyDown(KeyCode.O))
       {
           isOpen =!isOpen;
           myBag.SetActive(isOpen);
       }
   }

   void HunterSkill()
   {
       //開局配有斧頭跟陷阱
   }
   void ChefSKill()
   {
       //開局有食物
   }
   void PriestSkill()
   {
       //可以開啟棺材
   }
   void Sailor()
   {
       //可以開寶藏
   }
   void ShooterSkill()
   {
       //有手槍
   }



}
