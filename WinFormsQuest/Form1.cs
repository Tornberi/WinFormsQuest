using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinFormsQuest
{
    public partial class Form1 : Form
    {
        private NewRowsList newRowsList;
        private DataService dataService;

        DataTable grid;
        decimal znPrice;
        int znTypeProduct;
        string znCount;
        string znSeller;
        string znProduct;
        string znPriceFilter = "";
        string znProductFilter = "";
        string znSellerFilter = "";
        string znDateFilter = "";
        DateTime znDelivery = DateTime.Now;
        ReportDataSource rep = new ReportDataSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Grid();
            GridInfo();
            Combo();
        }

        private void Grid()
        {
            this.dataService = new DataService();
            this.newRowsList = NewRowsList.GetRows(this.dataService.rowList(znPriceFilter, znProductFilter, znSellerFilter, znDateFilter));
            this.newRowsBindingSource.DataSource = this.newRowsList;

        }

        private void GridInfo()
        {
            grd.Columns[0].HeaderText = "Название продукта";
            grd.Columns[1].HeaderText = "Тип продукта";
            grd.Columns[2].HeaderText = "Название организации";
            grd.Columns[3].HeaderText = "Цена";
            grd.Columns[4].HeaderText = "Кол-во продукта";
            grd.Columns[5].HeaderText = "Дата поставки";

            grd.Columns[0].Width = 130;
            grd.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }
        
        private void Combo()
        {
            DataTable SellName = SqlHelp.load_table(@"select distinct NameSeller from [dbo].[Sellers] order by 1");

            cBSellName.Items.Clear();
            cBSellName.Items.Add(" ");
            for (int i = 0; i < SellName.Rows.Count; i++)
            {
                cBSellName.Items.Add(SellName.Rows[i]["NameSeller"]);
            }        
            

            cBProduct.Items.Add(" ");
            cBPrice.Items.Add(" ");

            DataTable combo1 = SqlHelp.load_table(@"select distinct SellersName from dbo.Supply");
            DataTable combo2 = SqlHelp.load_table(@"select distinct ProductName from dbo.Supply sup
                                                    left join [dbo].[Articles] art ON art.id = sup.idProducts");
            DataTable combo3 = SqlHelp.load_table(@"select distinct Price from dbo.Supply");
            DataTable combo4 = SqlHelp.load_table(@"select distinct Convert(char(16), Delivery_date, 120) as Delivery_date from dbo.Supply");

            comboBox1.Items.Clear();
            comboBox1.Items.Add(" ");
            for (int i = 0; i < combo1.Rows.Count; i++)
            {
                comboBox1.Items.Add(combo1.Rows[i]["SellersName"]);
            }

            comboBox2.Items.Clear();
            comboBox2.Items.Add(" ");
            for (int i = 0; i < combo2.Rows.Count; i++)
            {
                comboBox2.Items.Add(combo2.Rows[i]["ProductName"]);
            }

            comboBox3.Items.Clear();
            comboBox3.Items.Add(" ");
            for (int i = 0; i < combo3.Rows.Count; i++)
            {
                comboBox3.Items.Add(combo3.Rows[i]["Price"]);
            }

            comboBox4.Items.Clear();
            comboBox4.Items.Add(" ");
            for (int i = 0; i < combo4.Rows.Count; i++)
            {
                comboBox4.Items.Add(combo4.Rows[i]["Delivery_date"]);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) && !(e.KeyChar == 44))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                znCount = textBox1.Text;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            znCount = textBox1.Text;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Cleaner();
        }

        private void Cleaner()
        {
            cBSellName.Text = " "; znSeller = "";
            cBProduct.Items.Clear(); znProduct = "";
            cBPrice.Items.Clear(); znPrice = 0;
            textBox1.Text = ""; znCount = "";
            dTPDelivery.Value = DateTime.Now; znDelivery = DateTime.Now;
        }
        private void dTPDelivery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                znDelivery = dTPDelivery.Value;
            }
        }

        private void dTPDelivery_CloseUp(object sender, EventArgs e)
        {
            znDelivery = dTPDelivery.Value;
        }

        private void cBSellName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBSellName.Text != " ")
            {
                DataTable NameProduct = SqlHelp.load_table(string.Format(@"select ProductName from [dbo].[Articles] art
                left join [dbo].[Sellers] sell ON art.id = sell.idProduct
                where sell.NameSeller = '{0}'", cBSellName.Text));

                cBProduct.Items.Clear();
                cBProduct.Items.Add(" ");
                for (int i = 0; i < NameProduct.Rows.Count; i++)
                {
                    cBProduct.Items.Add(NameProduct.Rows[i]["ProductName"]);
                }
            }
        }

        private void cBProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBSellName.Text != " " && cBProduct.Text != " ")
            {
                DataTable Price = SqlHelp.load_table(string.Format(@"select Price from [dbo].[Sellers] sell
                left join dbo.Articles art ON art.id = sell.idProduct
                where sell.NameSeller = '{0}' and art.ProductName = '{1}'", cBSellName.Text, cBProduct.Text));

                znTypeProduct = Convert.ToInt32(SqlHelp.string_table(string.Format(@"select id from [dbo].[Articles] where ProductName = '{0}'", cBProduct.Text)));

                cBPrice.Items.Clear();
                cBPrice.Items.Add(" ");
                for (int i = 0; i < Price.Rows.Count; i++)
                {
                    cBPrice.Items.Add(Price.Rows[i]["Price"]);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if ((cBSellName.Text != " " || cBSellName.Text != "") && ( cBProduct.Text != " " || cBProduct.Text != "") && (cBPrice.Text != " " || cBPrice.Text != "") && (textBox1.Text != " " || textBox1.Text != ""))
            {
                SqlHelp.string_table(string.Format(@"insert into dbo.Supply Values ({0}, '{1}', {2}, {3}, '{4}', default)", znTypeProduct, cBSellName.Text, znCount, znPrice.ToString().Replace(',','.'), znDelivery));
                Grid();
                Cleaner();
            }
            else
            {
                MessageBox.Show("Ошибка! Нужно заполнить все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cBPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            znPrice = Convert.ToDecimal(cBPrice.Text);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            List<NewRows> newRows = new List<NewRows>();
            newRows.Clear();

            for (int i = 0; i < grd.Rows.Count; i++)
            {

                NewRows rows = new NewRows
                {
                    ProductName = grd.Rows[i].Cells[0].Value.ToString(),
                    TypeProduct = grd.Rows[i].Cells[1].Value.ToString(),
                    NameSeller = grd.Rows[i].Cells[2].Value.ToString(),
                    Price = decimal.Parse(grd.Rows[i].Cells[3].Value.ToString()),
                    CountProduct = int.Parse(grd.Rows[i].Cells[4].Value.ToString()),
                    Delivery_date = grd.Rows[i].Cells[5].Value.ToString()
                };
                newRows.Add(rows);
            }

                rep.Name = "DataSet1";
                rep.Value = newRows;
                PrintForms form = new PrintForms();
                form.reportViewer1.LocalReport.DataSources.Clear();
                form.reportViewer1.LocalReport.DataSources.Add(rep);
                form.reportViewer1.LocalReport.ReportEmbeddedResource = "WinFormsQuest.Report1.rdlc";

                form.ShowDialog();            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != " ")
            {
                znSellerFilter = string.Format("and NameSeller = '{0}'", comboBox1.Text);
            }
            else
            {
                znSellerFilter = "";
            }
            Grid();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != " ")
            {
                znProductFilter = string.Format("and ProductName = '{0}'", comboBox2.Text);
            }
            else
            {
                znProductFilter = "";
            }
            Grid();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != " ")
            {
                znPriceFilter = string.Format("and sup.Price = {0}", comboBox3.Text.Replace(',','.'));
            }
            else
            {
                znPriceFilter = "";
            }
            Grid();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (comboBox4.Text != " ")
                {
                    znDateFilter = string.Format("and Convert(char(16),sup.Delivery_date,120) = '{0}'", comboBox4.Text);
                }
                else
                {
                    znDateFilter = "";
                }
            Grid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Text = " ";
            comboBox2.Text = " ";
            comboBox3.Text = " ";
            comboBox4.Text = " ";
        }
    }
}
