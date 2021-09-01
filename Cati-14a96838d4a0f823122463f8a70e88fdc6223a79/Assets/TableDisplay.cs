using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDisplay : MonoBehaviour
{

    public Evaluation evaluation;

    public GameObject largeTable;
    public GameObject smallTable;
   
    void Start()
    {

        if (evaluation.condition == ConditionType.Veridical || evaluation.condition == ConditionType.SideToSide)
        {
            largeTable.SetActive(true);
            smallTable.SetActive(false);
        }

        else // condition == ConditionType.Approach
        {
            largeTable.SetActive(true);
            smallTable.SetActive(false);
        }
    }

}