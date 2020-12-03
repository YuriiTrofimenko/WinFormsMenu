using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WinFormsMenu
{
    public partial class Form1 : Form
    {
        private BindingList<GroupModel> groupModels = new BindingList<GroupModel>();
        private BindingSource groupModelsSource;
        private XmlDocument xmlDoc = new XmlDocument();
        private XmlElement document;
        private int selectedGroupId = -1;
        public Form1()
        {
            InitializeComponent();
            groupModelsSource = new BindingSource(groupModels, null);
        }

        private void FillGroupsGrid() {
            groupsDataGridView.Rows.Clear();
            xmlDoc.Load("groups.xml");
            try
            {
                /* groupModels.Add(new GroupModel() { Id = 0, Name = "Zero" });
                groupsDataGridView.DataSource = null;
                groupsDataGridView.DataSource = groupModels; */
                document = xmlDoc.DocumentElement;

                foreach (XmlElement groupElement in document.ChildNodes)
                {
                    groupModels.Add(
                        new GroupModel()
                        {
                            Id = Int32.Parse(groupElement.GetAttribute("id")),
                            Name = groupElement.GetElementsByTagName("name")[0].InnerXml
                        });
                }
                groupsDataGridView.DataSource = null;
                groupsDataGridView.DataSource = groupModelsSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillGroupsGrid();
        }

        private void groupsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            /* foreach (var cell in groupsDataGridView.SelectedRows?[0].Cells)
            {
                Console.WriteLine(cell);
            } */
            // Console.WriteLine(groupsDataGridView.Rows);
            foreach (DataGridViewRow row in groupsDataGridView.Rows)
            {
                // Console.WriteLine(row.Selected);
                if (row.Selected)
                {
                    // Console.WriteLine(row.Cells[0].Value);
                    selectedGroupId = (int)row.Cells[0].Value;
                    /* foreach (DataGridViewCell cell in row.Cells)
                    {
                        Console.WriteLine(cell.Value);
                    } */
                    break;
                }
            }
            /* for (int i = 0; i < groupsDataGridView.SelectedRows?[0]?.Cells?.Count; i++)
            {
                Console.WriteLine(i);
                // Console.WriteLine(groupsDataGridView.SelectedRows?[0].Cells[i]);
            } */
        }

        private void SaveToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            
        }

        private void RemoveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (selectedGroupId != -1)
            {
                XmlElement removedXmlElement =
                document.ChildNodes.Cast<XmlElement>()
                     .Where(groupElement => Int32.Parse(groupElement.GetAttribute("id")) == selectedGroupId)
                     .SingleOrDefault();
                document.RemoveChild(removedXmlElement);
                xmlDoc.Save("groups.xml");
                FillGroupsGrid();
            }
            else {
                MessageBox.Show("Select a group before");
            }
        }
    }
}
