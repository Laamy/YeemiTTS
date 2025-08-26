using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using Serilog;

namespace YeemiTTS.ControlForm.Windows;
public partial class StarChartScene : Form
{
    const string CHAR_STAR = "★"; // lol stolen character ez fuck u pringle

    public Color GreenToRedGradient(int index = 0)
    {
        const int MAX_INDEX = 10; // 10 stars for bright greeen
        // 0 stars for deep red

        var gradientDelta = 239 / ((MAX_INDEX+1)- index);

        var green = 255 * (gradientDelta / 239);
        return Color.FromArgb(
            0,
            255 * (gradientDelta / 239),
            255 - green,
            0
        );
    }

    public StarChartScene()
    {
        InitializeComponent();

        LoadChart();
    }

    public bool isValidStar(string str)
        => str.All(c => c == CHAR_STAR[0]) && str.Length >= 1 && str.Length <= 10;

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (isLoaded)
            SaveChart();

        // colour stars

    }

    private void SaveChart()
    {
        var serializer = new JavaScriptSerializer();
        var chartData = new List<Dictionary<string, object>>();

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue; // skip new row
            var rowData = new Dictionary<string, object>();
            foreach (DataGridViewCell cell in row.Cells)
            {
                rowData[dataGridView1.Columns[cell.ColumnIndex].Name] = cell.Value;
            }
            chartData.Add(rowData);
        }

        var json = serializer.Serialize(chartData);
        File.WriteAllText("starchart.json", json);
    }

    bool isLoaded = false;
    private void LoadChart()
    {
        if (!File.Exists("starchart.json"))
        {
            isLoaded = true;
            return;
        }

        var serializer = new JavaScriptSerializer();
        var json = File.ReadAllText("starchart.json");
        var chartData = serializer.Deserialize<List<Dictionary<string, object>>>(json);
        
        dataGridView1.Rows.Clear();
        foreach (var rowData in chartData)
        {
            var rowIndex = dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[rowIndex];
            foreach (var kvp in rowData)
            {
                if (dataGridView1.Columns.Contains(kvp.Key))
                {
                    row.Cells[kvp.Key].Value = kvp.Value;
                }
            }
        }
        isLoaded = true;
    }
}
