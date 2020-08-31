using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public partial class Form1 : Form
    {
        Dictionary<int, Size> pairs = null;

        System.Threading.Timer timer=null;

        DateTime time = DateTime.Parse("00:00:00");

        delegate void SetTime();

        /// <summary>
        /// 存储数据
        /// </summary>
        private int[,] arr = new int[4, 4];

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            pairs = new Dictionary<int, Size>();
            #region 初始化不同大小的值不同的属性
            // 初始化不同大小的值不同的属性
            Size two = new Size() {
                backColor = Color.FromArgb(239, 229, 219),
                fontSize=20,
            };
            pairs.Add(2, two);

            Size four = new Size() {
                backColor=Color.FromArgb(237, 225, 201),
                fontSize=20,
            };
            pairs.Add(4, four);

            Size eight = new Size() {
                backColor = Color.FromArgb(241, 178, 124),
                fontSize = 20,
            };
            pairs.Add(8,eight);

            Size sixteen = new Size() {
                backColor = Color.FromArgb(236, 141, 83),
                fontSize = 20,
            };
            pairs.Add(16,sixteen);

            Size thirtyTwo = new Size() {
                backColor = Color.FromArgb(245, 124, 97),
                fontSize = 20,
            };
            pairs.Add(32,thirtyTwo);

            Size sixtyFour = new Size() {
                backColor = Color.FromArgb(245, 95, 62),
                fontSize = 20,
            };
            pairs.Add(64, sixtyFour);

            Size oneHundredAndTwentyEight = new Size() {
                backColor = Color.FromArgb(235, 207, 113),
                fontSize = 15,
            };
            pairs.Add(128,oneHundredAndTwentyEight);

            Size twoHundredAndFiftySix = new Size() {
                backColor = Color.FromArgb(235, 202, 95),
                fontSize = 15,
            };
            pairs.Add(256, twoHundredAndFiftySix);

            Size fiveHundredAndTwelve = new Size() {
                backColor = Color.FromArgb(236, 200, 80),
                fontSize = 15,
            };
            pairs.Add(512, fiveHundredAndTwelve);

            Size oneThousandAndTwentyFour = new Size() {
                backColor = Color.FromArgb(234, 196, 59),
                fontSize = 10,
            };
            pairs.Add(1024, oneThousandAndTwentyFour);

            Size twoThousandAndFortyEight = new Size()
            {
                backColor = Color.FromArgb(30, 205, 239),
                fontSize = 10,
            };
            pairs.Add(2048, oneThousandAndTwentyFour);
            #endregion

        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            arr = new int[4,4];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = 0;
                }
            }
            // 增加两个
            Add();
            Add();

            time = DateTime.Parse("00:00:00");
            label1.Text = time.ToLongTimeString().ToString();
            // 开始计时
            if (timer == null)
            {
                timer = new System.Threading.Timer(AddSecond, null, 0, 1000);
            }
            else {
                timer.Change(1000,1000);
            }
            
        }

        Random random = new Random();

        private void Add() {
            int x = 0;
            int y = 0;
            do
            {
                x = random.Next(0, 4);
                y = random.Next(0, 4);
                if (arr[x,y]==0)
                {
                    arr[x, y] = 2;
                    break;
                }
            } while (true);
            Node node = new Node()
            {
                x=x,
                y=y,
                value=arr[x,y],
            };
            Refresh(node);
        }

        private void AddSecond(object state)
        {
            time = time.AddSeconds(1);
            this.Invoke(new SetTime(() =>
            {
                label1.Text = time.ToLongTimeString().ToString();
            }));
        }

        /// <summary>
        /// 渲染按钮
        /// </summary>
        /// <param name="node"></param>
        private void Refresh(Node node) {
            Size size = new Size()
            {
                backColor = System.Drawing.SystemColors.AppWorkspace,
                fontSize = 20,
            };
            int index = node.x*4 + node.y;
            string text = node.value.ToString();
            if (node.value == 0)
            {
                text = "";
            }
            Button btn = new Button();
            foreach (Control item in panel1.Controls)
            {
                if (Convert.ToInt32(item.Tag) == index)
                {
                    btn = item as Button;
                }
            }
            btn.Text = text;

            if (pairs.ContainsKey(node.value))
            {
                size = pairs[node.value];
            }
            btn.Font = new System.Drawing.Font("Microsoft YaHei UI", size.fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btn.BackColor = size.backColor;
            if (node.value == 2048)
            {
                timer.Change(Timeout.Infinite,100);
                string timeSpent = time.ToLongTimeString().ToString();
                if (MessageBox.Show($"挑战成功!,用时:{timeSpent}", "提示", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Init();
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button22_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                panel1.Controls[i].Font = new System.Drawing.Font("Microsoft YaHei UI", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                panel1.Controls[i].BackColor = System.Drawing.SystemColors.AppWorkspace;
                panel1.Controls[i].Text = "";
            }
            Init();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.button22.Focus();
        }

        /// <summary>
        /// 获取键盘按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBoardKeyDown(object sender, KeyEventArgs e) {
            Keys keyData = e.KeyCode;
            switch (keyData)
            {
                case Keys.Up:
                    Up();
                    break;
                case Keys.Down:
                    Down();
                    break;
                case Keys.Left:
                    Left();
                    break;
                case Keys.Right:
                    Right();
                    break;
            }
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                bool isOver = true;
                foreach (Control item in panel1.Controls)
                {
                    if (string.IsNullOrEmpty(item.Text))
                    {
                        isOver = false;
                        break;
                    }
                }
                if (isOver)
                {
                    //DialogResult result = MessageBox.Show($"游戏结束!,是否重新开始?", "提示", MessageBoxButtons.OKCancel);
                    //if (result == DialogResult.OK)
                    //{
                    //    Init();
                    //}
                    //else if (result == DialogResult.Cancel)
                    //{
                    //    this.Close();
                    //}
                }
                else
                {
                    // 按完方向键再增加一个
                    Add();
                }
            }
        }

        private void Up() {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                int temp = 0;
                for (int j = arr.GetLength(0)-1; j >= 0; j--)
                {
                    if (j == 0)
                    {
                        if (temp != 0)
                        {
                            arr[j,i ] = temp;
                            Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                        }

                    }
                    else {
                        if (arr[j, i] != 0)
                        {
                            if (temp != 0)
                            {
                                if (temp == arr[j - 1, i])
                                {
                                    arr[j - 1, i] = arr[j - 1, i] * 2;
                                    Refresh(new Node() { x = j - 1, y = i, value = arr[j - 1, i] });
                                    temp = 0;
                                    j = arr.GetLength(0);
                                }
                                else
                                {
                                    int v = temp;
                                    temp = arr[j - 1, i];
                                    arr[j - 1, i] = v;
                                    Refresh(new Node() { x = j - 1, y = i, value = arr[j - 1, i] });
                                }
                            }
                            else {
                                if (arr[j, i] == arr[j - 1, i])
                                {
                                    arr[j - 1, i] = arr[j - 1, i] * 2;
                                    Refresh(new Node() { x = j - 1, y = i, value = arr[j - 1, i] });
                                    arr[j, i] = 0;
                                    Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                                    j = arr.GetLength(0);
                                }
                                else
                                {
                                    if (arr[0, i] == 0 || (j > 1 && (arr[1, i] == 0)) || (j > 2 && (arr[2, i] == 0)))
                                    {
                                        temp = arr[j - 1, i];
                                        arr[j - 1, i] = arr[j, i];
                                        Refresh(new Node() { x = j - 1, y = i, value = arr[j - 1, i] });
                                        arr[j, i] = 0;
                                        Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                                        j = arr.GetLength(0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Down() {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                int temp = 0;
                for (int j = 0; j <arr.GetLength(0); j++)
                {
                    if (j == arr.GetLength(1)-1)
                    {
                        if (temp != 0)
                        {
                            arr[j, i] = temp;
                            Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                        }

                    }
                    else
                    {
                        if (arr[j, i] != 0)
                        {
                            if (temp != 0)
                            {
                                if (temp == arr[j + 1, i])
                                {
                                    arr[j + 1, i] = arr[j + 1, i] * 2;
                                    Refresh(new Node() { x = j + 1, y = i, value = arr[j + 1, i] });
                                    arr[j, i] = temp;
                                    j = -1;
                                }
                                else
                                {
                                    int v = temp;
                                    temp = arr[j + 1, i];
                                    arr[j + 1, i] = v;
                                    Refresh(new Node() { x = j + 1, y = i, value = arr[j + 1, i] });
                                }
                            }
                            else
                            {
                                if (arr[j, i] == arr[j + 1, i])
                                {
                                    arr[j + 1, i] = arr[j + 1, i] * 2;
                                    Refresh(new Node() { x = j + 1, y = i, value = arr[j + 1, i] });
                                    arr[j, i] = 0;
                                    Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                                    j = -1;
                                }
                                else
                                {
                                    if (arr[arr.GetLength(1) - 1, i] == 0 || (j < 2 && (arr[arr.GetLength(1) - 2, i] == 0)) || (j < 1 && (arr[arr.GetLength(1) - 3, i] == 0)))
                                    {
                                        temp = arr[j + 1, i];
                                        arr[j + 1, i] = arr[j, i];
                                        Refresh(new Node() { x = j + 1, y = i, value = arr[j + 1, i] });
                                        arr[j, i] = 0;
                                        Refresh(new Node() { x = j, y = i, value = arr[j, i] });
                                        j = -1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private new void Left() {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                int temp = 0;
                for (int j = arr.GetLength(1) - 1; j >= 0; j--)
                {
                    if (j == 0)
                    {
                        if (temp != 0)
                        {
                            arr[i, j] = temp;
                            Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                        }
                    }
                    else
                    {
                        if (arr[i, j] != 0)
                        {
                            // 之前交换过位置
                            if (temp != 0)
                            {
                                if (temp == arr[i, j - 1])
                                {
                                    arr[i, j - 1] = arr[i, j - 1] * 2;
                                    Refresh(new Node() { x = i, y = j - 1, value = arr[i, j - 1] });
                                    temp = 0;
                                    j = arr.GetLength(1);
                                }
                                else
                                {
                                    int v = temp;
                                    temp = arr[i, j - 1];
                                    arr[i, j - 1] = v;
                                    Refresh(new Node() { x = i, y = j - 1, value = arr[i, j - 1] });
                                }
                            }
                            else
                            {
                                if (arr[i, j] == arr[i, j - 1])
                                {
                                    arr[i, j - 1] = arr[i, j - 1] * 2;
                                    Refresh(new Node() { x = i, y = j - 1, value = arr[i, j - 1] });
                                    arr[i, j] = 0;
                                    Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                                    j = arr.GetLength(1);
                                }
                                else
                                {
                                    if (arr[i, 0] == 0 || (j > 1 && (arr[i, 1] == 0)) || (j > 2 && (arr[i, 2] == 0)))
                                    {
                                        temp = arr[i, j - 1];
                                        arr[i, j - 1] = arr[i, j];
                                        Refresh(new Node() { x = i, y = j - 1, value = arr[i, j - 1] });
                                        arr[i, j] = 0;
                                        Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                                        j = arr.GetLength(1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private new void Right() {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                int temp = 0;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j == arr.GetLength(1) - 1)
                    {
                        if (temp != 0)
                        {
                            arr[i, j] = temp;
                            Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                        }
                    }
                    else
                    {
                        if (arr[i, j] != 0)
                        {
                            // 之前交换过位置
                            if (temp!=0)
                            {
                                if (temp == arr[i, j + 1])
                                {
                                    arr[i, j + 1] = arr[i, j + 1] * 2;
                                    Refresh(new Node() { x = i, y = j + 1, value = arr[i, j + 1] });
                                    temp = 0;
                                    j = -1;
                                }
                                else
                                {
                                    int v = temp;
                                    temp = arr[i, j + 1];
                                    arr[i, j + 1] = v;
                                    Refresh(new Node() { x = i, y = j + 1, value = arr[i, j + 1] });
                                }
                            }
                            else
                            {
                                if (arr[i, j] == arr[i, j + 1])
                                {
                                    arr[i, j + 1] = arr[i, j + 1] * 2;
                                    Refresh(new Node() { x = i, y = j + 1, value = arr[i, j + 1] });
                                    arr[i, j] = 0;
                                    Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                                    j = -1;
                                }
                                else
                                {
                                    if (arr[i, arr.GetLength(1) - 1] == 0 || (j < 2 && (arr[i, arr.GetLength(1) - 2] == 0)) || (j < 1 && (arr[i, arr.GetLength(1) - 3] == 0)))
                                    {
                                        temp = arr[i, j + 1];
                                        arr[i, j + 1] = arr[i, j];
                                        Refresh(new Node() { x = i, y = j + 1, value = arr[i, j + 1] });
                                        arr[i, j] = 0;
                                        Refresh(new Node() { x = i, y = j, value = arr[i, j] });
                                        j = -1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData==Keys.Up||keyData==Keys.Down||keyData==Keys.Left||keyData==Keys.Right)
            {
                return false;
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
