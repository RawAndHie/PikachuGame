using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
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
        private Image[,] m_matrix = new Image[1000, 1000];
        public List<Sprite> m_list;
        public List<Image> m_allImage; // chứa các obj còn lại chưa bị destroy
        private int[] m_rowX = {-1, 0, 0, 1};
        private int[] m_colY = {0, -1, 1, 0};

        public int[,] m_imagePos = new int[1000, 1000];
        // chứa vị trí các obj chưa bị destroy trong ma trận
        // m_imagePos: 0 là đi được,1 là không đi được

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
            // list hình ảnh còn lại
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
            // spawn ra số lượng sprite : 8 dòng , 18 ảnh ~ 18 cột
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    m_list.Add(m_listSprite[j]);
                }
            }

            for (int i = 0; i < m_row; i++)
            {
                for (int j = 0; j < m_col; j++)
                {
                    if (i != 0 && j != 0 && i != 9 && j != 19)
                    {
                        var num = Random.Range(0, m_list.Count);
                        m_listBtn.sprite = m_list[num];
                        m_list.Remove(m_list[num]);
                        m_matrix[i, j] = Instantiate(m_listBtn, transform);
                        m_matrix[i, j].name = i + "," + j;
                        m_imagePos[i, j] = 1;
                        m_allImage.Add(m_matrix[i, j]);
                    } 
                    else if (i == 0 || j == 0 || i == 9 || j == 19)
                    {
                        m_imagePos[i, j] = 0;
                    }
                }
            }
        }

        // if destroy
        public bool ReleaseImg(Image img1, Image img2)
        {
            // lấy vị trí của img trên ma trận
            int index = img1.name.LastIndexOf(",");
            int row = int.Parse(img1.name.Substring(0, index));
            int col = int.Parse(img1.name.Substring(index + 1));
            // m_imagePos[x, y] = 0;
            int index2 = img2.name.LastIndexOf(",");
            int row2 = int.Parse(img2.name.Substring(0, index2));
            int col2 = int.Parse(img2.name.Substring(index2 + 1));
            // m_imagePos[x2, y2] = 0;
            if (AroundCheck(row, col, row2, col2) == true)
            {
                m_imagePos[row, col] = 0;
                m_imagePos[row2, col2] = 0;
                m_allImage.Remove(img1);
                m_allImage.Remove(img2);
                return true;
                // around check trả true tức có thể release 2 vị trí trên
            }
            else if (AroundCheck(row, col, row2, col2) == false)
            {
                int[,] visited = new int[1000, 1000];
                if (CheckLine(row, col, row2, col2, visited) == true)
                {
                    Debug.Log("tao return true");
                    m_imagePos[row, col] = 0;
                    m_imagePos[row2, col2] = 0;
                    m_allImage.Remove(img1);
                    m_allImage.Remove(img2);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // check xungq quanh hoặc check từng vị trí

            // m_allImage.Remove(img1);
            // m_allImage.Remove(img2);
            Debug.Log("false roi");
            return false;
        }

        public bool AroundCheck(int row, int col, int row2, int col2)
        {
            for (int i = 0; i < 4; i++)
            {
                int nextRow = row + m_rowX[i];
                int nextCol = col + m_colY[i];
                //cần xét trường hợp cùng dòng hoặc cột ở lể,
                if (row == 1 && row2 == 1 || row == 8 && row2 == 8 || col == 1 & col2 == 1 || col == 18 && col2 == 18)
                {
                    Debug.Log("1 img hàng ngoài cùng, 1 img tìm được đường đến hàng ngoài cùng");
                    return true;
                }
                // xong
                //cạnh nhau
                else if (nextRow == row2 && nextCol == col2)
                {
                    Debug.Log("O gan theo chieu ngang , doc");
                    return true;
                }
                // xong
                // các trường hợp không gần nhau, không cùng hàng hoặc cột ngoài cùng. sẽ chạ tiếp tìm đường đi được
                // rồi chạy tiếp 2 trường hợp trên hoặc không còn đường đi phù hợp
            }

            // trường hợp không thể đi tiếp -> false
            return false;
        }

        public bool CheckLine(int row, int col, int row2, int col2, int[,] visited)
        {
            // vòng này chạy khi AroundCheck = flase , tức không phải 2 ô liền kề hoặc ngoài rìa
            // kiểm tra nếu chạm vào ô có thể đi được , thì di chuyển qua ô đó
            // lại chạy for từ đầu
            for (int i = 0; i < 4; i++)
            {
                Debug.Log("for lần thứ :" + i);
                int currentRow = row + m_rowX[i];
                int currentCol = col + m_colY[i];
                Debug.Log("col" + currentCol);
                Debug.Log("row" + currentRow);
                // vòng for lấy vị trí 4 ô tiếp theo
                // nếu ô tiếp theo có thể di chuyển được, thì tiếp tục đi
                // khi không đi được nữa, lại chạy tiếp vòng for nếu đang dở
                if (currentRow < 0 || currentRow > 10 || currentCol < 0 || currentCol > 20)
                {
                    Debug.Log("false 1");
                    return false;
                }
                else if (currentRow == row2 && currentCol == col2)
                {
                    Debug.Log("đã đến nơi");
                    return true;
                }
                else if (m_imagePos[currentRow, currentCol] == 0 && visited[currentRow, currentCol] != 1)
                {
                    // phải là vị trí chưa đi nếu không sẽ bị vòng đi vòng lại
                    // chưa đến nơi trì mới cần check đường đi.. check đường đi xong đến nơi trì trả true
                    
                    Debug.Log("đang ở ");
                    Debug.Log("đang ở : [" + currentRow + "][" + currentCol + "]");
                    visited[currentRow, currentCol] = 1;
                    if (CheckLine(currentRow, currentCol, row2, col2, visited))
                    {
                        return true;
                    }
                    else
                    {
                        Debug.Log("tao bị kẹt rồi");
                        Debug.Log("tao kẹt ở ô : [" + currentRow + "][" + currentCol + "]");
                        Debug.Log("v" + visited[currentRow, currentCol]);
                        Debug.Log("p" + m_imagePos[currentRow, currentCol]);
                    }
                }

                // trả về false tức đã chạy hết tất cả lần nhưng không tìm được đường đi
            }

            Debug.Log("false 2");
            return false;
        }
    }
}