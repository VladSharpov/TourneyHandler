using HandlerLibrary;
using HandlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandlerUI
{
    public partial class CreateTeamForm : Form
    {
        private ITeamRequester callingForm;
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm(ITeamRequester callingForm)
        {
            InitializeComponent();

            this.callingForm = callingForm;

            //CreateSampleData();

            WireUpLists();
        }

        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Corey" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Sue", LastName = "Storm" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Jane", LastName = "Smith" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Bill", LastName = "Jones" });
        }

        private void WireUpLists()
        {
            // TODO - find better way to refresh data binding

            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                var person = new PersonModel
                {
                    FirstName = firstNameValue.Text,
                    LastName = lastNameValue.Text,
                    EmailAddress = emailValue.Text,
                    CellphoneNumber = cellphoneValue.Text
                };

                person = GlobalConfig.Connection.CreatePerson(person);

                selectedTeamMembers.Add(person);
                WireUpLists();

                firstNameValue.Text = string.Empty;
                lastNameValue.Text = string.Empty;
                emailValue.Text = string.Empty;
                cellphoneValue.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Invalid input. Please, try again", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }

            if (emailValue.Text.Length == 0)
            {
                return false;
            }

            if (cellphoneValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            var person = selectTeamMemberDropDown.SelectedItem as PersonModel;

            if (person != null)
            {
                availableTeamMembers.Remove(person);
                selectedTeamMembers.Add(person);

                WireUpLists(); 
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            var person = teamMembersListBox.SelectedItem as PersonModel;

            if (person != null)
            {
                selectedTeamMembers.Remove(person);
                availableTeamMembers.Add(person);

                WireUpLists(); 
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            var team = new TeamModel()
            {
                TeamName = teamNameValue.Text,
                TeamMembers = selectedTeamMembers
            };

            team = GlobalConfig.Connection.CreateTeam(team);

            callingForm.TeamComplete(team);

            this.Close();
        }
    }
}
