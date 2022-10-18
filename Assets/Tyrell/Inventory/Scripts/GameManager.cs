using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    

    
    public List<Item> GunItemList = new List<Item>();
    
    
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;

    public Transform mainCanvas;
    public Transform GunInventoryTransform;
    public Transform inventoryTransform;

    public float moveX;
    public float moveY;


    public int upgradeToSpawn = 0;
    

    private void Start()
    {
        Time.timeScale = 1;

    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Inventory.instance.AddItemToGun(GunItemList[upgradeToSpawn]);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            TakeScreenshot();
        }

    }

    

    public void DisplayItemInfo(string itemName, string itemDescription, Vector2 buttonPos)
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }

        buttonPos.x -= moveX;
        buttonPos.y += moveY;

        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, mainCanvas);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
    }

    public void DestroyItemInfo()
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }
    }

    private string directoryName = "Screenshots";
    private string fileName = "TestImage.png";

    public void TakeScreenshot()
    {
        DirectoryInfo screenshotDirectory = Directory.CreateDirectory(directoryName);
        string fullPath = Path.Combine(screenshotDirectory.FullName, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);
    }

}
