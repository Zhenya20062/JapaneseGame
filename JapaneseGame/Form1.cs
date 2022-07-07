using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JapaneseGame
{
    enum Role
    {
        father,
        mother,
        son,
        daughter,
        thief,
        policeman
    }
    public partial class Form1 : Form
    {
        readonly Game game;
        public Form1()
        {
            InitializeComponent();

            List<PictureBox> pbPeople = new List<PictureBox>()
            {
                pbBoy1, pbBoy2, pbGirl1, pbGirl2, pbMom, pbDad, pbCop, pbThief
            };
            game = new Game(pbPeople);
        }

        private void PbPerson_Click(object sender, EventArgs e)
        {
            game.Send(sender as PictureBox);
        }

        private void pbLever_Click(object sender, EventArgs e)
        {
            try
            {
                game.CheckRules();

                if (game.SendBoat()) BackgroundImage = Properties.Resources.anFullMap;
                else BackgroundImage = Properties.Resources.background;
            }
            catch (Exception ex) { MessageBox.Show($"{ex}", "Error", MessageBoxButtons.OK); };

        }
    }
}
