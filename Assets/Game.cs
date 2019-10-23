using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject centerPanel;
    public GameObject buttonPrefab;

    public Dropdown d;
    public Dropdown d1;
    public InputField inputf;

    public Text money_text;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player(5,0,1000);
        display_properties();
        money_text.text = "Money : " + player.money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void display_properties()
    {
        DeleteButtons();
        foreach (Property p in player.properties)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.position = leftPanel.transform.position;
            //button.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            button.transform.SetParent(leftPanel.transform, false);

            Button tempButton = button.GetComponent<Button>();
            tempButton.onClick.AddListener(() => Click());
            tempButton.GetComponentInChildren<Text>().text = p.name;

            Debug.Log("value " + p.value
                + "name " + p.name
                + "type " + p.type 
                + "sub " + p.sub_type
                + "size " + p.size
                );
        }
    }

    void DeleteButtons()
    {
        Transform panelTransform = GameObject.Find("LeftContent").transform;
        foreach (Transform child in panelTransform)
        {
            if(child.name == "Button(Clone)")
            {
                Destroy(child.gameObject);
            }
            
        }
    }

    void Click()
    {
        Debug.Log("now display property details in center panel");
    }

    void CreateProperty(string prop_type, string prop_sub_type, string size)
    {
        player.properties.Add(new Property(prop_type, prop_sub_type, int.Parse(size)));
        display_properties();
    }

    //dropdown d calls this
    public void OnValueChanged()
    {
        List<string> str = new List<string>();
        d1.ClearOptions();
        if (d.options[d.value].text.Equals("Orchard"))
        {
            str.Add("pear");
            str.Add("apple");
            str.Add("plum");
            str.Add("citrus");
            str.Add("olive");
            str.Add("date");
            str.Add("fig");
            str.Add("orange");
            str.Add("pecan");
            str.Add("cashews");
            str.Add("almonds");
        } else if (d.options[d.value].text.Equals("Livestock"))
        {
            str.Add("cattle");
            str.Add("goat");
            str.Add("sheep");
            str.Add("swine");
            str.Add("poultry");
        } else if (d.options[d.value].text.Equals("Aquaculture"))
        {
            str.Add("carp");
            str.Add("salmon");
            str.Add("tilapia");
            str.Add("catfish");
            str.Add("microalgea");
            str.Add("shrimp");
            str.Add("prawn");
            str.Add("oyster");
            str.Add("mussel");
        }
        else if (d.options[d.value].text.Equals("Electricity"))
        {
            str.Add("wind");
            str.Add("solar");
        }
        d1.AddOptions(str);
    }

    //Accept button calls this
    public void OnAccept()
    {
        //TODO check if player has enough money
        CreateProperty(d.options[d.value].text, //property type
            d1.options[d1.value].text,          //property sub type
            inputf.text);                        //size
        Transform t = GameObject.Find("Create_Property_Scroll_View").transform;
        t.GetComponent<CanvasGroup>().alpha = 0.0f;

        Update_money();
    }

    void Update_money()
    {
        Debug.Log("player.money " + player.money);
        player.money -= player.properties[player.properties.Count-1].value; //find the last added property to properties list and sub its' value
        Debug.Log(player.properties[player.properties.Count - 1].value);

        money_text.text = "Money : " + player.money;
    }

    //create_property button calls this
    public void Create_Property_Button()
    {
        Transform t = GameObject.Find("Create_Property_Scroll_View").transform;
        t.GetComponent<CanvasGroup>().alpha = 1.0f;
    }
}
