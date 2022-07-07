using System.Collections.Generic;
using System.Drawing;

namespace JapaneseGame
{
    class Boat
    {
        public readonly List<Person> peopleOnBoat;
        public bool IsSend { get; private set; } = false;
        public readonly Point startLocation = new Point(375, 345); 
        public readonly Point finishLocation = new Point(705, 208);

        public Boat()
        {
            peopleOnBoat = new List<Person>();
        }
        public void AddPerson(Person person)
        {
            peopleOnBoat.Add(person);

            person.IsOnBoat = true;

            if (IsSend)
            {
                peopleOnBoat[0].pbPerson.Location = new Point(finishLocation.X, finishLocation.Y);
                if (peopleOnBoat.Count != 1)
                    peopleOnBoat[1].pbPerson.Location = new Point(finishLocation.X + 60, finishLocation.Y);
            }
            else {

            peopleOnBoat[0].pbPerson.Location = new Point(startLocation.X, startLocation.Y);
            if (peopleOnBoat.Count != 1)
                peopleOnBoat[1].pbPerson.Location = new Point(startLocation.X + 95, startLocation.Y);
            }
        }
        public void RemovePerson(Person person)
        {
            peopleOnBoat.Remove(person);

            person.IsOnBoat = false;

            person.pbPerson.Location = IsSend ? person.finishLocation : person.startLocation;

        }
        public void SendBoat()
        {
            IsSend = !IsSend;
            if (IsSend)
            {
                peopleOnBoat[0].pbPerson.Location = new Point(finishLocation.X, finishLocation.Y);
                if (peopleOnBoat.Count != 1)
                    peopleOnBoat[1].pbPerson.Location = new Point(finishLocation.X + 60, finishLocation.Y);
                for (int i = 0; i < peopleOnBoat.Count; i++)
                    peopleOnBoat[i].IsArrived = true;
            }
            else
            {
                peopleOnBoat[0].pbPerson.Location = new Point(startLocation.X, startLocation.Y);
                if (peopleOnBoat.Count != 1)

                    peopleOnBoat[1].pbPerson.Location = new Point(startLocation.X + 95, startLocation.Y);
                for (int i = 0; i < peopleOnBoat.Count; i++)
                    peopleOnBoat[i].IsArrived = false;
            }
        }
    }
}
