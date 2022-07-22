using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private static int countClick;
        private static GameManager m_instance;
        private Image m_image1, m_image2;
        private bool m_clickTwoBtn;
        private bool m_draw = false;

        public static GameManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<GameManager>();
                }

                return m_instance;
            }
        }

        private void Awake()
        {
            countClick = 0;

            if (m_instance == null)
            {
                m_instance = this;
            }
            else if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (countClick == 2)
            {
                countClick = 0;
                m_clickTwoBtn = false;
            }
        }

        public void SetCountClick(Image image)
        {
            countClick++;
            if (countClick == 1)
            {
                m_image1 = image;
                Debug.Log("Image 1 : " + m_image1.sprite.name);
            }
            else if (countClick == 2)
            {
                m_image2 = image;
                Debug.Log("Image 2 : " + m_image2.sprite.name);
                m_clickTwoBtn = true;
            }

            if (m_clickTwoBtn == true)
            {
                if (m_image1.sprite.name == m_image2.sprite.name && m_image1 != m_image2)
                {
                    m_draw = true;
                    CreateLevel.Instance.ReleaseImg(m_image1, m_image2);
                    Destroy(m_image1);
                    Destroy(m_image2);
                }
                else
                {
                    SetNullImage();
                }
            }

            Debug.Log(countClick);
        }

        public int GetCountClick(Image image)
        {
            return countClick;
        }

        private void SetNullImage()
        {
            m_image1 = null;
            m_image2 = null;
        }

        private void OnDrawGizmos()
        {
        }
    }
}