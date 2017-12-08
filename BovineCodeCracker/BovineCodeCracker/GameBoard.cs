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
// <copyright file="GameBoard.cs" company="Chris Menning">                                            --
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
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The GameBoard Form.
    /// </summary>
    public partial class GameBoard : Form
    {
        /// <summary>
        /// A reference to the GameController form.
        /// </summary>
        private GameController gameControl;

        /// <summary>
        /// The size of a square. This becomes a base-unit for responsive resizing.
        /// </summary>
        private int squareSize;

        /// <summary>
        /// Tracks the number of spots within a guess that have been placed, for detecting a completed answer.
        /// </summary>
        private int spotsUsed;

        /// <summary>
        /// An array of GuessSpots used in the construction of the board.
        /// </summary>
        private GuessSpot[,] attemptsArrayP1;

        /// <summary>
        /// An array of GuessSpots used in the construction of the board.
        /// </summary>
        private GuessSpot[,] attemptsArrayP2;

        /// <summary>
        /// A list of Results Labels for Player 1's board.
        /// </summary>
        private List<Label> listOfResultsP1;

        /// <summary>
        /// A list of Results Labels for Player 2's board.
        /// </summary>
        private List<Label> listOfResultsP2;

        /// <summary>
        /// A list of Pickers on this form.
        /// </summary>
        private List<Picker> pickers;

        /// <summary>
        /// An integer representing what turn number this is. The first turn is turn 0.
        /// </summary>
        private int thisTurn;

        /// <summary>
        /// A value indicating whether or not the game is over.
        /// </summary>
        private bool gameOver;

        /// <summary>
        /// The "wrong" sound effect.
        /// </summary>
        private SoundPlayer soundWrong = new SoundPlayer(BovineCodeCracker.Properties.Resources._67454__splashdust__negativebeep);

        /// <summary>
        /// The "win" sound effect.
        /// </summary>
        private SoundPlayer soundWin = new SoundPlayer(BovineCodeCracker.Properties.Resources._391539__mativve__electro_win_sound);

        /// <summary>
        /// The "lose" sound effect.
        /// </summary>
        private SoundPlayer soundLose = new SoundPlayer(BovineCodeCracker.Properties.Resources._391536__mativve__electro_loose_sound);

        /// <summary>
        /// The "bull or cow detected" sound effect.
        /// </summary>
        private SoundPlayer soundBullOrCow = new SoundPlayer(BovineCodeCracker.Properties.Resources._387217__spiceprogram__ping_1);

        /// <summary>
        /// A label for outputting messages from the AI.
        /// </summary>
        private Label botMouth;

        /// <summary>
        /// Initializes a new instance of the GameBoard class.
        /// </summary>
        /// <param name="gameControl">A reference to the GameController form.</param>
        public GameBoard(GameController gameControl)
        {
            this.GameOver = false;

            this.InitializeComponent();
            this.GameControl = gameControl;
            this.SpotsUsed = 0;

            // Set up the players' results lists' relationships to board elements.
            this.listOfResultsP1 = this.gameControl.P1.ListOfResults;
            this.listOfResultsP2 = this.gameControl.P2.ListOfResults;

            // Set up the board.
            // Responsive SquareSize
            this.SquareSize = 500 / this.GameControl.AttemptsAllowed;
            if (this.GameControl.Versus == true)
            {
                this.Width = (this.SquareSize * this.GameControl.CodeLength * 4) + (this.SquareSize * 1 / 4);
                this.DrawP1CrackingBoard();
                this.DrawP2CrackingBoard();
                this.MakeBotMouth();
            }
            else
            {
                this.Width = ((this.SquareSize * this.GameControl.CodeLength) * 2) + this.SquareSize;
                this.DrawP1CrackingBoard();
            }

            this.Height = (this.SquareSize * this.GameControl.AttemptsAllowed) + (this.SquareSize * 3) + (this.SquareSize * 1 / 2);

            this.DrawPickerBox();

            this.StartPosition = FormStartPosition.CenterScreen;

            this.thisTurn = this.gameControl.ActivePlayer.AttemptsUsed;
        }

        /// <summary>
        /// Destroys the GameBoard, freeing up memory.
        /// </summary>
        ~GameBoard()
        {
            GameControl.P1.MyInputList.Clear();
            GameControl.P2.MyInputList.Clear();
            GameControl.P1.MyOutputList.Clear();
            GameControl.P2.MyOutputList.Clear();
            GameControl.P1.SecretCode = string.Empty;
            GameControl.P2.SecretCode = string.Empty;
        }

        /// <summary>
        /// Gets or sets the GameController field.
        /// </summary>
        public GameController GameControl
        {
            get
            {
                return this.gameControl;
            }

            set
            {
                this.gameControl = value;
            }
        }

        /// <summary>
        /// Gets or sets the Pickers list field.
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
        /// Gets or sets squareSize.
        /// </summary>
        public int SquareSize
        {
            get
            {
                return this.squareSize;
            }

            set
            {
                this.squareSize = value;
            }
        }

        /// <summary>
        /// Gets or sets spotsUsed.
        /// </summary>
        public int SpotsUsed
        {
            get
            {
                return this.spotsUsed;
            }

            set
            {
                this.spotsUsed = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the game is over.
        /// This is used for preventing scoring of the other player after the game is over.
        /// </summary>
        public bool GameOver
        {
            get
            {
                return this.gameOver;
            }

            set
            {
                this.gameOver = value;
            }
        }

        /// <summary>
        /// Select a Picker after looking it up by ID number.
        /// </summary>
        /// <param name="id">The passed-in id.</param>
        /// <returns>A Picker</returns>
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
        /// The method that scores the guess and checks for winners or losers.
        /// </summary>
        /// <param name="row">The passed-in number of the row for the guess that's being checked.</param>
        public void ScoreTheGuess(int row)
        {
            if (this.GameOver == false)
            {
                if (this.gameControl.ActivePlayer == this.gameControl.P1)
                {
                    for (int i = 0; i < this.gameControl.CodeLength; i++)
                    {
                        this.gameControl.ActivePlayer.CurrentGuess += this.attemptsArrayP1[i, row].Text;
                    }
                }
                else
                {
                    for (int i = 0; i < this.gameControl.CodeLength; i++)
                    {
                        this.gameControl.ActivePlayer.CurrentGuess += this.attemptsArrayP2[i, row].Text;
                    }
                }

                this.gameControl.ActivePlayer.Guesses.Add(this.gameControl.ActivePlayer.CurrentGuess);

                // Clear the label's text prior to adding cows and bulls to it.
                this.gameControl.ActivePlayer.ListOfResults[row].Text = string.Empty;

                // Check for bulls. (Correct symbol in correct space.)
                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    if (this.gameControl.ActivePlayer.CurrentGuess.Length > 0 && this.gameControl.ActivePlayer.CurrentGuess[i].ToString() == this.gameControl.Opponent.SecretCode[i].ToString())
                    {
                        this.gameControl.ActivePlayer.ListOfResults[row].Text += "🐂";
                        this.soundBullOrCow.Play();

                        // Remove the character so it is not counted as a cow.
                        this.gameControl.ActivePlayer.CurrentGuess = Regex.Replace(this.gameControl.ActivePlayer.CurrentGuess, this.gameControl.Opponent.SecretCode[i].ToString(), " ");
                    }
                }

                if (this.gameControl.ActivePlayer.ListOfResults[row].Text.Contains("🐂"))
                {
                    // New line.
                    this.gameControl.ActivePlayer.ListOfResults[row].Text += "\n";
                }

                // Check for cows. (Correct symbol in wrong space.)
                foreach (int secretSpot in this.gameControl.Opponent.SecretCode)
                {
                    foreach (int guessSpot in this.gameControl.ActivePlayer.CurrentGuess)
                    {
                        if (guessSpot == secretSpot)
                        {
                            // Console.WriteLine("Cow!");
                            this.gameControl.ActivePlayer.ListOfResults[row].Text += "🐄";
                            this.soundBullOrCow.Play();
                            break;
                        }
                    }
                }

                if (!this.gameControl.ActivePlayer.ListOfResults[row].Text.Contains("🐂") &&
                    !this.gameControl.ActivePlayer.ListOfResults[row].Text.Contains("🐄"))
                {
                    this.soundWrong.Play();
                }

                // Change font to appropriate size.
                if (this.gameControl.ActivePlayer.ListOfResults[row].Text.Contains("🐂") &&
                    this.gameControl.ActivePlayer.ListOfResults[row].Text.Contains("🐄"))
                {
                    this.gameControl.ActivePlayer.ListOfResults[row].Font = new Font("Consolas", this.SquareSize * 1 / 3, FontStyle.Regular);
                }
                else
                {
                    this.gameControl.ActivePlayer.ListOfResults[row].Font = new Font("Consolas", this.SquareSize * 2 / 3, FontStyle.Regular);
                }

                this.gameControl.ActivePlayer.ListOfResults[row].Invalidate();
                this.gameControl.ActivePlayer.ListOfResults[row].Update();

                // Increment the attempts used.
                this.gameControl.ActivePlayer.AttemptsUsed++;

                // Check for wins.
                if (Regex.Matches(this.gameControl.ActivePlayer.ListOfResults[row].Text, "🐂").Count == this.gameControl.CodeLength)
                {
                    this.soundWin.Play();
                    this.GameOver = true;
                    if (this.gameControl.Versus == true)
                    {
                        MessageBox.Show(this.gameControl.ActivePlayer.Name + " cracked " + this.gameControl.Opponent.Name + "'s code! " +
                           "\n" + this.gameControl.ActivePlayer.Name + " wins!");
                    }
                    else
                    {
                        MessageBox.Show(this.gameControl.ActivePlayer.Name + " wins!");
                    }

                    this.gameControl.WindowState = FormWindowState.Normal;
                    this.Close();
                }

                // Check for losses.
                if (this.gameControl.Versus == false && this.thisTurn + 1 == this.gameControl.AttemptsAllowed)
                {
                    this.soundLose.Play();
                    this.GameOver = true;
                    System.Threading.Thread.Sleep(50);
                    MessageBox.Show("You lose." + "the secret code was " + this.gameControl.Opponent.SecretCode + ".");
                    this.gameControl.WindowState = FormWindowState.Normal;
                    this.Close();
                }
                else if (this.gameControl.Versus == true && this.gameControl.ActivePlayer == this.gameControl.P2 &&
                    this.thisTurn == this.gameControl.AttemptsAllowed && this.GameOver == false)
                {
                    this.soundLose.Play();
                    this.GameOver = true;
                    System.Threading.Thread.Sleep(50);
                    MessageBox.Show(this.gameControl.ActivePlayer.Name + " loses." + "\n" + this.gameControl.Opponent.Name + "'s secret code was " + this.gameControl.Opponent.SecretCode + ".");
                    this.gameControl.WindowState = FormWindowState.Normal;
                    this.Close();
                }

                this.SpotsUsed = 0;
                if (this.gameControl.Versus == true && this.GameOver == false)
                {
                    this.gameControl.ChangeTurns();
                }
                else
                {
                    this.gameControl.ActivePlayer.CurrentGuess = string.Empty;
                }

                // If, after having just changed turns, it is again P1's turn, Move to Next Row.
                if (this.gameControl.ActivePlayer == this.gameControl.P1 && this.GameOver == false)
                {
                    this.MoveToNextRow();
                }
                else if (this.gameControl.Versus == true && this.gameControl.ActivePlayer == this.gameControl.P2 && this.GameOver == false)
                {
                    // But if it's P2's turn, enable this row's GuessSpots.
                    for (int i = 0; i < this.gameControl.CodeLength; i++)
                    {
                        this.attemptsArrayP2[i, this.thisTurn].Enabled = true;
                        this.attemptsArrayP2[0, this.thisTurn].Focus();

                        if ((this.gameControl.Versus == true && this.gameControl.ActivePlayer.IsHuman == false) || this.gameControl.Versus == false)
                        {
                            this.AIFirstMove();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws Player 1's board.
        /// </summary>
        private void DrawP1CrackingBoard()
        {
            // Define an array of GuessSpots
            this.attemptsArrayP1 = new GuessSpot[this.GameControl.CodeLength, this.GameControl.AttemptsAllowed];

            // Declare a temporary results label.
            LabelResults labelOutputResults;

            int xPos = this.SquareSize * 1 / 4;
            int yPos = (this.SquareSize * this.GameControl.AttemptsAllowed) + this.SquareSize;

            // Make a name label.
            Label p1nameLabel = new Label();
            p1nameLabel.Location = new Point(this.SquareSize * 1 / 4, this.SquareSize);
            p1nameLabel.Text = this.gameControl.P1.Name;
            p1nameLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 3, FontStyle.Bold);
            p1nameLabel.AutoSize = true;
            p1nameLabel.BackColor = Color.Transparent;
            p1nameLabel.ForeColor = Color.White;
            this.Controls.Add(p1nameLabel);

            // Label parts of the board.
            Label p1guessesLabel = new Label();
            p1guessesLabel.Location = new Point(this.SquareSize * 1 / 4, (this.SquareSize * 2) * 3 / 4);
            p1guessesLabel.Text = "Attempts:";
            p1guessesLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 5);
            p1guessesLabel.AutoSize = true;
            p1guessesLabel.BackColor = Color.Transparent;
            p1guessesLabel.ForeColor = Color.White;
            this.Controls.Add(p1guessesLabel);
            Label p1resultsHeaderLabel = new Label();
            p1resultsHeaderLabel.Location = new Point((this.SquareSize * this.gameControl.CodeLength) + (this.SquareSize * 1 / 4), (this.SquareSize * 2) * 3 / 4);
            p1resultsHeaderLabel.Text = "Results:";
            p1resultsHeaderLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 5);
            p1resultsHeaderLabel.AutoSize = true;
            this.Controls.Add(p1resultsHeaderLabel);
            p1resultsHeaderLabel.BackColor = Color.Transparent;
            p1resultsHeaderLabel.ForeColor = Color.White;

            for (int y = 0; y < this.GameControl.AttemptsAllowed; y++)
            {
                GuessSpot gs;

                for (int x = 0; x < this.GameControl.CodeLength; x++)
                {
                    gs = this.attemptsArrayP1[x, y];
                    gs = new GuessSpot(this.GameControl, this);
                    gs.Row = y;
                    gs.Col = x;
                    gs.Width = this.SquareSize;
                    gs.Height = this.SquareSize;
                    gs.Location = new Point(xPos, yPos);
                    xPos = xPos + this.SquareSize;

                    // Finally, add it.
                    this.Controls.Add(gs);
                    this.attemptsArrayP1[x, y] = gs;
                }

                labelOutputResults = new LabelResults(this);
                this.listOfResultsP1.Add(labelOutputResults);
                labelOutputResults.Location = new Point(xPos, yPos);
                this.Controls.Add(labelOutputResults);
                labelOutputResults.Name = labelOutputResults.Name + y;

                xPos = this.SquareSize * 1 / 4;
                yPos = yPos - this.SquareSize;
            }

            foreach (GuessSpot gs in this.attemptsArrayP1)
            {
                if (gs.Row != this.GameControl.ActivePlayer.AttemptsUsed)
                {
                    gs.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Draws Player 2's board.
        /// </summary>
        private void DrawP2CrackingBoard()
        {
            // Console.WriteLine("Drawing P2's Cracking Board");
            // Define an array of GuessSpots
            this.attemptsArrayP2 = new GuessSpot[this.GameControl.CodeLength, this.GameControl.AttemptsAllowed];

            // Declare a temporary results label.
            LabelResults labelOutputResults;

            // Make a name label.
            Label p2nameLabel = new Label();
            p2nameLabel.Location = new Point(this.SquareSize * this.gameControl.CodeLength * 2, this.SquareSize);
            p2nameLabel.Text = this.gameControl.P2.Name;
            p2nameLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 3, FontStyle.Bold);
            p2nameLabel.AutoSize = true;
            p2nameLabel.BackColor = Color.Transparent;
            p2nameLabel.ForeColor = Color.White;
            this.Controls.Add(p2nameLabel);

            // Label parts of the board.
            Label p2guessesLabel = new Label();
            p2guessesLabel.Location = new Point((this.SquareSize * this.gameControl.CodeLength) * 2, (this.SquareSize * 2) * 3 / 4);
            p2guessesLabel.Text = "Attempts:";
            p2guessesLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 5);
            p2guessesLabel.AutoSize = true;
            p2guessesLabel.BackColor = Color.Transparent;
            p2guessesLabel.ForeColor = Color.White;
            this.Controls.Add(p2guessesLabel);
            Label p2resultsHeaderLabel = new Label();
            p2resultsHeaderLabel.Location = new Point(this.SquareSize * this.gameControl.CodeLength * 3, (this.SquareSize * 2) * 3 / 4);
            p2resultsHeaderLabel.Text = "Results:";
            p2resultsHeaderLabel.Font = new Font("Century Gothic", this.SquareSize * 1 / 5);
            p2resultsHeaderLabel.AutoSize = true;
            p2resultsHeaderLabel.BackColor = Color.Transparent;
            p2resultsHeaderLabel.ForeColor = Color.White;
            this.Controls.Add(p2resultsHeaderLabel);

            int xPos = (this.SquareSize * this.GameControl.CodeLength * 2) - this.SquareSize;
            int yPos = (this.SquareSize * this.GameControl.AttemptsAllowed) + this.SquareSize;

            for (int y = 0; y < this.GameControl.AttemptsAllowed; y++)
            {
                GuessSpot gs;

                for (int x = 0; x < this.GameControl.CodeLength; x++)
                {
                    gs = this.attemptsArrayP2[x, y];
                    gs = new GuessSpot(this.GameControl, this);
                    gs.Row = y;
                    gs.Col = x;
                    gs.Width = this.SquareSize;
                    gs.Height = this.SquareSize;
                    xPos = xPos + gs.Width;
                    gs.Location = new Point(xPos, yPos);

                    // Finally, add it.
                    this.Controls.Add(gs);
                    this.attemptsArrayP2[x, y] = gs;
                }

                labelOutputResults = new LabelResults(this);
                this.listOfResultsP2.Add(labelOutputResults);
                labelOutputResults.Location = new Point(xPos + this.SquareSize, yPos);
                this.Controls.Add(labelOutputResults);
                labelOutputResults.Name = labelOutputResults.Name + y;

                xPos = (this.SquareSize * this.GameControl.CodeLength * 2) - this.SquareSize;
                yPos = yPos - this.SquareSize;
            }

            foreach (GuessSpot gs in this.attemptsArrayP2)
            {
                if (gs.Row != this.GameControl.ActivePlayer.AttemptsUsed)
                {
                    gs.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Draws the Symbol Pickers on the board.
        /// </summary>
        private void DrawPickerBox()
        {
            this.Pickers = new List<Picker>();
            int pickerXpos = this.SquareSize * 1 / 4;

            for (int i = 0; i < this.gameControl.CodeDepth; i++)
            {
                IntToSymbol its = new IntToSymbol();
                Picker picker = new Picker(this, this.gameControl, i);
                picker.Name = "Picker" + i;

                picker.Text = its.convert(i);

                picker.Width = this.SquareSize * 2 / 3;
                picker.Height = this.SquareSize * 2 / 3;

                picker.Location = new Point(pickerXpos, 0);
                pickerXpos = pickerXpos + (this.SquareSize * 2 / 3);
                this.Controls.Add(picker);
                this.Pickers.Add(picker);
            }

            this.gameControl.ActivePlayer.SelectedPicker = this.GetPickerByID(0);
        }

        /// <summary>
        /// After scoring a turn, this moves to the next row.
        /// </summary>
        private void MoveToNextRow()
        {
            Console.WriteLine(" Trying to move to next row.");
            if (this.GameOver == false)
            {
                if (this.thisTurn < this.gameControl.AttemptsAllowed)
                {
                    this.thisTurn++;
                }

                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    this.attemptsArrayP1[i, this.thisTurn].Enabled = true;
                }

                this.attemptsArrayP1[0, this.thisTurn].Focus();
            }
        }

        /// <summary>
        /// The AI's first move.
        /// </summary>
        private void AIFirstMove()
        {
            if (this.gameControl.ActivePlayer.AttemptsUsed < 1)
            {
                IntToSymbol its = new IntToSymbol();
                Random rand = new Random();
                List<int> randomNumbers = Enumerable.Range(0, this.gameControl.CodeDepth).OrderBy(x => rand.Next()).Take(this.gameControl.CodeDepth).ToList();

                string codeToBuildOn = string.Empty;

                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    int tempChar = randomNumbers[rand.Next(0, randomNumbers.Count() - 1)];
                    codeToBuildOn = codeToBuildOn + its.convert(tempChar);
                    randomNumbers.Remove(tempChar);
                }

                this.AIenterGuess(codeToBuildOn);
            }
            else
            {
                this.AIsolver();
            }

            this.ScoreTheGuess(this.gameControl.ActivePlayer.AttemptsUsed);
        }

        /// <summary>
        /// The primary logic for the AI's solving algorithm.
        /// </summary>
        private async void AIsolver()
        {
            if (this.GameOver == false)
            {
                await this.UpdateBotMouth("Hang on...");

                // If this is the first turn, add all permutations to the list.
                if (this.thisTurn < 2)
                {
                    PermutationMaker pm = new PermutationMaker();
                    // Add all permutations to an input list.
                    foreach (string s in pm.Permutations(this.gameControl.CodeLength, this.gameControl.CodeDepth))
                    {
                        this.gameControl.ActivePlayer.MyInputList.Add(s);
                    }
                }

                // Figure out which spots in previous guesses may have been responsible for a result containing 
                // either bulls or cows. 
                Console.WriteLine("Finding Bulls or Cows");
                FindPossibleBullsOrCows();
                Console.WriteLine("Input list size: " + gameControl.ActivePlayer.MyInputList.Count);

                // Infer where bulls might be, and add them 
                Console.WriteLine("Inferring where bulls may be");
                BullSense(this.AIinferredBulls());

                Console.WriteLine("Deducing where bulls are not.");
                CowSense(this.AIdeducedNoBulls());

                // Remove used guesses.
                await this.RemoveUsedGuessesFromPermuations(this.gameControl.ActivePlayer.MyInputList);

                // Now pick one from the list at random.
                Random rand = new Random();
                string finalOutputGuess = string.Empty;
                if (this.gameControl.ActivePlayer.MyInputList.Count() > 1)
                {
                    int randomSelect = rand.Next(0, this.gameControl.ActivePlayer.MyInputList.Count());

                    finalOutputGuess = this.gameControl.ActivePlayer.MyInputList[randomSelect];
                    this.botMouth.Text = "I'm ready. (" + this.gameControl.ActivePlayer.MyInputList.Count + " possibilites.)";
                    this.botMouth.Invalidate();
                    this.botMouth.Update();
                }
                else
                {
                    // There's a strong chance the last guess was a permuation of the actual code.
                    if (this.gameControl.ActivePlayer.MyInputList[0].ToString() != this.gameControl.ActivePlayer.Guesses[this.gameControl.ActivePlayer.AttemptsUsed - 1].ToString())
                    {
                        finalOutputGuess = this.gameControl.ActivePlayer.MyInputList[0];
                    }
                    else
                    {
                        MessageBox.Show("The only number left on my list is the same as the last.");
                        finalOutputGuess = this.gameControl.ActivePlayer.MyInputList[0];
                    }
                }

                // Write them to the guess spots on the board.
                this.AIenterGuess(finalOutputGuess);
            }
        }

        private async void FindPossibleBullsOrCows()
        {
            int myBullsCount = 0;
            int guessCounter = 0;

            // Loop through every previous guess.
            foreach (string guess in this.gameControl.ActivePlayer.Guesses)
            {
                if (this.gameControl.ActivePlayer.MyInputList.Count() > 0)
                {
                    await this.UpdateBotMouth("Considering " + this.gameControl.ActivePlayer.MyInputList.Count() + " possibilities.");

                    List<AISpot> permSpots = new List<AISpot>();
                    List<AISpot> guessSpots = new List<AISpot>();
                    // For each guess, loop through every permutation in MyInputList.
                    foreach (string perm in this.gameControl.ActivePlayer.MyInputList)
                    {
                        // Compare the permutation to the guess.
                        // Turn each of the two strings into a list of AISpots.
                        

                        for (int i = 0; i < this.gameControl.CodeLength; i++)
                        {
                            AISpot tempPermSpot = new AISpot();
                            tempPermSpot.Name = "a" + i;
                            tempPermSpot.Value = perm.Substring(i, 1);
                            permSpots.Add(tempPermSpot);
                        }

                        for (int i = 0; i < this.gameControl.CodeLength; i++)
                        {
                            AISpot tempGuessSpot = new AISpot();
                            tempGuessSpot.Name = "b" + i;
                            tempGuessSpot.Value = guess.Substring(i, 1);
                            guessSpots.Add(tempGuessSpot);
                        }

                        myBullsCount = 0;

                        // Compare spots. A match means this character might be responsible for either a bull or a cow.
                        for (int k = 0; k < this.gameControl.CodeLength; k++)
                        {
                            if (guessSpots[k].Value == permSpots[k].Value)
                            {
                                myBullsCount++;
                                guessSpots[k].Value = string.Empty;
                                permSpots[k].Value = string.Empty;
                            }
                        }

                        // See if this permutation gets the same number of bulls as what we just had.
                        // If so, add it to the new OUTPUT list.
                        if (myBullsCount == Regex.Matches(this.gameControl.ActivePlayer.ListOfResults[guessCounter].Text, "🐂").Count)
                        {
                            this.gameControl.ActivePlayer.MyOutputList.Add(perm);
                        }
                        permSpots.Clear();
                        guessSpots.Clear();
                    } // end cycling through input list.
                }

                if (this.gameControl.ActivePlayer.MyOutputList.Count() > 0)
                {
                    // Assuming MyOutputList has multiple guess candidates inside, clear out the inputList 
                    // and put the output list into it. This way, next time it analyzes the previous turn
                    // it will be re-sorting a previously sorted list to further eliminate possiblities.
                    this.gameControl.ActivePlayer.MyInputList.Clear();
                    foreach (string myValue in this.gameControl.ActivePlayer.MyOutputList)
                    {
                        this.gameControl.ActivePlayer.MyInputList.Add(myValue);
                    }

                    this.gameControl.ActivePlayer.MyOutputList.Clear();
                }
                else
                {
                    MessageBox.Show("Congratulations! BessieBot's brain is broken in such a way \n that should not even be possible. \n");
                }

                guessCounter++;
            }
        }

        /// <summary>
        /// One of the AI's secondary logic algorithms. This one infers which characters in which spots
        /// might be responsible for a bull, by comparing similar guesses.
        /// </summary>
        /// <returns>A list of AISpots that most likely have bulls.</returns>
        private List<AISpot> AIinferredBulls()
        {
            // The AI needs to be able to look at the board, look at which spots have bulls, and then try to
            // deduce which spots they have in common that are probably responsible for those bulls.
            // Then, look at my list of candidates, and figure out which of those contain these spots in their proper positions.
            List<string> pastGuessesWith2Bulls = new List<string>();
            List<AISpot> unfilteredBullSpots = new List<AISpot>();
            List<AISpot> inferredBullSpots = new List<AISpot>();

            // Loop through the player's list of results labels.
            for (int i = 0; i < this.gameControl.ActivePlayer.ListOfResults.Count(); i++)
            {
                // If there are are more than two bulls on this label...
                // Look at the row next to this label. What's the corresponding guess in the player's guess list?
                // Add this guess to the new list.
                if (Regex.Matches(this.gameControl.ActivePlayer.ListOfResults[i].Text, "🐂").Count > 1)
                {
                    pastGuessesWith2Bulls.Add(this.gameControl.ActivePlayer.Guesses[i]);
                }
            }

            // If there are at least two guesses with at least two bulls, compare each guess to find similarities.
            if (pastGuessesWith2Bulls.Count() > 1)
            {
                Console.WriteLine("Found " + pastGuessesWith2Bulls.Count() + " past guesses with 2 bulls.");
                // For each member of this list...
                for (int i = 0; i < pastGuessesWith2Bulls.Count(); i++)
                {
                    // Compare it to all other members of this list...
                    for (int j = 0; j < pastGuessesWith2Bulls.Count(); j++)
                    {
                        // And see if they have any chars in common.
                        for (int k = 0; k < this.gameControl.CodeLength; k++)
                        {
                            if (pastGuessesWith2Bulls[i][k] == pastGuessesWith2Bulls[j][k])
                            {
                                // Note: It's okay (though not ideal) to include self-matches with those that match others in here.
                                AISpot bullSpot = new AISpot();
                                bullSpot.Name = k.ToString();
                                bullSpot.Value = pastGuessesWith2Bulls[i][k].ToString();

                                // Add them to the master list that will later be returned from this method.
                                unfilteredBullSpots.Add(bullSpot);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Found "+ unfilteredBullSpots.Count + " unfiltered bull spots.");

            foreach (AISpot i in unfilteredBullSpots)
            {
                int matchCounter = 0;
                foreach (AISpot j in unfilteredBullSpots)
                {
                    if (i.Name == j.Name && i.Value == j.Value)
                    {
                        matchCounter++;
                        if (matchCounter > 1)
                        {
                            // Console.WriteLine("Found a self match?");
                            inferredBullSpots.Add(i);
                        }
                    }
                }
            }
            Console.WriteLine("Permutations with inferred bull matches: " + inferredBullSpots.Count);
            return inferredBullSpots;
        }

        /// <summary>
        /// One of the AI's secondary logic algorithms. This one deduces which characters in which spots
        /// are not responsible for any bulls, by comparison, which can then be ruled out from future 
        /// consideration.
        /// </summary>
        /// <returns>A list of AISpots that do not have bulls.</returns>
        private List<AISpot> AIdeducedNoBulls()
        {
            /*
             * Just as we can infer the position of where a bull is located, we can rule out positions where
             * we are sure a bull is not. Sometimes the main AI is satisfied with cow after cow instead of
             * trying to get more bulls. This should curb that tendency.
             */

            List<string> pastGuessesWith2Cows = new List<string>();
            List<AISpot> preFilteredCows = new List<AISpot>();
            List<AISpot> deducedNoBullSpots = new List<AISpot>();

            // Loop through the player's list of results labels.
            for (int i = 0; i < this.gameControl.ActivePlayer.ListOfResults.Count(); i++)
            {
                if (Regex.Matches(this.gameControl.ActivePlayer.ListOfResults[i].Text, "🐄").Count > 1 &&
                    Regex.Matches(this.gameControl.ActivePlayer.ListOfResults[i].Text, "🐂").Count == 0)
                {
                    pastGuessesWith2Cows.Add(this.gameControl.ActivePlayer.Guesses[i]);
                }
            }

            List<AISpot> unfilteredCowSpots = new List<AISpot>();

            // If there are at least two guesses with at least two cows, compare each guess to find similarities.
            if (pastGuessesWith2Cows.Count() > 1)
            {
                Console.WriteLine("Found " + pastGuessesWith2Cows.Count + " past guesses with two cows and no bulls.");

                // For each member of this list...
                for (int i = 0; i < pastGuessesWith2Cows.Count(); i++)
                {
                    // Compare it to all other members of this list...
                    for (int j = 0; j < pastGuessesWith2Cows.Count(); j++)
                    {
                        // And see if they have any chars in common.
                        for (int k = 0; k < this.gameControl.CodeLength; k++)
                        {
                            if (pastGuessesWith2Cows[i][k] == pastGuessesWith2Cows[j][k])
                            {
                                AISpot noBullSpot = new AISpot();
                                noBullSpot.Name = k.ToString();
                                noBullSpot.Value = pastGuessesWith2Cows[i][k].ToString();

                                unfilteredCowSpots.Add(noBullSpot);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Found " + unfilteredCowSpots.Count + " unfilted cow spots.");

            foreach (AISpot i in unfilteredCowSpots)
            {
                int matchCounter = 0;
                foreach (AISpot j in unfilteredCowSpots)
                {
                    if (i.Name == j.Name && i.Value == j.Value)
                    {
                        matchCounter++;
                        if (matchCounter > 1)
                        {
                            deducedNoBullSpots.Add(i);
                        }
                    }
                }
            }

            Console.WriteLine("Permutations that match deduction of where bulls are not: " + deducedNoBullSpots.Count());
            return deducedNoBullSpots;
        }

        private void BullSense(List<AISpot> whereDaBullsAt)
        {
            // Now look at each previous guess, and try to determine which symbol is responsible for the bulls.
            if (whereDaBullsAt.Count() > 0)
            {
                // Make a new list for storing the permutations that match our inferred bull locations.
                List<string> candidates = new List<string>();

                // For each permutation in the input list...
                for (int i = 0; i < this.gameControl.ActivePlayer.MyInputList.Count(); i++)
                {
                    for (int j = 0; j < this.gameControl.CodeLength; j++)
                    {
                        foreach (AISpot bullSpot in whereDaBullsAt)
                        {
                            if (this.gameControl.ActivePlayer.MyInputList[i] == bullSpot.Name && (this.gameControl.ActivePlayer.MyInputList[i][j].ToString() == bullSpot.Value))
                            {
                                candidates.Add(this.gameControl.ActivePlayer.MyInputList[i]);
                            }
                        }
                    }
                }

                if (candidates.Count > 0)
                {
                    // Now let's remove everything but those that match our inferred bull locations.
                    this.gameControl.ActivePlayer.MyInputList.Clear();
                    foreach (string candidate in candidates)
                    {
                        this.gameControl.ActivePlayer.MyInputList.Add(candidate);
                    }
                }
            }
        }

        private void CowSense(List<AISpot> whereDaBullsAint)
        {
            if (whereDaBullsAint.Count() > 0)
            {
                // Make a new list for storing the permutations that match our deduced NOBULLS spots.
                List<string> rejects = new List<string>();

                // For each permutation in the input list...
                for (int i = 0; i < this.gameControl.ActivePlayer.MyInputList.Count(); i++)
                {
                    for (int j = 0; j < this.gameControl.CodeLength; j++)
                    {
                        foreach (AISpot noBullSpot in whereDaBullsAint)
                        {
                            if (this.gameControl.ActivePlayer.MyInputList[i] == noBullSpot.Name && (this.gameControl.ActivePlayer.MyInputList[i][j].ToString() == noBullSpot.Value))
                            {
                                rejects.Add(this.gameControl.ActivePlayer.MyInputList[i]);
                            }
                        }
                    }
                }

                if (rejects.Count > 0)
                {
                    // Now let's remove all of our noBull spot rejects from MyOutputList.
                    List<string> sanitization = new List<string>();

                    foreach (string reject in rejects)
                    {
                        for (int i = 0; i < this.gameControl.ActivePlayer.MyInputList.Count(); i++)
                        {
                            if (this.gameControl.ActivePlayer.MyInputList[i] == reject)
                            {
                                sanitization.Add(this.gameControl.ActivePlayer.MyInputList[i]);
                            }
                        }
                    }

                    foreach (string s in sanitization)
                    {
                        this.gameControl.ActivePlayer.MyInputList.Remove(s);
                    }
                }
            }
        }

        /// <summary>
        /// An asynchronous Task for removing used guesses from MyInputList.
        /// </summary>
        /// <param name="myInputList">The player's input list.</param>
        /// <returns>An asynchronous task.</returns>
        private async Task RemoveUsedGuessesFromPermuations(List<string> myInputList)
        {
            List<string> toBeRemoved = new List<string>();
            for (int i = 0; i < myInputList.Count(); i++)
            {
                foreach (string guess in this.gameControl.ActivePlayer.Guesses)
                {
                    if (myInputList[i] == guess)
                    {
                        toBeRemoved.Add(guess);
                    }
                }
            }

            foreach (string tbr in toBeRemoved)
            {
                myInputList.Remove(tbr);
            }

            await Task.Delay(0);
        }

        /// <summary>
        /// After thinking really hard about it, AI is now ready to enter a guess.
        /// </summary>
        /// <param name="finalOutputGuess">A string.</param>
        private void AIenterGuess(string finalOutputGuess)
        {
            if (this.GameOver == false)
            {
                for (int i = 0; i < this.gameControl.CodeLength; i++)
                {
                    if (this.gameControl.ActivePlayer == this.gameControl.P1)
                    {
                        this.attemptsArrayP1[i, this.thisTurn].Text = finalOutputGuess[i].ToString();
                        this.attemptsArrayP1[i, this.thisTurn].Invalidate();
                        this.attemptsArrayP1[i, this.thisTurn].Update();
                    }
                    else
                    {
                        this.thisTurn = this.GameControl.ActivePlayer.AttemptsUsed;
                        this.attemptsArrayP2[i, this.thisTurn].Text = finalOutputGuess[i].ToString();
                        this.attemptsArrayP2[i, this.thisTurn].Invalidate();
                        this.attemptsArrayP2[i, this.thisTurn].Update();
                    }
                }
            }
        }

        /// <summary>
        /// Draw a window where the AI may display some status updates.
        /// </summary>
        private void MakeBotMouth()
        {
            /*
             * Sometimes, the bot has to think long and hard about its next move. At code lengths of 6 or more, this
             * is especially true. Instead of appearing to be unresponsive, output some simple messages until done.
             */

            this.botMouth = new Label();
            this.botMouth.Location = new Point(this.SquareSize * this.gameControl.CodeLength * 2, 0);
            this.botMouth.BorderStyle = BorderStyle.FixedSingle;
            this.botMouth.BackColor = Color.DarkSlateGray;
            this.botMouth.ForeColor = Color.Orange;
            this.botMouth.Width = ((this.SquareSize * this.gameControl.CodeLength) * 2) - this.SquareSize;
            this.botMouth.Height = this.SquareSize * 5 / 8;
            this.botMouth.Font = new Font("Consolas", this.SquareSize * 1 / 4, FontStyle.Bold);
            this.botMouth.TextAlign = ContentAlignment.MiddleCenter;
            this.botMouth.Text = "BessieBot says 'moo.'";

            this.Controls.Add(this.botMouth);
        }

        /// <summary>
        /// Update the botMouth to display a new message.
        /// </summary>
        /// <param name="sayWhat">A passed-in string.</param>
        /// <returns>An asynchronous task.</returns>
        private async Task UpdateBotMouth(string sayWhat)
        {
            this.botMouth.Text = sayWhat;
            this.botMouth.Invalidate();
            this.botMouth.Update();
            await Task.Delay(0);
        }
    }
}
