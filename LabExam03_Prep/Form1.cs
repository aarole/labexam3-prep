using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LabExam03_Prep
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //DON'T FORGET INTRO COMMENTS
        private const int cSize = 100;
        private string[] mName = new string[cSize];
        private double[] mSalary = new double[cSize]; //If any calculations, declare appropriately (i.e. int or double)
        private bool[] mTenure = new bool[cSize];
        private int mIndex = 0;
        private string mFileName = Path.Combine(Application.StartupPath, "Data.txt");

        private void DisplayMessage(string msg)
        {
            MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        private bool ValidateInput()
        {
            if(txtName.Text == "")
            {
                DisplayMessage("Please enter your last name.");
                txtName.Focus();
                return false;
            }
            if(txtSalary.Text == "")
            {
                DisplayMessage("Please enter your salary.");
                txtSalary.Focus();
                return false;
            }
            double salary;
            if(double.TryParse(txtSalary.Text, out salary) == false)
            {
                DisplayMessage("Please enter a real number in salary.");
                txtSalary.Focus();
                return false;
            }
            return true;

        }
        private void ClearInput()
        {
            txtName.Clear();
            txtSalary.Clear();
            chkTenure.Checked = false;

            return;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if(ValidateInput() == false)
            {
                return;
            }
            string name = txtName.Text;
            string salary = txtSalary.Text;

            mName[mIndex] = name;
            mSalary[mIndex] = double.Parse(salary);
            mTenure[mIndex] = chkTenure.Checked;
            name = txtName.Text; //set the values for string variables
            int length = name.Length; //Length of name
            string firstLetter = name.Substring(0, 1);        // first letter
            string rest = name.Substring(1, length - 1);  // rest of the letters in name
            string properCase = firstLetter.ToUpper() + rest.ToLower();
            //-	Store the last name in proper case at proper index.
            mName[mIndex] = properCase;

            mIndex++;

            if(mIndex == cSize)
            {
                DisplayMessage("The arrays are full.");
                btnEnter.Enabled = false;
                return;
            }
            ClearInput();
            txtName.Focus();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            if (mIndex == 0)
            {
                DisplayMessage("The arrays are empty.");
                return;
            }
            lstOut.Items.Add("Name".PadRight(15) + "Salary".PadRight(15) + "Tenure");
            lstOut.Items.Add("=================================================================================");
            for(int ctr = 0; ctr<mIndex; ctr++)
            {
                lstOut.Items.Add(mName[ctr].PadRight(15) + mSalary[ctr].ToString("c").PadRight(15) + mTenure[ctr]);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
            lstOut.Items.Clear();
            radAsc.Checked = false;
            radDesc.Checked = false;
            txtSearch.Clear();
            txtName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstOut.SelectedIndex < 2)
            {
                return;
            }
            int index = lstOut.SelectedIndex - 2;
            txtName.Text = mName[index];
            txtSalary.Text = mSalary[index].ToString("n");
            chkTenure.Checked = mTenure[index];
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            if (radAsc.Checked == false && radDesc.Checked == false)
            {
                DisplayMessage("Please select a sort option.");
                return;
            }

            string[] tempName = new string[cSize];
            Array.Copy(mName, tempName, mIndex);

            Array.Sort(mName, mSalary, 0, mIndex);
            Array.Sort(tempName, mTenure, 0, mIndex);

            if(radDesc.Checked == true)
            {
                Array.Reverse(mName, 0, mIndex);
                Array.Reverse(mSalary, 0, mIndex);
                Array.Reverse(mTenure, 0, mIndex);
            }

            lstOut.Items.Add("Name".PadRight(15) + "Salary".PadRight(15) + "Tenure");
            lstOut.Items.Add("=================================================================================");
            for (int ctr = 0; ctr < mIndex; ctr++)
            {
                lstOut.Items.Add(mName[ctr].PadRight(15) + mSalary[ctr].ToString("c").PadRight(15) + mTenure[ctr]);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            if (txtSearch.Text == "")
            {
                DisplayMessage("Please enter a search term.");
                return;
            }

            string search = txtSearch.Text.ToLower();
            bool flag = false;
            int ctr;

            for(ctr = 0; ctr<mIndex; ctr++)
            {
                if(search == mName[ctr].ToLower())
                {
                    flag = true;
                    break;
                }
            }

            if(flag == true)
            {
                lstOut.Items.Add("Discovered name: " + mName[ctr]);
                lstOut.Items.Add("Name".PadRight(15) + "Salary".PadRight(15) + "Tenure");
                lstOut.Items.Add("=================================================================================");
                lstOut.Items.Add(mName[ctr].PadRight(15) + mSalary[ctr].ToString("c").PadRight(15) + mTenure[ctr]);
            }
            else
            {
                DisplayMessage("Search term not found.");
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter SW = null;

            if(MessageBox.Show("Do you want to quit the application?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                txtName.Focus();
                return;
            }
            else
            {
                try
                {
                    SW = new StreamWriter(mFileName);
                    for (int ctr = 0; ctr < mIndex; ctr++)
                    {
                        SW.WriteLine(mName[ctr] + '\t' + mSalary[ctr] + '\t' + mTenure[ctr]);
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message);
                }
                finally
                {
                    if (SW != null)
                    {
                        SW.Close();
                    }
                }
            }
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            double max = mSalary[0];
            string name = "";
            for(int ctr = 1; ctr<mIndex; ctr++)
            {
                if(max<mSalary[ctr])
                {
                    max = mSalary[ctr];
                    name = mName[ctr];
                }
            }
            lstOut.Items.Add("Maximum salary: " + max.ToString("c"));
            lstOut.Items.Add("Person with max salary: " + name);
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            double min = mSalary[0];
            string name = "";
            for (int ctr = 1; ctr < mIndex; ctr++)
            {
                if (min > mSalary[ctr])
                {
                    min = mSalary[ctr];
                    name = mName[ctr];
                }
            }
            lstOut.Items.Add("Minimum salary: " + min.ToString("c"));
            lstOut.Items.Add("Person with max salary: " + name);
        }

        private void btnAvg_Click(object sender, EventArgs e)
        {
            lstOut.Items.Clear();
            double avg;
            double sum = 0;
            for(int ctr = 0; ctr<mIndex; ctr++)
            {
                sum += mSalary[ctr];
            }
            avg = sum / mIndex;
            lstOut.Items.Add("Average salary: " + avg.ToString("c"));
        }
    }
}
