using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using CodeMonkey.Utils;
using System;



public class Game : MonoBehaviour
{
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject centerPanel;
    public GameObject workerPanel;
    public GameObject buttonPrefab;
    public GameObject imagePrefab;
    public GameObject propertyObject;
    public GameObject jobObject;
    public GameObject hireWorkersObject;
    public GameObject hiredWorkersPanel;
    public GameObject buyStorageObject;
    public GameObject seasonTextObject;
    public GameObject bottomRightContent;

    public Dropdown d;
    public Dropdown d1;

    public InputField inputf;
    public InputField storageAmountInputField;

    public Text Center_value;
    public Text Center_name;
    public Text Center_type;
    public Text Center_sub;
    public Text Center_size;
    public Text Center_harvesting;
    public Text resource_amount;
    public Text money_text;

    public Dropdown jobDropdown;
    public Dropdown jobDropdown2;
    public Dropdown jobDropdown3;
    public Dropdown storageTypeDropdown;

    Player player;
    bool run_timers;
    List<Image> progressImgList;
    List<Worker> avail_workers;
    float end_day;
    Market market;
    Seasons season;

    //WindowGraph
    public Sprite dotSprite;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private RectTransform graphContainer;
    private GameObject tooltipGameObject;
    private static Game instance;
    private List<GameObject> gameObjectList;
    public GameObject marketObject;
    public GameObject graphContainerObject;
    public GameObject marketButton;
    public Dropdown marketDropDown;


    //WindowGraph Cache variables
    List<float> valueList;
    IGraphVisual graphVisual;
    Func<int, string> getAxisLabelX;
    Func<float, string> getAxisLabelY;

    //Consts
    const float DAY = 600;  //6 min in secs
    const int MAX_AVAIL_WORKERS = 5;
    const int STORAGE_UNIT_COST = 1000;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player(5,0,10000);
        money_text.text = "Money : " + player.money;
        run_timers = false;
        progressImgList = new List<Image>();
        avail_workers = new List<Worker>();
        end_day = 0;
        Shuffle_Workers();  //initialize available worker list
        propertyObject.SetActive(false);
        jobObject.SetActive(false);
        hireWorkersObject.SetActive(false);
        market = new Market();
        market.UpdateMarket();
        market.UpdateMarket();
        market.UpdateMarket();
        market.UpdateMarket();
        season = new Seasons(seasonTextObject);

        //WindowGraph *************************************************************************
        instance = this;
        //graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        graphContainer = graphContainerObject.GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        //marketObject = transform.Find("MarketGraphObject").GetComponent<GameObject>();

        gameObjectList = new List<GameObject>();

        //To Delete
        IGraphVisual graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, new Color(1, 1, 1, .5f));
        //List<float> valueList = new List<float>() { 5, 32, 45, 56, 34, 23, 13, 2, 150, 34, 6, 34, 65, 2, 45 };
        List<float> valueList = CopyArrayToList(market.pastValues, 0);
        ShowGraph(valueList, graphVisual, (int _i) => "Day " + (_i), (float _f) => "€" + Mathf.RoundToInt(_f));
        marketButton.GetComponent<Button_UI>().ClickFunc = () =>
        {
            SetGraphVisual(graphVisual);
            marketDropDown.ClearOptions();
            List<string> str = new List<string>();
            int count = Enum.GetNames(typeof(RESOURCE_TYPE)).Length;
            for (int i = 0; i < count; i++)
            {
                str.Add(Enum.GetName(typeof(RESOURCE_TYPE), i));
            }
            marketDropDown.AddOptions(str);
            UpdateTextValue();
            marketObject.SetActive(true);
        };

        tooltipGameObject = graphContainer.Find("Tooltip").gameObject;
        //*************************************************************************************

    }

    private List<float> CopyArrayToList(float[,] array, int selected_dim)
    {
        List<float> list = new List<float>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(array[selected_dim,i]);
        }
        return list;
    }

    // Update is called once per frame
    void Update()
    {
        float d = Time.deltaTime;
        end_day += d;
        if(end_day >= DAY)
        {
            end_day = 0;
            Shuffle_Workers();  //change available workers
            market.UpdateMarket();
            season.CheckSeason(seasonTextObject);
        }

        //Timers for jobs
        if (run_timers)
        {
            float f = Time.deltaTime;
            for(int j = 0; j < player.jobs.Count; j++)
            {
                player.jobs[j].time_left -= f;
                if(player.jobs[j].time_left <= 0)
                {
                    //timer elapsed
                    //free worker and delete job button and image
                    for (int i = 0; i < avail_workers.Count; i++)
                    {
                        if (avail_workers[i].job_name == player.jobs[j].job_name)
                        {
                            avail_workers[i].is_working = false;
                        }
                    }
                    //Destroy Job Button & Image
                    Transform panelTransform = GameObject.Find("RightContent").transform;
                    foreach (Transform child in panelTransform)
                    {
                        if(child.name == player.jobs[j].job_name)
                        {
                            Destroy(child.gameObject);
                        }
                        if(child.name == player.jobs[j].job_name + " img")
                        {
                            JobResult(player.jobs[j]);
                            Destroy(child.gameObject);
                            player.jobs.RemoveAt(j);
                            progressImgList.RemoveAt(j);
                            break;
                        }
                    }
                }
                else
                {
                    //Debug.Log("1");
                    //timer not elapsed - update progress bar
                    if (progressImgList.Count == 0)
                    {
                        //if no images are left dissable timers
                        run_timers = false;
                    }
                    else
                    {
                        foreach (Image i in progressImgList)
                        {
                            //Debug.Log("2");
                            if (i.name == (player.jobs[j].job_name + " img"))
                            {
                                //Debug.Log("3");
                                var percent = player.jobs[j].time_left / player.jobs[j].work;
                                i.fillAmount = Mathf.Lerp(0, 1, percent);
                            }
                        }
                    }
                }
            }
        }
    }
    //STORAGE ************************************************************************
    public void AddOptions()
    {
        int count = Enum.GetNames(typeof(RESOURCE_TYPE)).Length;
        List<string> str = new List<string>();
        storageTypeDropdown.ClearOptions();
        for (int i = 0; i < count; i++)
        {
            str.Add(Enum.GetName(typeof(RESOURCE_TYPE), i));
        }
        storageTypeDropdown.AddOptions(str);
    }
    //Buy_Storage_Button calls this
    public void ShowStorageObject()
    {
        AddOptions();
        buyStorageObject.SetActive(true);
    }
    //Cancel_Storage_Button calls this
    public void HideStorageObject()
    {
        buyStorageObject.SetActive(false);
    }
    //Accept_Storage_Button calls this
    public void AcceptStorage()
    {
        AddStorage();
        AddStorageButtons();
        buyStorageObject.SetActive(false);
    }
    private void AddStorageButtons()
    {
        Transform panelTransform = bottomRightContent.transform;
        foreach (Transform child in panelTransform)
        {
            if((child.name != "Buy_Storage_Button") && (child.name != "Text"))
            {
                Destroy(child.gameObject);
            }

        }
        foreach  (Storage st in player.storages)
        {
            //Create button
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.position = bottomRightContent.transform.position;
            button.transform.SetParent(bottomRightContent.transform, false);

            Button tempButton = button.GetComponent<Button>();
            tempButton.onClick.AddListener(() => StorageButtonClick(st.rt));
            tempButton.GetComponentInChildren<Text>().text = Enum.GetName(typeof(RESOURCE_TYPE), (int)st.rt) + " storage";
            tempButton.name = Enum.GetName(typeof(RESOURCE_TYPE), (int)st.rt);
        }
    }
    private void StorageButtonClick(RESOURCE_TYPE rt)
    {
        foreach (Storage st in player.storages)
        {
            if(st.rt == rt)
            {
                Center_value.text = "";
                Center_name.text = "";
                Center_type.text = "Type: " + Enum.GetName(typeof(RESOURCE_TYPE), (int)st.rt);
                Center_sub.text = "Amount: " + st.amount.ToString();
                Center_size.text = "Total size: " + (st.size * st.amount).ToString();
                Center_harvesting.text = "Resource held amount: " + st.held_resource_amount;
                break;
            }
        }
    }
    private void AddStorage()
    {
        bool flag = true;
        int amount = int.Parse(storageAmountInputField.text);
        RESOURCE_TYPE rt = (RESOURCE_TYPE)System.Enum.Parse(typeof(RESOURCE_TYPE), storageTypeDropdown.options[storageTypeDropdown.value].text);
        foreach (Storage st in player.storages)
        {
            if (st.rt == rt)
            {
                //Storage exists update amount
                flag = false;
                if((amount * STORAGE_UNIT_COST) <= player.money)
                {
                    player.money -= amount * STORAGE_UNIT_COST;
                    st.amount += amount;
                }
                break;
            }
        }
        if (flag == true)
        {
            //Storage doesn't exist - create it
            if ((amount * STORAGE_UNIT_COST) <= player.money)
            {
                player.money -= amount * STORAGE_UNIT_COST;
                player.storages.Add(new Storage(amount, rt));
            }
        }
        money_text.text = "Money: " + player.money;
    }
    //********************************************************************************
    //WINDOWGRAPH ********************************************************************
    private void UpdateTextValue()
    {
        RESOURCE_TYPE tmp = (RESOURCE_TYPE)Enum.Parse(typeof(RESOURCE_TYPE), marketDropDown.options[marketDropDown.value].text, true);
        int index = -1;
        for (int i = 0; i < player.storages.Count; i++)
        {
            if (player.storages[i].rt == tmp)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            //At least one storage exists
            resource_amount.text = player.storages[index].held_resource_amount.ToString();
        }
        else
        {
            resource_amount.text = "No Storage unit";
        }
    }
    public void OnMarketDropValueChanged()
    {
        RESOURCE_TYPE tmp = (RESOURCE_TYPE)Enum.Parse(typeof(RESOURCE_TYPE), marketDropDown.options[marketDropDown.value].text, true);
        this.valueList = CopyArrayToList(market.pastValues, (int) tmp);
        SetGraphVisual(graphVisual);
        UpdateTextValue();        
    }
    private void SetGraphVisual(IGraphVisual graphVisual)
    {
        ShowGraph(this.valueList, graphVisual, this.getAxisLabelX, this.getAxisLabelY);
    }

    //Call this to Draw graph
    private void ShowGraph(List<float> valueList, IGraphVisual graphVisual, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        this.valueList = valueList;
        this.graphVisual = graphVisual;
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMaximum = valueList[0];
        float yMinimum = valueList[0];
        foreach (float value in valueList)
        {
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }
        yMaximum = yMaximum + ((yMaximum - yMinimum) * 0.2f);
        yMinimum = yMinimum - ((yMaximum - yMinimum) * 0.2f);


        float xSize = graphWidth / valueList.Count;
        //LineGraphVisual lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, new Color(1, 1, 1, .5f));
        GameObject lastDotGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));
            /*
            Button_UI circleButtonUI = circleGameObject.AddComponent<Button_UI>();
            circleButtonUI.MouseOverOnceFunc += () =>
            {
                ShowToolTip_static(valueList[i].ToString(), circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            };
            circleButtonUI.MouseOutOnceFunc += () =>
            {
                HideToolTip_static();
            };
            */

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -7f);
            gameObjectList.Add(dashX.gameObject);
        }
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);
        }
    }


    private void ShowToolTip(string tooltipText, Vector2 anchoredPosition)
    {
        tooltipGameObject.SetActive(true);
        Text tooltipUIText = tooltipGameObject.transform.Find("TooltipText").GetComponent<Text>();
        tooltipUIText.text = tooltipText;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(
            tooltipUIText.preferredWidth + textPaddingSize * 2f,
            tooltipUIText.preferredHeight + textPaddingSize * 2f
        );

        tooltipGameObject.transform.Find("TooltipBackground").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        tooltipGameObject.transform.SetAsLastSibling();
    }
    private void HideToolTip()
    {
        tooltipGameObject.SetActive(false);
    }

    private interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
    }

    private class LineGraphVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Sprite dotSprite;
        public GameObject lastDotGameObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastDotGameObject = null;
        }
        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;
            return gameObjectList;
        }
        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameobject = new GameObject("dot", typeof(Image));
            gameobject.transform.SetParent(graphContainer, false);
            gameobject.GetComponent<Image>().sprite = dotSprite;
            gameobject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameobject;
        }
        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
        {
            GameObject gameobject = new GameObject("dotConnection", typeof(Image));
            gameobject.transform.SetParent(graphContainer, false);
            gameobject.GetComponent<Image>().color = dotConnectionColor;
            RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameobject;
        }
    }

    //Wrappers
    public static void ShowGraph_static(List<float> values)
    {
        try
        {
            //instance.ShowGraph(values, (int _i) => "Day " + (_i + 1), (float _f) => "€" + Mathf.RoundToInt(_f));
            List<float> d = new List<float>() { 4, 34, 45, 43 };
            //instance.ShowGraph(d, (int _i) => "Day " + (_i), (float _f) => "€" + Mathf.RoundToInt(_f));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + e.StackTrace + e.InnerException);
        }

    }
    private static void HideToolTip_static()
    {
        instance.HideToolTip();
    }
    private static void ShowToolTip_static(string tooltipText, Vector2 anchoredPosition)
    {
        instance.ShowToolTip(tooltipText, anchoredPosition);
    }
 
    //Market_Back_Button calls this
    public void HideMarket()
    {
        //Game.ShowGraph_static(market.resource_price_dict.Values.ToList());
        marketObject.SetActive(false);
    }
    //********************************************************************************

    //JOBS ***************************************************************************
    //cancel button from create job calls this
    public void JobResult(Job j)
    {
        if (j.is_harvesting_job == true)
        {
            float amount_harvested = UnityEngine.Random.Range(500, 1000);
            Debug.Log("amount_harvested before: " + amount_harvested);
            j.CalcWorkDone();
            amount_harvested += amount_harvested * j.property.work_completed;
            Debug.Log("amount_harvested after: " + amount_harvested);
            //Reset
            j.property.work_done.Clear();
            j.property.work_completed = 0;
            //Add harvested resources to storage or sell them if overflow
            //string str = Enum.GetName(j.property.resource_type.GetType(), j.property.resource_type);
            int index = -1;
            for (int i = 0; i < player.storages.Count; i++)
            {
                if(j.property.resource_type == player.storages[i].rt)
                {
                    index = i;
                }
            }
            if(index != -1)
            {
                player.storages[index].held_resource_amount += amount_harvested;
                if (player.storages[index].held_resource_amount > (player.storages[index].amount * player.storages[index].size))
                {
                    //Resource Overflow - sell excess to market
                    float tmp = player.storages[index].held_resource_amount - (player.storages[index].amount * player.storages[index].size);
                    player.storages[index].held_resource_amount = player.storages[index].amount * player.storages[index].size;
                    player.money += (int)(tmp * market.resource_price_dict[j.property.resource_type]);
                    money_text.text = "Money: " + player.money;
                }
            }
            else
            {
                // No storage bought sell all directly to market
                player.money += (int)(amount_harvested * market.resource_price_dict[j.property.resource_type]);
                money_text.text = "Money: " + player.money;
            }
        }
        else
        {
            j.WorkDone();
        }
    }
    public void OnCancelJob()
    {
        jobObject.SetActive(false);
    }
    //Create_Job_Button calls this
    public void OnCreateJob()
    {
        FindProperties();
        FindHiredWorkers();
        jobObject.SetActive(true);

    }
    void FindHiredWorkers()
    {
        List<string> str = new List<string>();
        jobDropdown3.ClearOptions();
        foreach (Worker w in player.workers)
        {
            if(w.is_working == false)
            {
                str.Add(w.name);
            }
        }
        jobDropdown3.AddOptions(str);
    }
    void FindProperties()
    {
        List<string> str = new List<string>();
        jobDropdown.ClearOptions();
        foreach (Property p in player.properties)
        {
            str.Add(p.name);
        }
        str.Add("None");
        jobDropdown.AddOptions(str);
    }
    //jobDropdown calls this
    public void OnValueChangedJob()
    {
        Property p = null;
        foreach (Property prop in player.properties)
        {
            if (prop.name == jobDropdown.options[jobDropdown.value].text)
            {
                p = prop;
                break;
            }
        }
        jobDropdown2.ClearOptions();
        List<string> str = new List<string>();
        //TODO Add the else if's
        if(jobDropdown.options[jobDropdown.value].text.Contains("Orchard"))
        {
            str.Add("pruning");
            str.Add("fertilizing");
            str.Add("spraying");
            if(((int)p.harvestingPeriod == (int)season.curSeason) || ((int)p.harvestingPeriod == 4))
            {
                str.Add("harvesting");
            }
        }else if (jobDropdown.options[jobDropdown.value].text.Contains("Livestock"))
        {
            str.Add("wrangling");
            str.Add("cleaning");
            str.Add("feeding");
            str.Add("doctoring");
            if (((int)p.harvestingPeriod == (int)season.curSeason) || ((int)p.harvestingPeriod == 4))
            {
                str.Add("harvesting");
            }
        }
        else if (jobDropdown.options[jobDropdown.value].text.Contains("Aquaculture"))
        {
            str.Add("cleaning");
            str.Add("feeding");
            str.Add("maintenance");
            if (((int)p.harvestingPeriod == (int)season.curSeason) || ((int)p.harvestingPeriod == 4))
            {
                str.Add("harvesting");
            }
        }
        jobDropdown2.AddOptions(str);
    }
    //Accept button calls this from job tab
    public void OnAcceptJob()
    {
        Property p = null;
        foreach (Property prop in player.properties)
        {
            if (prop.name == jobDropdown.options[jobDropdown.value].text)
            {
                p = prop;
                break;
            }
        }
        for (int i = 0; i < avail_workers.Count; i++)
        {
            if (jobDropdown3.options[jobDropdown3.value].text == avail_workers[i].name)
            {
                if (avail_workers[i].is_working == false)
                {
                    //TODO check if enough money?
                    player.jobs.Add(new Job(jobDropdown.options[jobDropdown.value].text,     //type
                        jobDropdown2.options[jobDropdown2.value].text,                //sub_type
                        jobDropdown.options[jobDropdown.value].text + " " + jobDropdown2.options[jobDropdown2.value].text,  //name
                        p   //the selected property
                        ));
                    avail_workers[i].is_working = true;
                    avail_workers[i].job_name = player.jobs[player.jobs.Count - 1].job_name;

                    foreach (Job j1 in player.jobs)
                    {
                        Debug.Log("name " + j1.job_name + " type " + j1.type + " sub type " + j1.sub_type + " work " + j1.work);
                    }
                    jobObject.SetActive(false);
                    displayJobs();  //generate job buttons
                }
                break;
            }
        }
        

    }

    public void displayJobs()
    {
        foreach (Job j in player.jobs)
        {
            if(j.is_displayed == false)
            {
                //Create button
                GameObject button = (GameObject)Instantiate(buttonPrefab);
                button.transform.position = rightPanel.transform.position;
                button.transform.SetParent(rightPanel.transform, false);

                Button tempButton = button.GetComponent<Button>();
                tempButton.onClick.AddListener(() => JobButtonClick(tempButton.GetComponentInChildren<Text>().text));
                tempButton.GetComponentInChildren<Text>().text = j.job_name;
                tempButton.name = j.job_name;

                //Create Progress Image
                GameObject img = (GameObject)Instantiate(imagePrefab);
                img.transform.position = rightPanel.transform.position;
                img.transform.SetParent(rightPanel.transform, false);
                img.name = j.job_name + " img";
                Image i = img.GetComponent<Image>();
                progressImgList.Add(i);
                run_timers = true;
                j.is_displayed = true;
            }
        }
    }
    public void JobButtonClick(string name)
    {
        foreach (Job j in player.jobs)
        {
            if (j.job_name == name)
            {
                Center_value.text = "";
                Center_name.text = "Name: " + j.job_name;
                Center_type.text = "Type: " + j.type;
                Center_sub.text = "Sub: " + j.sub_type;
                Center_size.text = "";
                Center_harvesting.text = "";
                break;
            }
        }
    }
    //****************************************************************
    //Hire Workers ***************************************************
    //Hire_Workers_Button calls this
    public void Hire_Workers_Click()
    {
        Display_Avail_Workers();
        hireWorkersObject.SetActive(true);
    }
    void Display_Avail_Workers()
    {
        //Delete previous buttons
        Transform panelTransform = workerPanel.transform;
        foreach (Transform child in panelTransform)
        {
            if (child.name != "Cancel_Button")
            {
                Destroy(child.gameObject);
            }

        }
        foreach (Worker w in avail_workers)
        {
            if (w.is_hired == false)
            {
                //Create button
                GameObject button = (GameObject)Instantiate(buttonPrefab);
                button.transform.position = workerPanel.transform.position;
                button.transform.SetParent(workerPanel.transform, false);

                Button tempButton = button.GetComponent<Button>();
                tempButton.onClick.AddListener(() => Hire_Worker_Button_Click(tempButton.GetComponentInChildren<Text>().text));
                tempButton.GetComponentInChildren<Text>().text = w.name;
                tempButton.name = w.name;
            }
        }
    }
    public void Hire_Worker_Button_Click(string name)
    {
        foreach (Worker w in avail_workers)
        {
            if (w.name == name)
            {
                player.workers.Add(w);
                w.is_hired = true;
            }
        }
        UpdateHiredWorkers();
        hireWorkersObject.SetActive(false);
    }
    private void UpdateHiredWorkers()
    {
        Transform panelTransform = hiredWorkersPanel.transform;
        foreach (Transform child in panelTransform)
        {
            if ((child.name != "Hire_workers_button") && (child.name != "Text"))
            {
                Destroy(child.gameObject);
            }

        }
        foreach (Worker w in player.workers)
        {
            //Create button
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.position = hiredWorkersPanel.transform.position;
            button.transform.SetParent(hiredWorkersPanel.transform, false);

            Button tempButton = button.GetComponent<Button>();
            tempButton.onClick.AddListener(() => Worker_Button_Click(tempButton.GetComponentInChildren<Text>().text));
            tempButton.GetComponentInChildren<Text>().text = w.name;
            tempButton.name = w.name;
        }
    }
    public void Worker_Button_Click(string name)
    {
        foreach (Worker w in player.workers)
        {
            if(w.name == name)
            {
                Center_value.text = "Is_Wokring: " + w.is_working;
                Center_name.text = "Name: " + w.name;
                Center_type.text = "";
                Center_sub.text = "";
                Center_size.text = "";
                Center_harvesting.text = "";
            }
        }

    }
    //Cancel_button from hire_workers_scroll_view calls this
    public void Cancel_Click()
    {
        hireWorkersObject.SetActive(false);
    }
    //Changes available workers
    public void Shuffle_Workers()
    {
        for (int i = 0; i < avail_workers.Count; i++)
        {
            if(avail_workers[i].is_hired == false)
            {
                avail_workers.RemoveAt(i);
            }
        }
        for (int i = 0; i < MAX_AVAIL_WORKERS; i++)
        {
            avail_workers.Add(new Worker());
        }
    }
    //********************************************************************************

    //PROPERTIES *********************************************************************
    //Displays the selected property stats in the center panel
    void PropButtonClick(string name)
    {
        foreach (Property p in player.properties)
        {
            if (p.name == name)
            {
                Center_value.text = "Value: " + p.value;
                Center_name.text = "Name: " + p.name;
                Center_type.text = "Type: " + p.type;
                Center_sub.text = "Sub: " + p.sub_type;
                Center_size.text = "Size: " + p.size;
                Center_harvesting.text = "Harvesting Period: " + p.harvestingPeriod.ToString();
                break;
            }
        }
    }
    //dropdown d calls this from property tab
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
    void DeleteButtons(string obj)
    {
        Transform panelTransform = GameObject.Find(obj).transform;
        foreach (Transform child in panelTransform)
        {
            if (child.name == "Button(Clone)")
            {
                Destroy(child.gameObject);
            }

        }
    }
    void display_properties()
    {
        DeleteButtons("LeftContent");
        foreach (Property p in player.properties)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.position = leftPanel.transform.position;
            //button.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            button.transform.SetParent(leftPanel.transform, false);

            Button tempButton = button.GetComponent<Button>();
            tempButton.onClick.AddListener(() => PropButtonClick(tempButton.GetComponentInChildren<Text>().text));
            tempButton.GetComponentInChildren<Text>().text = p.name;

            Debug.Log("value " + p.value
                + "name " + p.name
                + "type " + p.type
                + "sub " + p.sub_type
                + "size " + p.size
                );
        }
    }
    void CreateProperty(string prop_type, string prop_sub_type, string size)
    {
        player.properties.Add(new Property(prop_type, prop_sub_type, int.Parse(size)));
        //display_properties();
    }
    //Cancel button calls this from property tab
    public void OnCancel()
    {
        //Transform t = GameObject.Find("Create_Property_Scroll_View").transform;
        //t.GetComponent<CanvasGroup>().alpha = 0.0f;
        //t.GetComponent<CanvasGroup>().interactable = false;
        propertyObject.SetActive(false);
    }
    //Accept button calls this from property tab
    public void OnAccept()
    {
        //TODO check the arguments first
        CreateProperty(d.options[d.value].text, //property type
            d1.options[d1.value].text,          //property sub type
            inputf.text);                       //size

        if(player.money - player.properties[player.properties.Count - 1].value >= 0)
        {
            Update_money();
        }
        else
        {
            //remove the just added property from the list
            player.properties.RemoveAt(player.properties.Count - 1);
        }

        //Transform t = GameObject.Find("Create_Property_Scroll_View").transform;
        //t.GetComponent<CanvasGroup>().alpha = 0.0f;
        propertyObject.SetActive(false);
        display_properties();
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
        //Transform t = propertyObject.Find("Create_Property_Scroll_View").transform;
        //t.GetComponent<CanvasGroup>().alpha = 1.0f;
        //t.GetComponent<CanvasGroup>().interactable = true;
        //propertyScrollView.GetComponent<CanvasGroup>().alpha = 1.0f;
        propertyObject.SetActive(true);
    }
    //********************************************************************************
}
