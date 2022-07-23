using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All
#pragma warning disable CS0169
#pragma warning disable CS0414

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image m_timeFill;
        private float m_startTime = 120;
        private float currentTime;

        private void Start()
        {
            currentTime = m_startTime;
        }

        private void Update()
        {
            currentTime -= Time.deltaTime;
            m_timeFill.fillAmount = currentTime / m_startTime;
        }

        public void RandomBtnClick()
        {
            CreateLevel.Instance.RandomImage();
        }
        
    }
}