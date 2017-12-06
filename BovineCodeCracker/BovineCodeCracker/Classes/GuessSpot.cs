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
// <copyright file="GuessSpot.cs" company="Chris Menning">                                            --
//     © Chris Menning 2017                                                                           --
// </copyright>                                                                                       --
//------------------------------------------------------------------------------------------------------

namespace BovineCodeCracker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Media;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The GuessSpot is a special kind of TextBox.
    /// </summary>
    public class GuessSpot : TextBox
    {
        /// <summary>
        /// A reference to the GameController form.
        /// </summary>
        private GameController gameController;

        /// <summary>
        /// A reference to the GameBoard form.
        /// </summary>
        private GameBoard gameBoard;

        /// <summary>
        /// An integer for storing this GuessSpot's column.
        /// </summary>
        private int col;

        /// <summary>
        /// An integer for storing this GuessSpot's row.
        /// </summary>
        private int row;

        /// <summary>
        /// A bool used as a condition for preventing multiple GuessSpots from triggering other methods at once.
        /// </summary>
        private bool insuranceOn;

        /// <summary>
        /// The "Confirm" sound effect.
        /// </summary>
        private SoundPlayer soundConfirm = new SoundPlayer(BovineCodeCracker.Properties.Resources._387216__spiceprogram__pock_1);

        /// <summary>
        /// Initializes a new instance of the GuessSpot class.
        /// </summary>
        /// <param name="gameControl">A passed-in reference to the GameControl form.</param>
        /// <param name="gameBo">A passed-in reference to the GameBoard form.</param>
        public GuessSpot(GameController gameControl, GameBoard gameBo)
        {
            this.gameController = gameControl;
            this.gameBoard = gameBo;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.TextAlign = HorizontalAlignment.Center;
            this.MaxLength = 1;
            this.Text = "[ ]";
            string lastText = "[ ]";
            this.Font = new Font(this.Font.FontFamily, this.gameBoard.SquareSize * 5 / 8, FontStyle.Regular);

            this.GotFocus += (r, y) =>
            {
                if (lastText != "[ ]")
                {
                    this.Text = string.Empty;
                }
            };

            this.Click += (s, z) =>
            {
                if (this.row == gameControl.ActivePlayer.AttemptsUsed)
                {
                    this.Text = gameControl.ActivePlayer.SelectedPicker.Text;
                    soundConfirm.Play();
                }
            };

            this.TextChanged += (s, z) =>
            {
                this.Invalidate();
                this.Update();

                // Only increment spotsUsed if this is the first time this text has changed.
                if (lastText == "[ ]")
                {
                    insuranceOn = false;
                    gameBoard.SpotsUsed++;
                }

                int tempInt;

                bool result = int.TryParse(this.Text.ToString(), out tempInt);
                if (result == true)
                {
                    soundConfirm.Play();
                    lastText = result.ToString(); // Make sure Last Text keeps this from triggering again after conversion.

                    string emoji = string.Empty;

                    switch (tempInt)
                    {
                        case 0:
                            emoji = "☀";
                            break;
                        case 1:
                            emoji = "☾";
                            break;
                        case 2:
                            emoji = "✯";
                            break;
                        case 3:
                            emoji = "⚓";
                            break;
                        case 4:
                            emoji = "❄";
                            break;
                        case 5:
                            emoji = "⚕";
                            break;
                        case 6:
                            emoji = "☘";
                            break;
                        case 7:
                            emoji = "☠";
                            break;
                        case 8:
                            emoji = "⚖";
                            break;
                        case 9:
                            emoji = "♔";
                            break;
                    }

                    this.Text = emoji;

                    foreach (Picker p in gameBoard.Pickers)
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

                    if (gameControl.ActivePlayer.IsHuman == true && gameBo.SpotsUsed > 0)
                    {
                        SendKeys.Send("{TAB}");
                    }
                }

                lastText = this.Text;

                if (gameBoard.SpotsUsed == gameControl.CodeLength && gameControl.ActivePlayer.IsHuman == true && insuranceOn == false && gameBoard.GameOver == false)
                {
                    gameBoard.ScoreTheGuess(this.Row);
                    insuranceOn = true;
                    return;
                }
                else if (gameBoard.SpotsUsed == gameControl.CodeLength && gameControl.ActivePlayer.IsHuman == false)
                {                    
                    return;
                }
            };
        }

        /// <summary>
        /// Gets or sets the column field.
        /// </summary>
        public int Col
        {
            get
            {
                return this.col;
            }

            set
            {
                this.col = value;
            }
        }

        /// <summary>
        /// Gets or sets the row field.
        /// </summary>
        public int Row
        {
            get
            {
                return this.row;
            }

            set
            {
                this.row = value;
            }
        }
    }
}
