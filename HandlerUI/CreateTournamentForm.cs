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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            teamsListBox.DataSource = null;
            teamsListBox.DataSource = selectedTeams;
            teamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = selectTeamDropDown.SelectedItem as TeamModel;

            if (team != null)
            {
                availableTeams.Remove(team);
                selectedTeams.Add(team);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            var prizeForm = new CreatePrizeForm(this);
            prizeForm.Show();
        }

        public void PrizeComplete(PrizeModel prize)
        {
            selectedPrizes.Add(prize);
            WireUpLists();
        }

        public void TeamComplete(TeamModel team)
        {
            selectedTeams.Add(team);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var teamForm = new CreateTeamForm(this);
            teamForm.Show();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = prizesListBox.SelectedItem as PrizeModel;

            if (prize != null)
            {
                selectedPrizes.Remove(prize);

                WireUpLists();
            }
        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = teamsListBox.SelectedItem as TeamModel;

            if (team != null)
            {
                selectedTeams.Remove(team);
                availableTeams.Add(team);

                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(entryFeeValue.Text, out decimal entryFee))
            {
                MessageBox.Show("Invalid entry fee value", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var tournament = new TournamentModel
            {
                TournamentName = tournamentNameValue.Text,
                EntryFee = entryFee
            };

            tournament.Prizes = selectedPrizes;
            tournament.EnteredTeams = selectedTeams;

            TournamentLogic.CreateRounds(tournament);

            GlobalConfig.Connection.CreateTournament(tournament);
        }
    }
}
