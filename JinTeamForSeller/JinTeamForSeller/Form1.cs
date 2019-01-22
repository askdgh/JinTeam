using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace JinTeamForSeller
{
    public partial class Form1 : Form
    {
        List<WebPage> lstWeb = new List<WebPage>();
        List<Product> lstPro = new List<Product>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(DateTime.Now.ToString().Remove(DateTime.Now.ToString().IndexOf('오'),2).Replace(" ","").Replace("-","").Replace(":",""));
            int cat = 57;
            string url = "http://www.comeintofashion.net/product/list.html?cate_no=" + cat + "&page=1";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            //textBox1.Text = doc.Text;

            var doc2 = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div");
            var doc3 = doc2[1].SelectNodes("//h6/a");

            foreach (var item in doc3)
            {
                if (item.Attributes[0].Value.Contains("product"))
                {
                    lstWeb.Add(new WebPage("http://www.comeintofashion.net" + item.Attributes[0].Value, cat));
                    //textBox1.Text += "http://www.comeintofashion.net" + item.Attributes[0].Value + "\r\n";
                }
            }
            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            //    SqlCommand cmd = new SqlCommand();
            //    foreach (var item in lstWeb)
            //    {
            //        conn.Open();
            //        cmd.Connection = conn;
            //        cmd.CommandType = CommandType.Text;
            //        cmd.CommandText = "insert into webPages values('" + item.WebP + "'," + item.Category + ");";

            //        cmd.ExecuteNonQuery();
            //        conn.Close();
            //    }
            //}
            foreach (var item in lstWeb)
            {
                string u = item.WebP;
                int proId = int.Parse(u.Substring(u.IndexOf("=") + 1, 4));
                doc = web.Load(u);
                // textBox1.Text = doc.Text;

                // 대표 이미지
                string main_img = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/div/img").First().Attributes[0].Value;

                // 제품명
                string pro_Name = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/div/h2").First().InnerText;

                // 제품 가격
                int pro_Price = int.Parse(doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/div/div/span").First().InnerText);

                lstPro.Add(new Product("CominFa_" + proId, pro_Name, pro_Price, main_img));

                // 사이즈
                //var docdet = doc.DocumentNode.SelectNodes("//optgroup/option");
                //foreach (var item in docdet)
                //{
                //    textBox1.Text += item.InnerText + "\r\n";
                //}

                //제품 이미지
                //var docImg = doc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/div/p/span/div/img");
                //foreach (var item in docImg)
                //{
                //    if (!item.Attributes[1].Value.Contains("유의"))
                //    {
                //        textBox1.Text += "http://www.comeintofashion.net" + item.Attributes[1].Value + "\r\n";
                //    }
                //}


                //textBox1.Text += doc3.First().Attributes[0].Value;

                //foreach (var item in lstWeb)
                //{
                //    string detailUrl = item.WebP;
                //    doc = web.Load(detailUrl);


                //}
            }
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
                SqlCommand cmd = new SqlCommand();
                foreach (var item in lstPro)
                {
                    cmd.Parameters.Clear();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "insertProductsForTest";

                    cmd.Parameters.AddWithValue("pro_Id", item.Pro_ID);
                    cmd.Parameters.AddWithValue("pro_Name", item.Pro_Name);
                    cmd.Parameters.AddWithValue("pro_Price", item.Pro_Price);
                    cmd.Parameters.AddWithValue("main_Image", item.Main_Image);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}