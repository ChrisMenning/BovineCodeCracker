//------------------------------------------------------------------------------------------------------
// <art>                                                                                              --
//                                                                                                    --
//                                                                                                    --
//   ____     __                                                                                      --
//  /\  _`\  /\ \             __              /'\_/`\                         __                      --
//  \ \ \/\_\\ \ \___   _ __ /\_\    ____    /\      \     __    ___     ___ /\_\    ___      __      --
//   \ \ \/_/_\ \  _ `\/\`'__\/\ \  /',__\   \ \ \__\ \  /'__`\/' _ `\ /' _ `\/\ \ /' _ `\  /'_ `\    --
//    \ \ \L\ \\ \ \ \ \ \ \/ \ \ \/\__, `\   \ \ \_/\ \/\  __//\ \/\ \/\ \/\ \ \ \/\ \/\ \/\ \L\ \   --
//     \ \____/ \ \_\ \_\ \_\  \ \_\/\____/    \ \_\\ \_\ \____\ \_\ \_\ \_\ \_\ \_\ \_\ \_\ \____ \  --
//      \/___/   \/_/\/_/\/_/   \/_/\/___/      \/_/ \/_/\/____/\/_/\/_/\/_/\/_/\/_/\/_/\/_/\/___L\ \ --
//                                                                                            /\____/ --
//                                                                                            \_/__/  --
//                                                                                                    --
// </art>                                                                                             --
// <copyright file="GameController.cs" company="Chris Menning">                                       --
//     © Chris Menning 2017                                                                           --
// </copyright>                                                                                       --
//------------------------------------------------------------------------------------------------------

namespace BovineCodeCracker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The GameController form.
    /// </summary>
    public partial class GameController : Form
    {
        /// <summary>
        /// Player 1.
        /// </summary>
        private Player p1;

        /// <summary>
        /// Player 2.
        /// </summary>
        private Player p2;

        /// <summary>
        /// The active player whose turn it is.
        /// </summary>
        private Player activePlayer;

        /// <summary>
        /// The active player's opponent, whose turn it is not.
        /// </summary>
        private Player opponent;

        /// <summary>
        /// A value indicating whether or not Versus mode is active.
        /// </summary>
        private bool versus;

        /// <summary>
        /// The length of the code.
        /// </summary>
        private int codeLength;

        /// <summary>
        /// The depth of the code. (How many unique symbols?)
        /// </summary>
        private int codeDepth;

        /// <summary>
        /// The number of attempts allowed in a game.
        /// </summary>
        private int attemptsAllowed;

        /// <summary>
        /// Initializes a new instance of the GameController class.
        /// </summary>
        public GameController()
        {
            this.InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.comboBoxStyle.SelectedItem = "Classic";
            this.checkBoxP1isHuman.Checked = true;
            this.checkBoxP2isHuman.Checked = false;
            this.radioButtonVersus.Checked = true;

            this.textBoxP1name.Text = "Player One";
            this.textBoxP2name.Text = "BessieBot";
        }

        /// <summary>
        /// Gets or sets p1.
        /// </summary>
        public Player P1
        {
            get
            {
                return this.p1;
            }

            set
            {
                this.p1 = value;
            }
        }

        /// <summary>
        /// Gets or sets p2.
        /// </summary>
        public Player P2
        {
            get
            {
                return this.p2;
            }

            set
            {
                this.p2 = value;
            }
        }

        /// <summary>
        /// Gets or sets activePlayer.
        /// </summary>
        public Player ActivePlayer
        {
            get
            {
                return this.activePlayer;
            }

            set
            {
                this.activePlayer = value;
            }
        }

        /// <summary>
        /// Gets or sets opponent.
        /// </summary>
        public Player Opponent
        {
            get
            {
                return this.opponent;
            }

            set
            {
                this.opponent = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not Versus mode is true.
        /// </summary>
        public bool Versus
        {
            get
            {
                return this.versus;
            }

            set
            {
                this.versus = value;
            }
        }

        /// <summary>
        /// Gets or sets codeLength.
        /// </summary>
        public int CodeLength
        {
            get
            {
                return this.codeLength;
            }

            set
            {
                this.codeLength = value;
            }
        }

        /// <summary>
        /// Gets or sets codeDepth.
        /// </summary>
        public int CodeDepth
        {
            get
            {
                return this.codeDepth;
            }

            set
            {
                this.codeDepth = value;
            }
        }

        /// <summary>
        /// Gets or sets AttemptsAllowed.
        /// </summary>
        public int AttemptsAllowed
        {
            get
            {
                return this.attemptsAllowed;
            }

            set
            {
                this.attemptsAllowed = value;
            }
        }

        /// <summary>
        /// Changes Turns.
        /// </summary>
        public void ChangeTurns()
        {
            this.ActivePlayer.CurrentGuess = string.Empty;
            if (this.ActivePlayer == this.P1)
            {
                this.ActivePlayer = this.P2;
                this.Opponent = this.P1;
            }
            else
            {
                this.ActivePlayer = this.P1;
                this.Opponent = this.P2;
            }
        }

        /// <summary>
        /// The Submit button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            this.P1 = new Player(textBoxP1name.Text.ToString(), this.checkBoxP1isHuman.Checked);
            this.P2 = new Player(textBoxP2name.Text.ToString(), this.checkBoxP2isHuman.Checked);
            this.P1.AttemptsUsed = 0;
            this.P2.AttemptsUsed = 0;

            // Begin with P1's turn.
            this.ActivePlayer = this.P1;
            this.Opponent = this.P2;

            this.CodeLength = int.Parse(this.comboBoxCodeLength.SelectedItem.ToString());
            this.CodeDepth = int.Parse(this.comboBoxCodeDepth.SelectedItem.ToString());
            this.AttemptsAllowed = int.Parse(this.comboBoxGuesses.SelectedItem.ToString());

            this.Versus = radioButtonVersus.Checked;

            CodeEnterScreen codeEnter = new CodeEnterScreen(this, this.ActivePlayer);
            codeEnter.ShowDialog();
        }

        /// <summary>
        /// The Versus radio button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonVersus_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonVersus.Checked)
            {
                this.groupBoxP2.Enabled = true;
                this.labelP2name.Enabled = true;
                this.textBoxP2name.Enabled = true;
                this.checkBoxP2isHuman.Enabled = true;
            }
            else
            {
                this.groupBoxP2.Enabled = false;
                this.labelP2name.Enabled = false;
                this.textBoxP2name.Enabled = false;
                this.checkBoxP2isHuman.Enabled = false;
            }
        }

        /// <summary>
        /// The Code Depth.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The event arguments.</param>
        private void comboBoxCodeDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Code Depth cannot be less than code length, because we do not use a character more than once.
            if (int.Parse(this.comboBoxCodeDepth.SelectedItem.ToString()) < int.Parse(this.comboBoxCodeLength.SelectedItem.ToString()))
            {
                this.comboBoxCodeLength.SelectedItem = this.comboBoxCodeDepth.SelectedItem;
            }
        }

        /// <summary>
        /// The Code Length comboBox.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void comboBoxCodeLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCodeDepth.SelectedItem != null && int.Parse(comboBoxCodeLength.SelectedItem.ToString()) > int.Parse(comboBoxCodeDepth.SelectedItem.ToString()))
            {
                comboBoxCodeDepth.SelectedItem = comboBoxCodeLength.SelectedItem;
            }
        }

        /// <summary>
        /// Player 2's "is human" checkbox.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void checkBoxP2isHuman_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxP2isHuman.Checked == true)
            {
                this.textBoxP2name.Enabled = true;
                this.textBoxP2name.Text = "Player Two";
            }
            else
            {
                this.textBoxP2name.Enabled = false;
                this.textBoxP2name.Text = "BessieBot";
            }
        }

        /// <summary>
        /// The "Game Style".
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void comboBoxStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxStyle.SelectedIndex)
            {
                case 0:                                                 // Classic
                    comboBoxCodeLength.Enabled = false;
                    comboBoxCodeDepth.Enabled = false;
                    comboBoxGuesses.Enabled = false;
                    comboBoxCodeLength.SelectedItem = "4";
                    comboBoxCodeDepth.SelectedItem = "6";
                    comboBoxGuesses.SelectedItem = "8";
                    break;
                case 1:                                                 // Deluxe
                    comboBoxCodeLength.Enabled = false;
                    comboBoxCodeDepth.Enabled = false;
                    comboBoxGuesses.Enabled = false;
                    comboBoxCodeLength.SelectedItem = "5";
                    comboBoxCodeDepth.SelectedItem = "10";
                    comboBoxGuesses.SelectedItem = "12";
                    break;
                case 2:                                                 // Supreme
                    comboBoxCodeLength.Enabled = false;
                    comboBoxCodeDepth.Enabled = false;
                    comboBoxGuesses.Enabled = false;
                    comboBoxCodeLength.SelectedItem = "6";
                    comboBoxCodeDepth.SelectedItem = "10";
                    comboBoxGuesses.SelectedItem = "14";
                    break;
                case 3:                                                 // Glacial
                    comboBoxCodeLength.Enabled = false;
                    comboBoxCodeDepth.Enabled = false;
                    comboBoxGuesses.Enabled = false;
                    comboBoxCodeLength.SelectedItem = "10";
                    comboBoxCodeDepth.SelectedItem = "10";
                    comboBoxGuesses.SelectedItem = "16";
                    break;
                case 4:                                                 // Custom
                    comboBoxCodeLength.Enabled = true;
                    comboBoxCodeDepth.Enabled = true;
                    comboBoxGuesses.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// The click event for "how to play" button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonHowToPlay_Click(object sender, EventArgs e)
        {
            HowToPlay howTo = new HowToPlay();
            howTo.Show();
        }

        /// <summary>
        /// The click event for the Exit button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
