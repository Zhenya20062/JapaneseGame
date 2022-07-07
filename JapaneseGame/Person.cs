using System.Drawing;
using System.Windows.Forms;

namespace JapaneseGame
{
    class Person
    {
        public readonly Role role;
        public bool IsArrived { get; set; } = false;
        public bool IsOnBoat { get; set; } = false;
        public readonly Point startLocation;
        public readonly Point finishLocation;
        public PictureBox pbPerson;

        public Person(Role role, Point startPosition, Point finishLocation, PictureBox pbPerson)
        {
            this.pbPerson = pbPerson;
            this.role = role;
            startLocation = startPosition;
            this.finishLocation = finishLocation;
        }
    }
}
