using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTF_products
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
       private void openfile_Click(object sender, EventArgs e)
        {
       
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // create a new datatable
                DataTable dt = new DataTable();
                // create a new data row
                DataRow dr;
                // create a new data column
                DataColumn dc;
                // create a new string array
                string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                // create a new string array
                string[] dataCol = null;
                // create a new int variable
                int x = 0;

                // loop through the lines array
                foreach (string line in lines)
                {
                    // split the line at the commas
                    dataCol = line.Split(',');
                    // if x is 0
                    if (x == 0)
                    {
                        // loop through the dataCol array
                        for (int i = 0; i <= dataCol.Count() - 1; i++)
                        {
                            // create a new data column
                            dc = new DataColumn(dataCol[i].ToString());
                            // add the data column to the datatable
                            dt.Columns.Add(dc);
                        }
                        // increment x
                        x++;
                    }
                    else
                    {
                        // create a new data row
                        dr = dt.NewRow();
                        // loop through the dataCol array
                        for (int i = 0; i <= dataCol.Count() - 1; i++)
                        {
                            // add the dataCol to the data row
                            dr[i] = dataCol[i].ToString();
                        }
                        // add the data row to the datatable
                        dt.Rows.Add(dr);
                    }
                }
                // set the data source of the dataGridView1 to the datatable
                dataGridView1.DataSource = dt;
            }




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
