using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityClick : MonoBehaviour
{
    public GameObject cityScrollView;
    public Text nameText;
    public Text populationText;
    public Text prosperityText;
    public List<GameObject> regionsObject;
    List<Region> regionsList;

    void Start()
    {
        cityScrollView.SetActive(false);
        regionsList = new List<Region>();
        foreach (GameObject go in regionsObject)
        {
            regionsList.Add(new Region(go));
        }
    }

    void OnMouseDown()
    {
        Debug.Log("1");
        foreach (Region r in regionsList)
        {
            Debug.Log("2");
            foreach (City c in r.cities)
            {
                Debug.Log("3");
                if (c.go == gameObject)
                {
                    nameText.text = c.name;
                    populationText.text = c.population.ToString();
                    prosperityText.text = c.prosperity.ToString();
                    break;
                }
            }
        }

        //City c = (City)gameObject;
        //nameText.text = c.name;
        //populationText.text = c.population.ToString();
        //prosperityText.text =        
        cityScrollView.SetActive(true);
    }

    public void CityInfoAcceptButton()
    {
        cityScrollView.SetActive(false);
    }
}
