﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    Image healthBar;
    float maxHealth = 30f;
    public static float health;
    

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        health = maxHealth;
       

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;

       
    }
}
