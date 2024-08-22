using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryMonitor
{
    public class ExportToExcel
    {
        public void ExportDataGridViewToExcel(DataGridView dataGridView, string filePath=null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Export to Excel";
            saveFileDialog.FileName = "Lithium_measure_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"; // Tên tệp Excel với ngày tháng hiện tại
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;

                // Creating Excel workbook and worksheet
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Adding DataGridView headers to Excel and applying background color
                for (int i = 1; i <= dataGridView.Columns.Count; i++)
                {
                    worksheet.Cell(1, i).Value = dataGridView.Columns[i - 1].HeaderText;
                    worksheet.Cell(1, i).Style.Fill.BackgroundColor = XLColor.LightGray; // Color for header row
                }

                // Adding DataGridView rows to Excel
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        if (dataGridView.Rows[i].Cells[j].Value != null )
                        {

                            if (double.TryParse(dataGridView.Rows[i].Cells[j].Value.ToString(), out double result) )
                            {
                                if(double.IsInfinity(result))
                                {
                                    result = 0;
                                }
                                worksheet.Cell(i + 2, j + 1).Value = result;
                            }
                            else
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dataGridView.Rows[i].Cells[j].Value;
                            }
                        }
                        else
                        {
                            // Handle case when value is null
                            worksheet.Cell(i + 2, j + 1).Value = "NULL";
                        }

                    }
                }

                // Saving Excel file
                try
                {
                    workbook.SaveAs(filePath);
                    MessageBox.Show("Exported successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    
}
