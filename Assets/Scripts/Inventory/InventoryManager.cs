using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
   public Inventory myBag;
   public GameObject slotGrid;
   //public Slot slotPrefab;
   public GameObject emptySlot;
   public Text itemInfromation;

   public List<GameObject> slots = new List<GameObject>();

   void OnEnable()
   {
       RefreshItem();
       Instance.itemInfromation.text = "";
   }

   public static void UpdateItemInfo(string itemDescripttion)
   {
       Instance.itemInfromation.text = itemDescripttion;
   }

  /* public static void CreateNewItem(Item item)
   {
       Slot newItem = Instantiate(Instance.slotPrefab,Instance.slotGrid.transform.position,Quaternion.identity);
       newItem.gameObject.transform.SetParent(Instance.slotGrid.transform);
       newItem.slotItem = item;
       newItem.slotImage.sprite = item.itemImage;
       newItem.slotNum.text = item.itemHeld.ToString();

   }*/

   public static void RefreshItem()
   {
       //循環刪除slotGrid下的子集物體
       for (int i = 0; i < Instance.slotGrid.transform.childCount ; i++)
       {
           if (Instance.slotGrid.transform.childCount ==0)
               break;

           Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
           Instance.slots.Clear();
       }
        //重新生成對應myBag裡面的物品的slot
       for (int i = 0; i < Instance.myBag.itemList.Count; i++)
       {
          // CreateNewItem(Instance.myBag.itemList[i]);
          Instance.slots.Add(Instantiate(Instance.emptySlot));
          Instance.slots[i].transform.SetParent(Instance.slotGrid.transform);
          Instance.slots[i].GetComponent<Slot>().slotID = i;
          Instance.slots[i].GetComponent<Slot>().SetupSlot(Instance.myBag.itemList[i]);
       }
   }

}
