using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    private Stack<Item> items;

    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHighlighted;

    public bool IsEmpty {
        get { return items.Count == 0; }
    }

	void Start () {
        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.6);
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }
	
	void Update () {
	
	}

    public void AddItem(Item item) {
        items.Push(item);

        if (items.Count > 1)
        {
            stackTxt.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);

    }

    private void ChangeSprite(Sprite neutralSprite, Sprite highlightedSprite) {
        GetComponent<Image>().sprite = neutralSprite;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlightedSprite;
        st.pressedSprite = neutralSprite;

        GetComponent<Button>().spriteState = st;   
    }
}

