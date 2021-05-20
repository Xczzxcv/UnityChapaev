using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputScript : MonoBehaviour
{
    [SerializeField] private GameObject mainmenuObj;
    [SerializeField] private GameObject optionsObj;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            mainmenuObj.SetActive(true);
            optionsObj.SetActive(false);
		}
    }
}
