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
// <copyright file="Picker.cs" company="Chris Menning">                                               --
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
    using System.Windows.Input;

    /// <summary>
    /// The Picker is a special kind of label used as a symbol-selector tool.
    /// </summary>
    public class Picker : Label
    {
        /// <summary>
        /// A reference to the GameBoard form.
        /// </summary>
        private GameBoard gb;

        /// <summary>
        /// A reference to the GameController form.
        /// </summary>
        private GameController gc;

        /// <summary>
        /// A reference to the Code Entry form.
        /// </summary>
        private CodeEnterScreen ces;

        /// <summary>
        /// An integer for storing this Picker's ID number.
        /// </summary>
        private int id;

        /// <summary>
        /// A color that stores whatever the picker's first color was, so that highlights can be disabled.
        /// </summary>
        private Color initialColor;

        /// <summary>
        /// The sound that selecting a picker makes.
        /// </summary>
        private SoundPlayer soundPick = new SoundPlayer(BovineCodeCracker.Properties.Resources.spiceprogram__pock_higher);

        /// <summary>
        /// Initializes a new instance of the Picker class for use on the GameBoard form.
        /// </summary>
        /// <param name="gameBoard">A reference to the GameBoard form.</param>
        /// <param name="gameControl">A reference to the GameControl form.</param>
        /// <param name="id">A passed-in ID to be assigned to this picker.</param>
        public Picker(GameBoard gameBoard, GameController gameControl, int id)
        {
            this.gb = gameBoard;
            this.gc = gameControl;
            this.Id = id;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font(this.Font.FontFamily, this.gb.SquareSize * 3 / 8, FontStyle.Regular);
            this.InitialColor = this.BackColor;

            Label keylabel = new Label();
            keylabel.Location = new Point(this.Location.X, this.Location.Y);
            keylabel.Text = id.ToString();
            keylabel.Font = new Font(this.Font.FontFamily, 6, FontStyle.Regular);
            keylabel.Width = 10;
            keylabel.Height = 10;

            this.Controls.Add(keylabel);

            this.MouseHover += (s, z) =>
            {
                this.BackColor = Color.Aquamarine;
            };

            this.MouseLeave += (s, z) =>
            {
                if (gc.ActivePlayer.SelectedPicker != this)
                {
                    this.BackColor = InitialColor;
                }
            };

            this.Click += (s, z) =>
            {
                gameControl.ActivePlayer.SelectedPicker = this;
                this.BackColor = Color.Cyan;
                soundPick.Play();

                foreach (Picker p in gb.Pickers)
                {
                    if (p != this)
                    {
                        p.BackColor = this.InitialColor;
                    }
                }
            };
        }

        /// <summary>
        /// Initializes a new instance of the Picker class for use on the CodeEntry form.
        /// </summary>
        /// <param name="ces">A reference to the CodeEntry form.</param>
        /// <param name="gameControl">A reference to the GameControl form.</param>
        /// <param name="id">A passed-in value to be assigned as this Picker's ID.</param>
        public Picker(CodeEnterScreen ces, GameController gameControl, int id)
        {
            this.ces = ces;
            this.gc = gameControl;
            this.Id = id;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Font = new Font(this.Font.FontFamily, 50 * 3 / 8, FontStyle.Regular);
            this.InitialColor = this.BackColor;

            Label keylabel = new Label();
            keylabel.Location = new Point(this.Location.X, this.Location.Y);
            keylabel.Text = id.ToString();
            keylabel.Font = new Font(this.Font.FontFamily, 6, FontStyle.Regular);
            keylabel.Width = 10;
            keylabel.Height = 10;

            this.Controls.Add(keylabel);

            this.MouseHover += (s, z) =>
            {
                this.BackColor = Color.Aquamarine;
            };

            this.MouseLeave += (s, z) =>
            {
                if (gc.ActivePlayer.SelectedPicker != this)
                {
                    this.BackColor = InitialColor;
                }
            };

            this.Click += (s, z) =>
            {
                gameControl.ActivePlayer.SelectedPicker = this;
                this.BackColor = Color.Cyan;
                soundPick.Play();

                foreach (Picker p in ces.Pickers)
                {
                    if (p != this)
                    {
                        p.BackColor = this.InitialColor;
                    }
                }
            };
        }

        /// <summary>
        /// Gets or sets the id field.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets the initialColor field.
        /// </summary>
        public Color InitialColor
        {
            get
            {
                return this.initialColor;
            }

            set
            {
                this.initialColor = value;
            }
        }
    }
}
