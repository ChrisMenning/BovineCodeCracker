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
// <copyright file="CodeEnterScreen.cs" company="Chris Menning">                                      --
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
    using System.Media;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The form on which the player sets their secret code.
    /// </summary>
    public partial class CodeEnterScreen : Form
    {
        /// <summary>
        /// A reference to the GameControl form.
        /// </summary>
        private GameController gameControl;

        /// <summary>
        /// A contextual reference to the player whose turn it currently is at any given moment.
        /// </summary>
        private Player whoseTurn;

        /// <summary>
        /// A list of all Pickers on this form.
        /// </summary>
        private List<Picker> pickers;

        /// <summary>
        /// A list of single-character-long textboxes arranged in a series, for inputting a secret code.
        /// </summary>
        private List<TextBox> codeEntryList = new List<TextBox>();

        /// <summary>
        /// The "generate code" sound effect.
        /// </summary>
        private SoundPlayer soundGenerate = new SoundPlayer(BovineCodeCracker.Properties.Resources._387217__spiceprogram__ping_1);

        /// <summary>
        /// The "confirm" sound effect.
        /// </summary>
        private SoundPlayer soundConfirm = new SoundPlayer(BovineCodeCracker.Properties.Resources._387216__spiceprogram__pock_1);

        /// <summary>
        /// An integer similar to the SquareSize on the gameBoard, but this one does not grow or shrink.
        /// </summary>
        private int specialSquareSize = 58;

        /// <summary>
        /// A string for compiling the user's input, prior to validating and prior to becoming the player's secret code.
        /// </summary>
        private string codeText;

        /// <summary>
        /// Initializes a new instance of the CodeEnterScreen class.
        /// </summary>
        /// <param name="gameControl">A reference to the GameControl form.</param>
        /// <param name="whoseTurn">A reference to the player whose turn it currently is.</param>
        public CodeEnterScreen(GameController gameControl, Player whoseTurn)
        {
            this.InitializeComponent();

            this.gameControl = gameControl;
            this.whoseTurn = whoseTurn;
            this.Text = whoseTurn.Name + ", Enter Secret Code";
            
            this.UpdateMessage();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.DrawPickerBox();
            this.DrawEntryBoxes();

            if (gameControl.Versus == false)
            {
                gameControl.ActivePlayer.SecretCode = string.Empty;
                this.GenerateSecretCode();
            }

            this.codeEntryList.First().Focus();
        }

        /// <summary>
        /// Gets or sets the Pickers list.
        /// </summary>
        public List<Picker> Pickers
        {
            get
            {
                return this.pickers;
            }

            set
            {
                this.pickers = value;
            }
        }

        /// <summary>
        /// Selects a Picker by its ID number.
        /// </summary>
        /// <param name="id">The Picker's id.</param>
        /// <returns>A Picker.</returns>
        public Picker GetPickerByID(int id)
        {
            foreach (Picker p in this.Pickers)
            {
                if (p.Id == id)
                {
                    p.BackColor = Color.Cyan;
                    return p;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the message on the form.
        /// </summary>
        private void UpdateMessage()
        {
            this.labelMessage.TextAlign = ContentAlignment.MiddleCenter;

            if (this.gameControl.Versus == false)
            {
                this.labelMessage.Text = this.gameControl.ActivePlayer.Name + " Please wait...";
            }
            else if (this.gameControl.Versus == true && this.gameControl.ActivePlayer.IsHuman == true && this.gameControl.Opponent.IsHuman == false)
            {
                this.labelMessage.Text = this.gameControl.ActivePlayer.Name + ", please choose your secret code.";
            }
            else if (this.gameControl.Versus == true && this.gameControl.ActivePlayer.IsHuman == false && this.gameControl.Opponent.IsHuman == true)
            {
                this.labelMessage.Text = this.gameControl.ActivePlayer.Name + ", please choose your secret code.";

                this.AIclicks();
            }
            else if (this.gameControl.Versus == true && this.gameControl.ActivePlayer.IsHuman == true && this.gameControl.Opponent.IsHuman == true)
            {
                this.labelMessage.Text = this.gameControl.Opponent.Name + ", please look away " + "\n" + "while " + this.gameControl.ActivePlayer.Name + "chooses their secret code.";
            }
        }

        /// <summary>
        /// Draws the entry boxes.
        /// </summary>
        private void DrawEntryBoxes()
        {
            IntToSymbol its = new IntToSymbol();
            int xPos = 20;
            for (int i = 0; i < this.gameControl.CodeLength; i++)
            {
                TextBox textboxEntry = new TextBox();
                textboxEntry.Location = new Point(xPos, 150);
                textboxEntry.Font = new Font("Century Gothic", 28, FontStyle.Bold);
                textboxEntry.Width = this.specialSquareSize;
                textboxEntry.Height = this.specialSquareSize;
                textboxEntry.Text = "[ ]";
                textboxEntry.BorderStyle = BorderStyle.FixedSingle;
                textboxEntry.TextAlign = HorizontalAlignment.Center;
                textboxEntry.MaxLength = 1;

                textboxEntry.Click += (s, z) =>
                {
                    textboxEntry.Text = gameControl.ActivePlayer.SelectedPicker.Text;
                    soundConfirm.Play();
                };
                textboxEntry.TextChanged += (s, z) =>
                {
                    int tempInt;

                    bool result = int.TryParse(textboxEntry.Text.ToString(), out tempInt);
                    if (result == true)
                    {
                        soundConfirm.Play();

                        textboxEntry.Text = its.convert(tempInt);

                        foreach (Picker p in Pickers)
                        {
                            if (p.Id == tempInt)
                            {
                                gameControl.ActivePlayer.SelectedPicker = p;
                                p.BackColor = Color.Cyan;
                            }
                            else
                            {
                                p.BackColor = p.InitialColor;
                            }
                        }
                    }

                    buttonSubmit.Focus();
                };

                xPos = xPos + this.specialSquareSize;

                this.Controls.Add(textboxEntry);
                textboxEntry.Name = textboxEntry.Name + i;
                this.codeEntryList.Add(textboxEntry);

                this.buttonSubmit.Location = new Point((this.specialSquareSize * this.gameControl.CodeLength) + 20, 150);
                this.buttonSubmit.Width = this.specialSquareSize * 2;
                this.buttonGenerateCode.Width = (this.specialSquareSize * this.gameControl.CodeLength) + (this.specialSquareSize * 2);
                this.Width = (this.specialSquareSize * this.gameControl.CodeLength) + (this.specialSquareSize * 3);
            }
        }

        /// <summary>
        /// The click event for the submit button.
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The event arguments</param>
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (this.gameControl.ActivePlayer.SecretCode == string.Empty)
            {
                foreach (TextBox tb in this.codeEntryList)
                {
                    this.codeText = this.codeText + tb.Text;
                }

                var uniques = (new HashSet<char>(this.codeText)).Count;

                if (uniques != this.gameControl.CodeLength)
                {
                    MessageBox.Show("No repeating characters allowed.");
                    this.codeText = string.Empty;
                }
                else
                {
                    this.gameControl.ActivePlayer.SecretCode = this.codeText;
                }
            }
            
            if (this.gameControl.Versus == true)
            {
                // If P1's code is set, but P2's code is not, change turns, close screen, and reopen this screen again.
                if (this.gameControl.ActivePlayer.SecretCode != string.Empty && this.gameControl.Opponent.SecretCode == string.Empty)
                {
                    this.gameControl.ChangeTurns();
                    CodeEnterScreen player2CodeEnterScreen = new CodeEnterScreen(this.gameControl, this.gameControl.ActivePlayer);
                    player2CodeEnterScreen.Show();
                    this.Close();
                }

                // If both players have their codes entered, open a new GameBoard.
                if (this.gameControl.ActivePlayer.SecretCode != string.Empty && this.gameControl.Opponent.SecretCode != string.Empty)
                {
                    GameBoard gameBoard = new GameBoard(this.gameControl);
                    this.gameControl.ChangeTurns();
                    gameBoard.Show();
                    this.Close();
                }
            }
            else
            {
                // Game is in single player mode.
                if (this.gameControl.ActivePlayer.SecretCode != string.Empty)
                {
                    GameBoard gameBoard = new GameBoard(this.gameControl);
                    gameBoard.Show();
                    gameBoard.BringToFront();
                    gameBoard.Focus();                    
                    this.Close();
                }
            } 
        }

        /// <summary>
        /// The click event for the Generate Code button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonGenerateCode_Click(object sender, EventArgs e)
        {
            this.soundGenerate.Play();
            this.gameControl.ActivePlayer.SecretCode = string.Empty;
            this.GenerateSecretCode();
            if (this.gameControl.ActivePlayer.IsHuman == false)
            {
                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    // Don't show the secret code.
                }
            }
            else
            {
                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    foreach (TextBox tb in this.codeEntryList)
                    {
                        if (tb.Name.Contains(i.ToString()))
                        {
                            tb.Text = this.gameControl.ActivePlayer.SecretCode[i].ToString();
                        }
                    }
                }

                SendKeys.Send("{TAB}");
            }
        }

        /// <summary>
        /// Generates a Secret Code instead of manually picking it.
        /// </summary>
        private void GenerateSecretCode()
        {
            IntToSymbol its = new IntToSymbol();
            Random rand = new Random();
            List<int> randomNumbers = Enumerable.Range(0, this.gameControl.CodeDepth).OrderBy(x => rand.Next()).Take(this.gameControl.CodeDepth).ToList();

            for (int i = 0; i < this.gameControl.CodeLength; i++)
            {
                Console.WriteLine("Randomly selecting character " + i);
                int tempChar = randomNumbers[rand.Next(0, randomNumbers.Count() - 1)];

                string symbol = its.convert(i);

                if (this.gameControl.Versus == true)
                {
                    this.gameControl.ActivePlayer.SecretCode = this.gameControl.ActivePlayer.SecretCode + symbol;
                }
                else
                {
                    this.gameControl.ActivePlayer.SecretCode = this.gameControl.ActivePlayer.SecretCode + "0"; // You have to give the player some code, even though it's not used.
                    this.gameControl.Opponent.SecretCode = this.gameControl.Opponent.SecretCode + symbol;
                }

                randomNumbers.Remove(tempChar);
            }

            if (this.gameControl.Versus == false)
            {
                buttonSubmit.Focus();
                SendKeys.Send("{ENTER}");
            }
        }

        /// <summary>
        /// Draws the box of Pickers.
        /// </summary>
        private void DrawPickerBox()
        {
            IntToSymbol its = new IntToSymbol();
            this.Pickers = new List<Picker>();
            int pickerXpos = 20;

            for (int i = 0; i < this.gameControl.CodeDepth; i++)
            {
                Picker picker = new Picker(this, this.gameControl, i);
                picker.Name = "Picker" + i;

                picker.Text = its.convert(i);

                picker.Width = 35;
                picker.Height = 35;

                picker.Location = new Point(pickerXpos, 0);
                pickerXpos = pickerXpos + 35;
                this.Controls.Add(picker);
                this.Pickers.Add(picker);
            }

            this.gameControl.ActivePlayer.SelectedPicker = this.GetPickerByID(0);
        }

        /// <summary>
        /// A little wait utility.
        /// </summary>
        /// <param name="waitTime">Wait time.</param>
        /// <returns>An asynchronous Task.</returns>
        private async Task TaskDelay(int waitTime)
        {
            await Task.Delay(waitTime);
        }

        /// <summary>
        /// The AI's ability to use the form.
        /// </summary>
        private async void AIclicks()
        {
            await this.TaskDelay(10);
            buttonGenerateCode.Focus();

            SendKeys.SendWait("{ENTER}");
            await this.TaskDelay(5);
            buttonGenerateCode.Enabled = false;

            await this.TaskDelay(5);
            buttonSubmit.Focus();
            SendKeys.SendWait("{ENTER}");
            await this.TaskDelay(5);
            buttonSubmit.Enabled = false;
        }
    }
}
