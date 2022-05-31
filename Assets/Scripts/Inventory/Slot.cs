using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
   public int slotID;//空格ID 等於 物品ID
   public Item slotItem;
   public Image slotImage;
   public Text slotNum;
   public string slotInfo;

   public GameObject itemInSlot;
   public void ItemOnclicked()
   {
       InventoryManager.UpdateItemInfo(slotInfo);
   }

   public void SetupSlot(Item item)
   {
       if(item == null)
       {
           itemInSlot.SetActive(false);
           return;
       }

       slotImage.sprite = item.itemImage;
       slotNum.text = item.itemHeld.ToString("00");
       slotInfo = item.itemInfo;
   }

}
