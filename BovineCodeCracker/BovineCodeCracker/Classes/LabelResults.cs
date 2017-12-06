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
// <copyright file="LabelResults.cs" company="Chris Menning">                                          --
//     © Chris Menning 2017                                                                           --
// </copyright>                                                                                       --
//------------------------------------------------------------------------------------------------------

namespace BovineCodeCracker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// A LabelResults object is a special kind of label for reporting out the result of a guess.
    /// </summary>
    public class LabelResults : Label
    {
        /// <summary>
        /// A reference to the GameBoard form in which this label appears.
        /// </summary>
        private GameBoard gameBoard;

        /// <summary>
        /// Initializes a new instance of the LabelResults class.
        /// </summary>
        /// <param name="gameBo">A passed-in reference to the GameBoard.</param>
        public LabelResults(GameBoard gameBo)
        {
            this.gameBoard = gameBo;
            this.Text = "\n" + "...";
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.AliceBlue;
            this.Font = new Font("Consolas", this.gameBoard.SquareSize * 1 / 3, FontStyle.Regular);
            this.Width = (this.gameBoard.SquareSize * this.gameBoard.GameControl.CodeLength) - (this.gameBoard.SquareSize * 1 / 3);
            this.Height = this.gameBoard.SquareSize;
            this.AutoSize = false;
            this.TextAlign = ContentAlignment.MiddleLeft;
        }
    }
}
