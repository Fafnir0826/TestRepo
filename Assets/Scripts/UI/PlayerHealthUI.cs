using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
   Text levelText;
   Image  healthSldier;

   void Awake() 
   {
       levelText = transform.GetChild(2).GetComponent<Text>();
       healthSldier = transform.GetChild(0).GetChild(0).GetComponent<Image>();
      
   }

   void Update() 
   {
   }

   void UpdateHealth()
   {
       float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth/GameManager.Instance.playerStats.MaxHealth;
       healthSldier.fillAmount = sliderPercent;
   }

   
}
