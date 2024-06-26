﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TabGroup : MonoBehaviour {

    public List<TabButton> tabButtons;
    public Color tabIdle, tabHover, tabActive;

    public List<GameObject> objectsToSwap;
    public TabButton selectedTab;

    public UnityEvent OnTabSelectedAction;

    public void Subscribe(TabButton button) {
        if (tabButtons == null) {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }
    
    public void OnTabEnter(TabButton button) {
        ResetTabs();
        if(selectedTab == null || button!=selectedTab)
            button.background.color = tabHover;
    }

    public void OnTabExit(TabButton button) {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button) {
        OnTabSelectedAction.Invoke();
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<objectsToSwap.Count; i++){
            if(i==index){
                objectsToSwap[i].SetActive(true);
            }else{
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() {
        foreach (TabButton button in tabButtons) {
            if(selectedTab!= null && selectedTab==button) continue;
                button.background.color = tabIdle;
        }
    }
}