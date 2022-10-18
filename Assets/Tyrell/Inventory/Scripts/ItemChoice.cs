using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChoice : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    public List<Item> NewitemList = new List<Item>();

    public ItemChooser Items;

    public List<ItemChooser> itemChoiceList = new List<ItemChooser>();

    [SerializeField]
    int totalCount = 3;
    int completedCount = 0;


    private void Start()
    {
        //closes inventory when items show
        InventoryUIHandler.instance.CloseInventory();
    }

    private void Update()
    {

        itemChoiceList.RemoveAll(GameObject => GameObject == null);
        
    }

    public void EndOfWave()
    {
        for(int i = 0; i < totalCount; i++)
        {
            SpawnItems();
            completedCount += 1;
            if (completedCount == totalCount)
            {
                updateitemList();
            }
        }
    }

    public void ShowItems()
    {
        
        for (int i = 0; i < totalCount ; i++)
        {
            SpawnItems();
            completedCount += 1;
            if(completedCount == totalCount)
            {
                updateitemList();
            }
        }
        
    }

    //should reset item list to new item list;
    void updateitemList()
    {
        completedCount = 0;
        itemList.Clear();
        itemList.AddRange(NewitemList);
    }


    //removes item from list so that it cannot be chosen again
    public void RemoveFromList(Item item)
    {
        itemList.Remove(item);
    }

    void SpawnItems()
    {
        ItemChooser itemchooser = Instantiate(Items, this.transform.transform);
        itemChoiceList.Add(itemchooser);
        Item newitem = itemList[Random.Range(0, itemList.Count)];
        itemchooser.AddItem(newitem);
        itemchooser.itemChoice = this;
        //remove item from list so that it cannot be chosen again;
        RemoveFromList(newitem);
    }

    //void CheckRarity()
    //{
    //    int RarityCheck = Random.Range(1, 4);
    //    switch (RarityCheck)
    //    {
    //        case 1:

    //            break;

    //        case 2:

    //            break;

    //        case 3:

    //            break;

    //        case 4:

    //            break;
    //    }
    //}

   public void DestoryItems()
    {
        GameManager.instance.DestroyItemInfo();
        foreach(ItemChooser items in itemChoiceList)
        {
            Destroy(items.gameObject);
            
        }
        

    }

}
