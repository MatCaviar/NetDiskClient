using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                var filepath = ofd.FileName;

                Ndutil.upload(filepath);
                MessageBox.Show("nihao,java");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
             Ndutil.login();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            showList();
        }   


        private void Form1_Load(object sender, EventArgs e)
        {

            Ndutil.login();
          
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             int CIndex = e.ColumnIndex;
            //按钮所在列为第五列，列下标从0开始的  
            if (CIndex == 8)
            {
                //获取在同一行第一列的单元格中的字段值  
                //int _UID = Convert.ToInt32(dataGridView1[0, e.RowIndex].Value);
                string fileid = dataGridView1[0, e.RowIndex].Value.ToString();
                string recordid = dataGridView1[7, e.RowIndex].Value.ToString();
                
                var filename = dataGridView1[2, e.RowIndex].Value.ToString();
                var url = Ndutil.get_dowlload(fileid, recordid);

                string localFilePath = String.Empty;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

               Console.WriteLine(filename);
                saveFileDialog1.FileName =filename;
                                //设置默认文件类型显示顺序  
                saveFileDialog1.FilterIndex = 2;
                                //保存对话框是否记忆上次打开的目录  
                saveFileDialog1.RestoreDirectory = true;

                //点了保存按钮进入  
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //获得文件路径  
                    localFilePath = saveFileDialog1.FileName.ToString();
                    Console.WriteLine(localFilePath);
                    //string filname = this.openFileDialog2.FileName;
                    //获取文件名，不带路径  
                    //fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);  

                    //获取文件路径，不带文件名  
                    //FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));  

                    //给文件名前加上时间  
                    //newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt;  
                    WebClient client = new WebClient();
                    string URLAddress = url;
                    string receivePath = localFilePath;
                    client.DownloadFile(URLAddress, receivePath);
                }

      
        }
        }

        public void showList()
        {
            var li = Ndutil.get_list();
            Console.WriteLine(li);

            dataGridView1.DataSource = li;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font= new Font("Tahoma", 11); 

            if (dataGridView1.ColumnCount == 8) {
            dataGridView1.AutoGenerateColumns = false;
            var col_link_down = new DataGridViewLinkColumn();
            col_link_down.HeaderText = "操作";

            col_link_down.UseColumnTextForLinkValue = false;
            col_link_down.Text = "下载";
            col_link_down.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.Columns["type"].Visible = false;

            dataGridView1.Columns.Add(col_link_down);

            };
            int row = dataGridView1.Rows.Count;//得到总行数     
            for (int i = 0; i < row; i++)//得到总行数并在之内循环    
            {

                if ("1" == dataGridView1[5, i].Value.ToString())
                {   //对比TexBox中的值是否与dataGridView中的值相同（上面这句）    
                    dataGridView1[8, i].Value = "";

                }
                else
                {
                    dataGridView1[8, i].Value = "下载";
                }
            }
        }


    }
}
