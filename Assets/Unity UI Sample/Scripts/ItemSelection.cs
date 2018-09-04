using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {

    public Image SelectionImage;
    public List<Sprite> ItemList = new List<Sprite>();
    private int itemSpot = 0;


    public void RightSelection()
    {
        if (itemSpot < ItemList.Count - 1)
        {
            itemSpot++;
            SelectionImage.sprite = ItemList[itemSpot];
        }
    }

    public void LeftSelection()
    {
        if (itemSpot > 0)
        {
            itemSpot--;
            SelectionImage.sprite = ItemList[itemSpot];
        }
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
