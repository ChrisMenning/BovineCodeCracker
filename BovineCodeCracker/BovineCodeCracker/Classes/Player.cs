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
// <copyright file="Player.cs" company="Chris Menning">                                               --
//     © Chris Menning 2017                                                                           --
// </copyright>                                                                                       --
//------------------------------------------------------------------------------------------------------

namespace BovineCodeCracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The Player class, whether human or AI.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// A string for storing the player's name.
        /// </summary>
        private string name;

        /// <summary>
        /// A bool for storing whether or not the player is human.
        /// </summary>
        private bool isHuman;

        /// <summary>
        /// A string for storing the player's secret code.
        /// </summary>
        private List<char> secretCode;

        /// <summary>
        /// A string for storing the current guess.
        /// </summary>
        private string currentGuess = string.Empty;

        /// <summary>
        /// An integer for storing the number of attempts used.
        /// </summary>
        private int attemptsUsed;

        /// <summary>
        /// A list of strings for storing all this player's guesses.
        /// </summary>
        private List<string> guesses;

        /// <summary>
        /// A list of labels for storing all results of all this player's guesses.
        /// </summary>
        private List<Label> listOfResults;

        /// <summary>
        /// A Picker for storing whichever picker is selected at any given moment.
        /// </summary>
        private Picker selectedPicker;

        /// <summary>
        /// A list of strings where the AI stores permutations of possible solutions prior to filtering and consideration.
        /// </summary>
        private List<string> myInputList = new List<string>();  // Where the AI stores permutations of possible moves prior to consideration.

        /// <summary>
        /// A list of strings where the AI stores permutations after some consideration and filtering.
        /// </summary>
        private List<string> myOutputList = new List<string>();

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        /// <param name="n">A passed-in value assigned to the player's name.</param>
        /// <param name="ih">A passed-in value assigned to the player's isHuman field.</param>
        public Player(string n, bool ih)
        {
            this.Name = n;
            this.IsHuman = ih;
            this.SecretCode = new List<char>();
            this.AttemptsUsed = 0;
            this.Guesses = new List<string>();
            this.ListOfResults = new List<Label>();
        }

        /// <summary>
        /// Gets or sets the name field.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the attemptsUsed field.
        /// </summary>
        public int AttemptsUsed
        {
            get
            {
                return this.attemptsUsed;
            }

            set
            {
                this.attemptsUsed = value;
            }
        }

        /// <summary>
        /// Gets or sets the guesses field.
        /// </summary>
        public List<string> Guesses
        {
            get
            {
                return this.guesses;
            }

            set
            {
                this.guesses = value;
            }
        }

        /// <summary>
        /// Gets or sets the listOfResults field.
        /// </summary>
        public List<Label> ListOfResults
        {
            get
            {
                return this.listOfResults;
            }

            set
            {
                this.listOfResults = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the player is human.
        /// </summary>
        public bool IsHuman
        {
            get
            {
                return this.isHuman;
            }

            set
            {
                this.isHuman = value;
            }
        }

        /// <summary>
        /// Gets or sets the secretCode field.
        /// </summary>
        public List<char> SecretCode
        {
            get
            {
                return this.secretCode;
            }

            set
            {
                this.secretCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the currentGuess field.
        /// </summary>
        public string CurrentGuess
        {
            get
            {
                return this.currentGuess;
            }

            set
            {
                this.currentGuess = value;
            }
        }

        /// <summary>
        /// Gets or sets the selectedPicker field.
        /// </summary>
        public Picker SelectedPicker
        {
            get
            {
                return this.selectedPicker;
            }

            set
            {
                this.selectedPicker = value;
            }
        }

        /// <summary>
        /// Gets or sets the myInputList field.
        /// </summary>
        public List<string> MyInputList
        {
            get
            {
                return this.myInputList;
            }

            set
            {
                this.myInputList = value;
            }
        }

        /// <summary>
        /// Gets or sets the myOutputList field.
        /// </summary>
        public List<string> MyOutputList
        {
            get
            {
                return this.myOutputList;
            }

            set
            {
                this.myOutputList = value;
            }
        }
    }
}
