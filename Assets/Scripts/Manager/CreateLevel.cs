using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ReSharper disable All
#pragma warning disable CS0169

namespace Manager
{
    public class CreateLevel : MonoBehaviour
    {
        private static CreateLevel m_instance;

        public static CreateLevel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<CreateLevel>();
                }

                return m_instance;
            }
        }
        [SerializeField] private Image m_listBtn;
        [SerializeField] private int m_col;
        [SerializeField] private int m_row;
        [SerializeField] private List<Sprite> m_listSprite;
        public List<Sprite> m_list;
        public List<Image> m_allImage;
        
        private void Awake()
        {
            CheckImage();
            if (m_instance == null)
            {
                m_instance = this;
            }
            else if (m_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void RandomImage()
        {
            List<Sprite> m_newList = new List<Sprite>();
            foreach (var lst in m_allImage)
            {
                m_newList.Add(lst.sprite);
            }

            for (int i = 0; i < m_allImage.Count; i++)
            {
                m_allImage[i].sprite = m_newList[Random.Range(0, m_newList.Count)];
            }
        }

        private void CheckImage()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < m_col; j++)
                {
                    m_list.Add(m_listSprite[j]);
                }
            }

            for (int i = 0; i < m_col; i++)
            {
                for (int j = 0; j < m_row; j++)
                {
                    var num = Random.Range(0, m_list.Count);
                    m_listBtn.sprite = m_list[num];
                    m_list.Remove(m_list[num]);
                    Image obj = GameObject.Instantiate(m_listBtn, transform);
                    m_allImage.Add(obj);
                }
            }
        }
        // if destroy
        public void ReleaseImg(Image img1, Image img2)
        {
            m_allImage.Remove(img1);
            m_allImage.Remove(img2);
        }
    }
}