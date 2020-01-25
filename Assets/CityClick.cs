using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey.Utils;

public class CityClick : MonoBehaviour
{
    public GameObject cityScrollView;
    public GameObject unlockedCitiesScrollView;
    public GameObject Game;

    public Text nameText;
    public Text populationText;
    public Text prosperityText;

    public List<GameObject> regionsObject;
    List<Region> regionsList;
    float end_day = 0;
    City selectedCity;

    //WINDOWGRAPH *****************
    IGraphVisual graphVisual;
    Func<int, string> getAxisLabelX;
    Func<float, string> getAxisLabelY;
    private List<GameObject> gameObjectList;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    public GameObject graphContainerObject;
    public Sprite dotSprite;
    public GameObject marketObject;
    //*****************************

    const float DAY = 600;  //6 min in secs

    void Start()
    {
        graphContainer = graphContainerObject.GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
        marketObject.SetActive(false);
        selectedCity = null;

        cityScrollView.SetActive(false);
        regionsList = new List<Region>();
        foreach (GameObject go in regionsObject)
        {
            if(go.name == "HomeRegionObject")
            {
                regionsList.Add(new Region(go, true));
            }
            else
            {
                regionsList.Add(new Region(go, false));
            }

        }

        foreach (Region reg in regionsList)
        {
            if(reg.isUnlocked == true)
            {
                reg.go.SetActive(true);
            }
            else
            {
                reg.go.SetActive(false);
            }
        }

        foreach (Region reg in regionsList)
        {

        }


        //TO DELETE
        foreach (Region r in regionsList)
        {
            foreach (City c in r.cities)
            {
                c.market.UpdateMarket();
                c.CheckProsperity();
                c.ConsumeResources();
            }
        }

        foreach (Region r in regionsList)
        {
            foreach (City c in r.cities)
            {
                c.market.UpdateMarket();
                c.CheckProsperity();
                c.ConsumeResources();
            }
        }
    }

    void Update()
    {
        end_day += Time.deltaTime;
        if (end_day >= DAY)
        {
            //Update markets
            foreach (Region r in regionsList)
            {
                foreach (City c in r.cities)
                {
                    c.market.UpdateMarket();
                    c.CheckProsperity();
                    c.ConsumeResources();
                }
            }
            end_day = 0;
        }
    }
    void OnMouseDown()
    {
        bool f = false;
        foreach (Region r in regionsList)
        {
            if(f == true)
            {
                break;
            }
            foreach (City c in r.cities)
            {
                if (c.go == gameObject)
                {
                    nameText.text = c.name;
                    populationText.text = "Population : " + c.population.ToString();
                    prosperityText.text = "Prosperity : " + c.prosperity.ToString();
                    f = true;
                    selectedCity = c;
                    break;
                }
            }
        }    
        cityScrollView.SetActive(true);
    }

    public void CityInfoAcceptButton()
    {
        cityScrollView.SetActive(false);
    }
    //MARKET ******************************************************************
    //Market_Back_Button calls this
    public void OnMarketBackButton()
    {
        marketObject.SetActive(false);
    }
    //*************************************************************************
    //WINDOWGRAPH *************************************************************
    private List<float> CopyArrayToList(float[,] array, int selected_dim)
    {
        List<float> list = new List<float>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(array[selected_dim, i]);
        }
        return list;
    }
    public void MarketClick()
    {
        IGraphVisual graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.blue, new Color(1, 1, 1, .5f));
        List<float> valueList = CopyArrayToList(selectedCity.market.pastValues, 0);
        SetGraphVisual(graphVisual, valueList);
        marketObject.SetActive(true);
    }
    private void SetGraphVisual(IGraphVisual graphVisual, List<float> valueList)
    {
        ShowGraph(valueList, graphVisual);
    }

    //Call this to Draw graph
    private void ShowGraph(List<float> valueList, IGraphVisual graphVisual, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        //this.valueList = valueList;
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
        //*************************************************************************
}
