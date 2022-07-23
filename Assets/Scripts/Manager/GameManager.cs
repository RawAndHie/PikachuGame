using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private LineRenderer m_lineRenderer;
        private static int countClick;
        private static GameManager m_instance;
        private Image m_image1, m_image2;
        private bool m_clickTwoBtn;

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
            m_lineRenderer = GetComponent<LineRenderer>();
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
            // ở đây kiểm tra lần click 1 và 2
            // 
            countClick++;
            if (countClick == 1)
            {
                m_image1 = image;
            }
            else if (countClick == 2)
            {
                m_image2 = image;
                m_clickTwoBtn = true;
            }

            // kiểm tra img giống nhau hay không
            if (m_clickTwoBtn == true)
            {
                
                if (m_image1.sprite.name == m_image2.sprite.name && m_image1 != m_image2)
                {
                    if (CreateLevel.Instance.ReleaseImg(m_image1, m_image2) == true)
                    {
                        StartCoroutine(RemoveLine());
                    }
                }
                else
                {
                    SetNullImage();
                }
            }

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
        

        private void CheckTop()
        {
            
        }

        private IEnumerator RemoveLine()
        {
            yield return new WaitForSeconds(0.3f);
            // m_lineRenderer.SetPosition(0,new Vector3(0,0,0));
            // m_lineRenderer.SetPosition(1,new Vector3(0,0,1));
            Destroy(m_image1);
            Destroy(m_image2);
        }
    }
}