using Empolyees.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YourProjectNamespace;

namespace Empolyees
{
    public partial class Form1 : Form
    {
        private List<Company> companies;

        public Form1()
        {
            InitializeComponent();
            comboCompanies.SelectedIndexChanged += comboCompanies_SelectedIndexChanged;
            listEmployees.SelectedIndexChanged += listEmployees_SelectedIndexChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCompanies();
        }

        private void LoadCompanies()
        {
            companies = DatabaseHelper.GetAllCompanies();
            comboCompanies.DataSource = companies;
            comboCompanies.DisplayMember = "Name";
            comboCompanies.ValueMember = "Id";
        }

        private void comboCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCompanies.SelectedItem is Company selectedCompany)
            {
                // Show company info
                txtDescription.Text = selectedCompany.Description;
                txtPhone.Text = selectedCompany.PhoneNumber;
                txtEmail.Text = selectedCompany.Email;

                string fullPath = Path.Combine(Application.StartupPath, "images", selectedCompany.LogoPath);

                if (File.Exists(fullPath))
                {
                    pictureLogo.Image = Image.FromFile(fullPath);
                }
                else
                {
                    pictureLogo.Image = null; // or a placeholder
                }

                // Load employees
                var employees = DatabaseHelper.GetEmployeesByCompanyId(selectedCompany.Id);
                listEmployees.DataSource = employees;
                listEmployees.DisplayMember = "FullName";
            }
        }

        private void listEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listEmployees.SelectedItem is Employee emp)
            {
                txtFirstName.Text = emp.FirstName;
                txtLastName.Text = emp.LastName;
                txtAge.Text = emp.Age.ToString();
                txtPosition.Text = emp.Position;
                txtSalary.Text = emp.Salary.ToString("F2");
                txtEmpEmail.Text = emp.Email;
                txtEmpPhone.Text = emp.PhoneNumber;

                string fullPath = Path.Combine(Application.StartupPath, "images", emp.PhotoPath);

                if (File.Exists(fullPath))
                {
                    picturePhoto.Image = Image.FromFile(fullPath);
                }
                else
                {
                    picturePhoto.Image = null; // or a placeholder
                }
            }
        }
    }
}
