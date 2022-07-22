using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All

namespace GamePlay
{
    public class GamePlayController : MonoBehaviour
    {
        private GameManager m_gamaManager;
        private Image m_image;
        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_image = GetComponent<Image>();
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_gamaManager = FindObjectOfType<GameManager>();
        }

        public void ClickImage()
        {
            m_gamaManager.SetCountClick(m_image);
        }
    }
}