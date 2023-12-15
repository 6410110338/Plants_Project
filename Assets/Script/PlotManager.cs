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

    bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unvailableSprite;

    float speed = 1f;
    public bool isBought = true;

    // Start is called before the first frame update
    void Start()
    {
        plot = GetComponent<SpriteRenderer>();
        fm = transform.parent.GetComponent<FarmManager>();
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(isBought )
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unvailableSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted && !isDry)
        {

            timer -= speed*Time.deltaTime;

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
            if (plantStage == selectPlant.plantStages.Length - 1 && !fm.isPlanting && !fm.isSelecting)
            {
                Harvest();
            }

        }
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money  && isBought)
        {
            Plant(fm.selectPlant.plant);
        }
        if (fm.isSelecting)
        {
            switch (fm.selectedTool) 
            {
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted) UpdatePlant();
                    }
                    break;
                case 2:
                    if (fm.money >= 10 && isBought)
                    {
                        fm.Transaction(-10);
                        if (speed < 2) speed += .2f;
                    }
                    break;
                case 3:
                    if (fm.money >= 100 && !isBought)
                    {
                        fm.Transaction(-100);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;
                default:
                    break;
            }

        }


    }
    private void OnMouseOver()
    {
        if (fm.isPlanting)
        {
            if (isPlanted || fm.selectPlant.plant.buyPrice > fm.money || !isBought)
            {
                plot.color = unavilableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1:
                case 2:
                    if (isBought && fm.money >= (fm.selectedTool - 1)*10)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavilableColor;
                    }
                    break;
                case 3:
                    if (!isBought && fm.money >= 100)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavilableColor;
                    }
                    break;
                default :
                    plot.color = unavilableColor;
                    break;
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
        isDry = true;
        plot.sprite = drySprite;
        speed = 1f;
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
        if (isDry)
        {
            plant.sprite = selectPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectPlant.plantStages[plantStage];
        }
        
    }



}