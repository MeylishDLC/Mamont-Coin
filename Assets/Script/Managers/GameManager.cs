using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Clicker")] 
    public static int Clicks;
    public static int Multiplier;

    [Header("Interface")] 
    [SerializeField] private GameObject clicker;
    [SerializeField] private GameObject clickerBuffsShop;
    [SerializeField] private GameObject skypeNotification;
    
    private void Start()
    {
        ///////////////
        Clicks = 0;
        Multiplier = 1;
        //////////////
        
        // clicker.SetActive(false);
        // clickerBuffsShop.SetActive(false);
    }
}
