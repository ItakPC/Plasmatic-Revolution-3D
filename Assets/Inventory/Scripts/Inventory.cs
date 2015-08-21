using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    private RectTransform inventoryRect;
    private RectTransform objectT;


    public CanvasGroup canvasGroup;





    private static GameObject playerRef;

    
    private List<GameObject> allSlots;
   


    private float inventoryWidth, inventoryHeight;
    public int slots;
    public int rows, columns;
    public float slotPaddingLeft, slotPaddingTop, slotPaddingBetween;
    public float slotSize;
    private static int emptySlots;
    private float hoverYOffset;
    private bool fadingIn;
    private bool fadingOut;
    public float fadeTime;

    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }

   

    void Start() {
        playerRef = GameObject.Find("Player");
        canvasGroup = GetComponent<CanvasGroup>();
        CreateLayOut();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !InventoryManager.Instance.EventSystem.IsPointerOverGameObject(-1))
        {
            if (!InventoryManager.Instance.EventSystem.IsPointerOverGameObject(-1) && InventoryManager.Instance.From != null)
            {
                foreach (Item item in InventoryManager.Instance.From.Items)
                {
                    float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);

                    Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

                    v *= 2;

                    Vector3 pos = playerRef.transform.position - v;

                    GameObject tmpDrop = (GameObject)Instantiate(InventoryManager.Instance.DroppedItem, pos, Quaternion.identity);

                    tmpDrop.GetComponent<Item>().SetContent(item);

                }
            }

            InventoryManager.Instance.From.ClearSlot();
            InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
            Destroy(GameObject.Find("Hover"));
            InventoryManager.Instance.To = null;
            InventoryManager.Instance.From = null;
            emptySlots++;
            
        }

        if (InventoryManager.Instance.HoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.Canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.Canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.Canvas.transform.TransformPoint(position);

        }

        
    }
    

    private void CreateLayOut() {
        if (allSlots != null)
        {
            foreach (GameObject go in allSlots)
            {
                Destroy(go);
            }
        }

        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        EmptySlots = slots;

        inventoryWidth = ((columns) * (slotSize) + (columns - 1) * (slotPaddingBetween) + slotPaddingLeft) - 1;
        inventoryHeight = (rows) * (slotPaddingTop + slotSize) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);


        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < columns; x++) {
                GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.SlotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft + (x - 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.Canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.Canvas.scaleFactor);
                newSlot.transform.SetParent(this.transform);

                allSlots.Add(newSlot);
            }
        }
    }

    public void ShowTooltip(GameObject slot) {
        Slot tmpSlot = slot.GetComponent<Slot>();

        if (!tmpSlot.IsEmpty && (InventoryManager.Instance.HoverObject) == null)
        {
            InventoryManager.Instance.TooltipTextObject.text = tmpSlot.CurrentItem.GetTooltip();
            InventoryManager.Instance.TooltipSizeObject.text = InventoryManager.Instance.TooltipTextObject.text;

            InventoryManager.Instance.Tooltip.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;
            InventoryManager.Instance.Tooltip.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideTooltip()
    {
        InventoryManager.Instance.Tooltip.SetActive(false);
    }

    public bool AddItem(Item item) {
        if (item.maxStackSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvaible)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }
            }

            if (EmptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item) {
        if (EmptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    EmptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked) {
        if (InventoryManager.Instance.From == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                InventoryManager.Instance.From = clicked.GetComponent<Slot>();
                InventoryManager.Instance.From.GetComponent<Image>().color = Color.gray;

                InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.IconPrefab);
                InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                InventoryManager.Instance.HoverObject.name = "Hover";

                RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas - Inventory").transform, true);

                InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.From.gameObject.transform.localScale;
            }
        }
        else if (InventoryManager.Instance.To == null)
        {
            InventoryManager.Instance.To = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(InventoryManager.Instance.To.Items);
            InventoryManager.Instance.To.AddItems(InventoryManager.Instance.From.Items);

            if (tmpTo.Count == 0)
            {
                InventoryManager.Instance.From.ClearSlot();
            }
            else
            {
                InventoryManager.Instance.From.AddItems(tmpTo);
            }

            InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
            InventoryManager.Instance.To = null;
            InventoryManager.Instance.From = null;
            InventoryManager.Instance.HoverObject = null;
            //Destroy(GameObject.Find("Hover"));
        }
    }

    public void Open()
    {
        if (canvasGroup.alpha > 0)
        {
            StartCoroutine("FadeOut");
            PutItemBack();
        }
        else
        {
            StartCoroutine("FadeIn");
        }
    }

    public void PutItemBack() {
        if (InventoryManager.Instance.From != null)
        {
            Destroy(GameObject.Find("Hover"));
            InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
            InventoryManager.Instance.From = null;
        }
    }

    private IEnumerator FadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = canvasGroup.alpha;

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 0;
            fadingOut = false;
        }
    }

         private IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine("FadeOut");

            float startAlpha = canvasGroup.alpha;

            float rate = 1.0f / fadeTime;

            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 1;
            fadingIn = false;
        }
    }
}

