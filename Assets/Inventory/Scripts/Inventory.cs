using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    private RectTransform inventoryRect;
    private RectTransform objectT;

    public GameObject slotPrefab;
    public GameObject iconPrefab;
    private static GameObject hoverObject;
    private static GameObject tooltip;
    public GameObject mana;
    public GameObject health;
    public GameObject toolTipObject;
    public Canvas canvas;
    public GameObject droppedItem;
    public CanvasGroup canvasGroup;

    private static GameObject playerRef;

    private static Slot from, to;
    private List<GameObject> allSlots;
    public Text tooltipSizeObject;
    public Text tooltipTextObject;
    private static Text tooltipSize;
    private static Text tooltipText;
    public EventSystem eventSystem;

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

    public static Text TooltipSize
    {
        get
        {
            return tooltipSize;
        }

        set
        {
            tooltipSize = value;
        }
    }

    public static Text TooltipText
    {
        get
        {
            return tooltipText;
        }

        set
        {
            tooltipText = value;
        }
    }

    void Start() {
        playerRef = GameObject.Find("First Person Controller");
        tooltipSize = tooltipSizeObject;
        tooltipText = tooltipTextObject;
        tooltip = toolTipObject;
        CreateLayOut();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !eventSystem.IsPointerOverGameObject(-1))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                foreach (Item item in from.Items)
                {
                    float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);

                    Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

                    v *= 2;

                    GameObject tmpDrop = (GameObject)GameObject.Instantiate(droppedItem, playerRef.transform.position - v, Quaternion.identity);

                    tmpDrop.GetComponent<Item>().SetContent(item);

                }
            }

            from.ClearSlot();
            from.GetComponent<Image>().color = Color.white;
            Destroy(GameObject.Find("Hover"));
            to = null;
            from = null;
            emptySlots++;
        }

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);

        }

        if (Input.GetKeyDown(KeyCode.E))
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
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";

                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft + (x - 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                newSlot.transform.SetParent(this.transform);

                allSlots.Add(newSlot);
            }
        }
    }

    public void ShowTooltip(GameObject slot) {
        Slot tmpSlot = slot.GetComponent<Slot>();

        if (!tmpSlot.IsEmpty && hoverObject == null)
        {
            TooltipText.text = tmpSlot.CurrentItem.GetTooltip();
            TooltipSize.text = TooltipText.text;

            tooltip.SetActive(true);
            float xPos = slot.transform.position.x + slotPaddingLeft;
            float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - slotPaddingTop;
            tooltip.transform.position = new Vector2(xPos, yPos);
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
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
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);

                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
            //Destroy(GameObject.Find("Hover"));
        }
    }

    private void PutItemBack() {
        if (from != null)
        {
            Destroy(GameObject.Find("Hover"));
            from.GetComponent<Image>().color = Color.white;
            from = null;
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

    public void SaveInventory() {
        string content = string.Empty;

        for (int i = 0; i < allSlots.Count; i++)
        {
            Slot tmp = allSlots[i].GetComponent<Slot>();

            if (!tmp.IsEmpty)
            {
                content += i + "-" + tmp.CurrentItem.ToString() + "-" + tmp.Items.Count.ToString() + ";";
      
            }
        }

        PlayerPrefs.SetString("content", content);
        PlayerPrefs.SetInt("slots", slots);
        PlayerPrefs.SetInt("rows", rows);
        PlayerPrefs.SetInt("columns", columns);
        PlayerPrefs.SetFloat("slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat("slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat("slotPaddingBetween", slotPaddingBetween);
        PlayerPrefs.SetFloat("slotSize", slotSize);
        PlayerPrefs.Save();
    }

    public void LoadInventory() {
        string content = PlayerPrefs.GetString("content");
        slots = PlayerPrefs.GetInt("slots");
        rows = PlayerPrefs.GetInt("rows");
        columns = PlayerPrefs.GetInt("columns");
        slotPaddingLeft = PlayerPrefs.GetFloat("slotPaddingLeft");
        slotPaddingTop = PlayerPrefs.GetFloat("slotPaddingTop");
        slotPaddingBetween = PlayerPrefs.GetFloat("slotPaddingBetween");
        slotSize = PlayerPrefs.GetFloat("slotSize");

        CreateLayOut();

        string[] splitContent = content.Split(';');

        for (int x = 0; x < splitContent.Length - 1; x++)
        {
            string[] splitValues = splitContent[x].Split('-');

            int index = Int32.Parse(splitValues[0]);
            ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]);
            int amount = Int32.Parse(splitValues[2]);

            for (int i = 0; i < amount; i++)
            {
                switch (type)
                {
                    case ItemType.FISH:
                        allSlots[index].GetComponent<Slot>().AddItem(mana.GetComponent<Item>());
                        break;
                    case ItemType.BANDAGE:
                        allSlots[index].GetComponent<Slot>().AddItem(health.GetComponent<Item>());
                        break;
                }
            }
        }
    }
}

