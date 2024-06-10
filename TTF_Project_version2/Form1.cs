using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace TTF_Project_version1
{
    /// <summary>
    /// Represents the main form of the TTF Project application.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reads data from a CSV file and returns it as a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="filePath">The path of the CSV file to read.</param>
        /// <returns>A <see cref="DataTable"/> containing the data from the CSV file, or null if an error occurred.</returns>
        public DataTable ReadCsvFile(string filePath)
        {
            DataTable dt = new DataTable();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("An error occurred while reading the file.", ex.Message);
                return null;
            }
            return dt;
        }

        /// <summary>
        /// Displays an error dialog with the specified message and details.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <param name="details">The detailed information about the error.</param>
        private void ShowErrorDialog(string message, string details)
        {
            DialogResult result = MessageBox.Show($"{message}\n\nDetails: {details}\n\nDo you want to submit again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (result == DialogResult.Yes)
            {
                btnImportCSV_Click(null, null); 
            }
            else
            {
                // User chose not to submit again
                // Add additional logic here if needed
            }
        }

        /// <summary>
        /// Handles the Click event of the Import CSV button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                DataTable dt = ReadCsvFile(filePath);

                if (dt != null)
                {
                    dataGridView1.DataSource = dt;
                }
            }
        }


        /// <summary>
        /// Handles the Click event of the Insert button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void insertBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    newRow[i] = ""; 
                }
                dt.Rows.Add(newRow);
                dataGridView1.Refresh();
            }
        }

        /// <summary>
        /// Handles the Click event of the Delete button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt != null && dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                {
                    dt.Rows.RemoveAt(selectedRow.Index);
                }
                dataGridView1.Refresh();
            }
        }

        /// <summary>
        /// Saves the data from a <see cref="DataTable"/> to a CSV file.
        /// </summary>
        /// <param name="filePath">The path of the CSV file to save.</param>
        /// <param name="dt">The <see cref="DataTable"/> containing the data to save.</param>
        private void SaveToCSV(string filePath, DataTable dt)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                string headerRow = string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                sw.WriteLine(headerRow);

                foreach (DataRow row in dt.Rows)
                {
                    string dataRow = string.Join(",", row.ItemArray.Select(item => item.ToString().Replace(",", "\\,")));
                    sw.WriteLine(dataRow);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Save button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    SaveToCSV(filePath, dt);
                }
            }
        }

        /// <summary>
        /// Performs a binary search on the <see cref="DataTable"/> based on the specified search value.
        /// </summary>
        /// <param name="searchValue">The value to search for.</param>
        private void BinarySearch(string searchValue)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt != null)
            {
                List<DataRow> rows = dt.AsEnumerable().ToList();

                rows = rows.OrderBy(r => r.Field<string>(0)).ToList();

                int left = 0;
                int right = rows.Count - 1;
                int mid;
                bool found = false;
                while (left <= right)
                {
                    mid = (left + right) / 2;
                    int comparison = string.Compare(rows[mid][0].ToString(), searchValue, StringComparison.OrdinalIgnoreCase);
                    if (comparison == 0)
                    {
                        found = true;
                        break;
                    }
                    else if (comparison < 0)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }

                DataView dv = new DataView(dt);
                if (found)
                {
                    dv.RowFilter = string.Format("[{0}] = '{1}'", dt.Columns[0].ColumnName, searchValue);
                }
                else
                {
                    dv.RowFilter = "1=0";
                }
                dataGridView1.DataSource = dv;
            }
        }

        /// <summary>
        /// Performs a sequential search on the <see cref="DataTable"/> based on the specified search value.
        /// </summary>
        /// <param name="searchValue">The value to search for.</param>
        private void SequentialSearch(string searchValue)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt != null)
            {
                List<DataRow> rows = dt.AsEnumerable().ToList();

                List<DataRow> matchingRows = new List<DataRow>();

                foreach (DataRow row in rows)
                {
                    string name = row[1].ToString();
                    if (name.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        name.Split(' ').Any(word => word.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        matchingRows.Add(row);
                    }
                }

                DataView dv = new DataView(dt);
                if (matchingRows.Count > 0)
                {
                    dv.RowFilter = string.Format("({0})", string.Join(" OR ", matchingRows.Select(r => string.Format("[{0}] = '{1}'", dt.Columns[1].ColumnName, r[1].ToString().Replace("'", "''")))));
                }
                else
                {
                    dv.RowFilter = "1=0";
                    dataGridView1.DataSource = null;
                }
                dataGridView1.DataSource = dv;
            }
        }


        /// <summary>
        /// Handles the TextChanged event of the Search textbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Reset the DataGridView when the search text is cleared
                DataTable dt = (DataTable)((DataView)dataGridView1.DataSource).Table;
                dataGridView1.DataSource = dt;
            }
        }

        /// <summary>
        /// Handles the Click event of the Binary Search button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void binaryBtn_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                BinarySearch(searchValue);
            }
        }

        /// <summary>
        /// Handles the Click event of the Sequential Search button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void sequentialBtn_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                SequentialSearch(searchValue);
            }
        }


    }
}
