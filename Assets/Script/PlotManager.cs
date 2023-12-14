using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plotmanage : MonoBehaviour
{
    bool isPlanted = false;
    public SpriteRenderer plant;

    int plantStage = 0;
    PlantObject selectPlant;
    float timer;

    public Color availableColor = Color.green;
    public Color unavilableColor = Color.red;

    SpriteRenderer plot;

    FarmManager fm;

    // Start is called before the first frame update
    void Start()
    {
        plot = GetComponent<SpriteRenderer>();
        fm = transform.parent.GetComponent<FarmManager>();
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted)
        {
            //https://medium.com/star-gazers/understanding-time-deltatime-6528a8c2b5c8

            timer -= Time.deltaTime;

            if (timer < 0 && plantStage < selectPlant.plantStages.Length - 1)
            {
                timer = selectPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (plantStage == selectPlant.plantStages.Length - 1 && !fm.isPlanting)
            {
                Harvest();
            }

        }
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money)
        {
            Plant(fm.selectPlant.plant);
        }


    }
    private void OnMouseOver()
    {
        if (fm.isPlanting)
        {
            if (isPlanted || fm.selectPlant.plant.buyPrice > fm.money)
            {
                plot.color = unavilableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
    }
    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        fm.Transaction(selectPlant.sellPrice);
    }
    void Plant(PlantObject newPlant)
    {
        selectPlant = newPlant;
        isPlanted = true;

        fm.Transaction(-selectPlant.buyPrice);

        plantStage = 0;
        UpdatePlant();
        timer = selectPlant.timeBtwStages;
        plant.gameObject.SetActive(true);

    }
    void UpdatePlant()
    {
        plant.sprite = selectPlant.plantStages[plantStage];
    }



}