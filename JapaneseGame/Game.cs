using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JapaneseGame
{
    class Game
    {
        readonly List<Person> people;
        readonly List<PictureBox> pbPeople;

        readonly Boat boat;

        public Game(List<PictureBox> pbPeople)
        {
            this.pbPeople = pbPeople;

            people = new List<Person>();

            List<Role> members = new List<Role>()
            {
                Role.son,
                Role.son,
                Role.daughter,
                Role.daughter,
                Role.mother,
                Role.father,
                Role.policeman,
                Role.thief,
            };

            List<Point> startPositions = new List<Point>()
            {
                new Point(700, 532), //boy1
                new Point(404, 532), //boy2
                new Point(5, 380), //girl1
                new Point(80, 380), //girl2 
                new Point(644, 488), //mom
                new Point(177, 357), //dad
                new Point(121, 488), //cop
                new Point(12, 488), //thief
            };
            List<Point> finishPositions = new List<Point>()
            {
                new Point(445, 70), //boy1
                new Point(295, 70), //boy2
                new Point(859, 145), //girl1
                new Point(934, 145), //girl2 
                new Point(1009, 110), //mom
                new Point(370, 35), //dad
                new Point(778, 35), //cop
                new Point(703, 35), //thief
            };

            for (int i = 0; i < pbPeople.Count; i++)
            {
                people.Add(new Person(members[i], startPositions[i], finishPositions[i], pbPeople[i]));
            }

            boat = new Boat();
        }
        public void SendPersonFromBoat(Person person, int index)
        {
            pbPeople[index].Location = (boat.IsSend) ? person.finishLocation : person.startLocation;
        }
        public void CheckRules() // problem - thief 
        {
            if (boat.peopleOnBoat.Count == 0) throw new Exception("There's no people on the boat!");
            if (boat.peopleOnBoat.Count == 1)
            {
                Person personOnBoat = boat.peopleOnBoat[0];

                //if one kid's on the boat
                if (personOnBoat.role == Role.daughter || personOnBoat.role == Role.son) throw new Exception("Kids can't be alone on the boat!");

                //if father leaves and mother stays with sons - throw Ex
                if ((personOnBoat.role == Role.father))
                {
                    if (FindPersonByRole(Role.son, boat.IsSend) && FindPersonByRole(Role.mother, boat.IsSend)) throw new Exception("You left kids with their mommy!");

                    if (FindPersonByRole(Role.daughter, !boat.IsSend) && FindPersonByRole(Role.mother, boat.IsSend)) throw new Exception("No!");

                }
                if (personOnBoat.role == Role.mother)
                {
                    if (FindPersonByRole(Role.daughter, boat.IsSend) && FindPersonByRole(Role.father, boat.IsSend)) throw new Exception("You left kids with their daddy!");

                    if (FindPersonByRole(Role.son, !boat.IsSend) && FindPersonByRole(Role.father, boat.IsSend)) throw new Exception("No!");
                }

                if ((personOnBoat.role == Role.thief)) throw new Exception("Thief can't move without Cop");
                else if (personOnBoat.role == Role.policeman && (FindPersonByRole(Role.thief, boat.IsSend))) throw new Exception("thief can't be without Cop");

            }
            else if (boat.peopleOnBoat.Count == 2)
            {
                Person personOnBoat1 = boat.peopleOnBoat[0];
                Person personOnBoat2 = boat.peopleOnBoat[1];

                Role personRole1 = boat.peopleOnBoat[0].role;
                Role personRole2 = boat.peopleOnBoat[1].role;

                //if kids alone
                if ((personOnBoat1.role == Role.daughter || personOnBoat1.role == Role.son) && (personOnBoat2.role == Role.son || personOnBoat2.role == Role.daughter)) throw new Exception("Kids can't be alone on the boat!");

                //if dad and mother together - okay, end this method
                else if ((personRole1 == Role.father || personRole2 == Role.father) && (personRole1 == Role.mother || personRole2 == Role.mother)) return;
                
                //if mother stays with sons without dad
                else if ((personOnBoat1.role == Role.father || personOnBoat2.role == Role.father) && FindPersonByRole(Role.mother, boat.IsSend) && FindPersonByRole(Role.son, boat.IsSend)) throw new Exception("You left sons with their mommy!");
                //if father stays with daughters without mom
                else if ((personOnBoat1.role == Role.mother || personOnBoat2.role == Role.mother) && FindPersonByRole(Role.father, boat.IsSend) && FindPersonByRole(Role.daughter, boat.IsSend)) throw new Exception("You left daughters with their daddy!");

                //if mother leaves for sons
                else if ((personOnBoat1.role == Role.mother || personOnBoat2.role == Role.mother) && FindPersonByRole(Role.father, boat.IsSend) && FindPersonByRole(Role.son, !boat.IsSend)) throw new Exception("God, please, no");
                //if father leaves for daughters
                else if ((personOnBoat1.role == Role.father || personOnBoat2.role == Role.father) && FindPersonByRole(Role.mother, boat.IsSend) && FindPersonByRole(Role.daughter, !boat.IsSend)) throw new Exception("God, please, no");

                //if thief without cop
                if ((personOnBoat1.role == Role.thief || personOnBoat2.role == Role.thief) && (personOnBoat1.role == Role.policeman || personOnBoat2.role == Role.policeman)) return;
                else if ((personOnBoat1.role == Role.policeman || personOnBoat2.role == Role.policeman) && (FindPersonByRole(Role.thief, !boat.IsSend))) return;
                else throw new Exception("Thief can't move without Cop");

            }
        }
        private bool FindPersonByRole(Role role, bool isArrived)
        {
            for (int i = 0; i < people.Count; i++)
            {
                if (people[i].role == role && people[i].IsArrived == isArrived && !people[i].IsOnBoat)
                    return true;
            }
            return false;
        }
        public void Send(PictureBox pbPerson) // when clicking on a person
        {
            int index = -1;
            for (int i = 0; i < people.Count; i++)
            {
                if (pbPerson == pbPeople[i])
                {
                    index = i;
                    break;
                }
            }

            if (index == -1) return;

            Person person = people[index];

            for (int i = 0; i < boat.peopleOnBoat.Count; i++) // delete person from boat
            {
                if (boat.peopleOnBoat[i] == person)
                {
                    boat.RemovePerson(person);
                    return;
                }
            }
            if (boat.peopleOnBoat.Count < 2 && boat.IsSend == person.IsArrived) //add person on boat
                boat.AddPerson(person);
        }
        public bool SendBoat()
        {
            boat.SendBoat();

            return boat.IsSend;
        }
    }
}
