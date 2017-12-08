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
// <copyright file="CodeEnterScreen.Designer.cs" company="Chris Menning">                                               --
//     © Chris Menning 2017                                                                           --
// </copyright>                                                                                       --
//------------------------------------------------------------------------------------------------------

namespace BovineCodeCracker
{
    /// <summary>
    /// The designer for the CodeEnterScreen form.
    /// </summary>
    public partial class CodeEnterScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.Label labelMessage;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.Button buttonSubmit;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.Button buttonGenerateCode;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.buttonGenerateCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelMessage.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.ForeColor = System.Drawing.Color.White;
            this.labelMessage.Location = new System.Drawing.Point(25, 62);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(456, 56);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "labelMessage";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonSubmit.Enabled = false;
            this.buttonSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSubmit.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSubmit.ForeColor = System.Drawing.Color.Gold;
            this.buttonSubmit.Location = new System.Drawing.Point(376, 174);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(105, 58);
            this.buttonSubmit.TabIndex = 2;
            this.buttonSubmit.Text = "&Submit";
            this.buttonSubmit.UseVisualStyleBackColor = false;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonGenerateCode
            // 
            this.buttonGenerateCode.BackColor = System.Drawing.Color.DarkSlateGray;
            this.buttonGenerateCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerateCode.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGenerateCode.ForeColor = System.Drawing.Color.White;
            this.buttonGenerateCode.Location = new System.Drawing.Point(28, 121);
            this.buttonGenerateCode.Name = "buttonGenerateCode";
            this.buttonGenerateCode.Size = new System.Drawing.Size(453, 47);
            this.buttonGenerateCode.TabIndex = 3;
            this.buttonGenerateCode.Text = "Generate Code For Me";
            this.buttonGenerateCode.UseVisualStyleBackColor = false;
            this.buttonGenerateCode.Click += new System.EventHandler(this.buttonGenerateCode_Click);
            // 
            // CodeEnterScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BovineCodeCracker.Properties.Resources.wood_texture_red_grain_wooden_panel_design_wallpaper_800;
            this.ClientSize = new System.Drawing.Size(510, 268);
            this.Controls.Add(this.buttonGenerateCode);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "CodeEnterScreen";
            this.Text = "CodeEnterScreen";
            this.ResumeLayout(false);

        }

        #endregion
    }
}