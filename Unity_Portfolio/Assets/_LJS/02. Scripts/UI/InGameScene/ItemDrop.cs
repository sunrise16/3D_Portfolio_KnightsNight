using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            ItemDrag.draggingItem.transform.SetParent(this.transform);
            // 슬롯에 추가된 아이템을 GameData에 추가하기 위해 AddItem을 호출
            // Item item = ItemDrag.draggingItem.GetComponent<ItemInfo>().itemData;
            // GameManager.instance.AddItem(item);
        }
    }
}