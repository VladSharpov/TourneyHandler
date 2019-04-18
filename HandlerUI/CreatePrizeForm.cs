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
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester callingForm;

        public CreatePrizeForm(IPrizeRequester callingForm)
        {
            InitializeComponent();

            this.callingForm = callingForm;
        }

        private void firstNameValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                var prize = new PrizeModel(
                    placeNameValue.Text, 
                    placeNumberValue.Text, 
                    prizeAmountValue.Text, 
                    prizePercentageValue.Text);

                prize = GlobalConfig.Connection.CreatePrize(prize);

                callingForm.PrizeComplete(prize);

                this.Close();

                //placeNameValue.Text = string.Empty;
                //placeNumberValue.Text = string.Empty;
                //prizeAmountValue.Text = "0";
                //prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("Invalid input. Please, try again", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool ValidateForm()
        {
            bool output = true;
            bool placeNumberIsValid = int.TryParse(placeNumberValue.Text, out int placeNumber);

            //TODO add XOR

            if (!placeNumberIsValid)
            {
                output = false;
            }

            if (placeNumber < 1)
            {
                output = false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                output = false;
            }

            bool prizeAmountIsValid = decimal.TryParse(prizeAmountValue.Text, out decimal prizeAmount);
            bool prizePercentageIsValid = double.TryParse(prizePercentageValue.Text, out double prizePercentage);

            if (!prizeAmountIsValid || !prizePercentageIsValid)
            {
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false; 
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }

            return output;
        }
    }
}
