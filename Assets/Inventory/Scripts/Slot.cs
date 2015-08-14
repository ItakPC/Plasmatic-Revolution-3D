using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack<Item> items;

    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHighlighted;

    public bool IsEmpty {
        get { return Items.Count == 0; }
    }

    public Item CurrentItem {
        get { return Items.Peek(); }
    }

    public bool IsAvaible {
        get { return CurrentItem.maxStackSize > Items.Count;  }
    }

    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    void Awake() {
        Items = new Stack<Item>();
    }
    void Start () {
     
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.6);
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        
    }
	
	void Update () {
	
	}

    public void AddItem(Item item) {
        Items.Push(item);

        if (Items.Count > 1)
        {
            stackTxt.text = Items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);

    }

    public void AddItems(Stack<Item> items) {
        this.Items = new Stack<Item>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutralSprite, Sprite highlightedSprite) {
        GetComponent<Image>().sprite = neutralSprite;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlightedSprite;
        st.pressedSprite = neutralSprite;

        GetComponent<Button>().spriteState = st;   
    }

    private void UseItem() {
        if (!IsEmpty)
        {
            Items.Pop().Use();

            stackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

            if (IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlighted);

                Inventory.EmptySlots++;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover"))
        {
            UseItem();

        }
    }

    public void ClearSlot() {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlighted);
        stackTxt.text = string.Empty;
    }
}

