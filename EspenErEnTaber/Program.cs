

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gambling

{

    internal class Program

    {

        static public List<Card> Cards = new List<Card>();

        static void Main(string[] args)

        {
            Console.Clear();

            Intro();
            System.Console.WriteLine(GetPlayer());








        }
        static void DrawNewCard(Player player)
        {
            Random random = new Random();
            int cardNumber = random.Next(0, Cards.Count);


            GiveCardTo(cardNumber, player);
            RemoveCard(cardNumber);


        }
        static void GiveCardTo(int cardArrayNumber, Player player)
        {
            player.PlayerCards.Add(Cards[cardArrayNumber]);
        }
        static void RemoveCard(int cardArrayNumber)
        {
            Cards.Remove(Cards[cardArrayNumber]);
        }
        static void Intro()
        {
            System.Console.WriteLine("WELCOME TO THE GRAND CARD CASINO");
            System.Console.WriteLine("Here you can play Poker and Blackjack");

            bool gettingInput = true;
            while (gettingInput)
            {
                System.Console.WriteLine("What a we playing?");
                switch (Console.ReadLine())
                {
                    case "poker":
                        gettingInput = false;

                        break;
                    case "Poker":
                        gettingInput = false;

                        break;
                    case "p":
                        gettingInput = false;

                        break;
                    case "P":
                        gettingInput = false;

                        break;
                    case "BJ" or "Blackjack" or "b":
                        PlayingBlackJack();
                        break;





                    default:
                        Console.WriteLine("Try again");
                        break;

                }
            }


        }

        static int GetPlayer()
        {
            System.Console.WriteLine("How many players are playing");
            System.Console.WriteLine("We recommend between 1-4");
            System.Console.WriteLine("If left blank playercound will be set to 1");
            string tempstring = Console.ReadLine();
            switch (tempstring)
            {
                case "2":
                    return 2;

                case "3":
                    return 3;

                case "4":
                    return 4;


                default:
                    return 1;


            }

        }



        static void PlayingBlackJack()
        {
            Cards.Clear();
            LoadCardValues();
            int Players = GetPlayer();
            List<Player> PlayerArray = new List<Player>();

            for (int i = 0; i <= Players; i++)
            {
                PlayerArray.Add(new Player());
            }
            System.Console.WriteLine(PlayerArray.Count);
            System.Console.WriteLine("p3");

            while (true)
            {

                for (int i = 1; i <= PlayerArray.Count - 1; i++)
                {
                    if (PlayerArray[i].PlayerCards.Count < 2)
                    {
                        DrawNewCard(PlayerArray[i]);
                        System.Console.WriteLine("Player {0} drawed a card", i);
                    }
                    else
                    {
                        bool playerturn = true;
                        while (playerturn)
                        {

                            Console.Clear();
                            System.Console.WriteLine("Player {0} currently has these cards", i);
                            PlayerArray[i].WriteCards();
                            System.Console.WriteLine("Total card values are currently {0}", PlayerArray[i].GetTotalCards());
                            System.Console.WriteLine("Draw y/n");
                            switch (Console.ReadLine().ToLower())
                            {
                                case "y" or "yes":
                                    DrawNewCard(PlayerArray[i]);

                                    break;
                                case "n" or "no":
                                    playerturn = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }

                Console.ReadLine();
            }


        }
        static void PlayingPoker()
        {

        }





























        static void LoadCardValues()
        {
            for (int i = 0; i < 52; i++)
            {
                Cards.Add(new Card());
            }
            Cards[0].Drawed = false; Cards[0].Name = "ACE"; Cards[0].Value = 11; Cards[0].Color = 1;
            Cards[1].Drawed = false; Cards[1].Name = "ACE"; Cards[1].Value = 11; Cards[1].Color = 2;
            Cards[2].Drawed = false; Cards[2].Name = "ACE"; Cards[2].Value = 11; Cards[2].Color = 3;
            Cards[3].Drawed = false; Cards[3].Name = "ACE"; Cards[3].Value = 11; Cards[3].Color = 4;
            Cards[4].Drawed = false; Cards[4].Name = "TWO"; Cards[4].Value = 2; Cards[4].Color = 1;
            Cards[5].Drawed = false; Cards[5].Name = "TWO"; Cards[5].Value = 2; Cards[5].Color = 2;
            Cards[6].Drawed = false; Cards[6].Name = "TWO"; Cards[6].Value = 2; Cards[6].Color = 3;
            Cards[7].Drawed = false; Cards[7].Name = "TWO"; Cards[7].Value = 2; Cards[7].Color = 4;
            Cards[8].Drawed = false; Cards[8].Name = "THREE"; Cards[8].Value = 3; Cards[8].Color = 1;
            Cards[9].Drawed = false; Cards[9].Name = "THREE"; Cards[9].Value = 3; Cards[9].Color = 2;
            Cards[10].Drawed = false; Cards[10].Name = "THREE"; Cards[10].Value = 3; Cards[10].Color = 3;
            Cards[11].Drawed = false; Cards[11].Name = "THREE"; Cards[11].Value = 3; Cards[11].Color = 4;
            Cards[12].Drawed = false; Cards[12].Name = "FOUR"; Cards[12].Value = 4; Cards[12].Color = 1;
            Cards[13].Drawed = false; Cards[13].Name = "FOUR"; Cards[13].Value = 4; Cards[13].Color = 2;
            Cards[14].Drawed = false; Cards[14].Name = "FOUR"; Cards[14].Value = 4; Cards[14].Color = 3;
            Cards[15].Drawed = false; Cards[15].Name = "FOUR"; Cards[15].Value = 4; Cards[15].Color = 4;
            Cards[16].Drawed = false; Cards[16].Name = "FIVE"; Cards[16].Value = 5; Cards[16].Color = 1;
            Cards[17].Drawed = false; Cards[17].Name = "FIVE"; Cards[17].Value = 5; Cards[17].Color = 2;
            Cards[18].Drawed = false; Cards[18].Name = "FIVE"; Cards[18].Value = 5; Cards[18].Color = 3;
            Cards[19].Drawed = false; Cards[19].Name = "FIVE"; Cards[19].Value = 5; Cards[19].Color = 4;
            Cards[20].Drawed = false; Cards[20].Name = "SIX"; Cards[20].Value = 6; Cards[20].Color = 1;
            Cards[21].Drawed = false; Cards[21].Name = "SIX"; Cards[21].Value = 6; Cards[21].Color = 2;
            Cards[22].Drawed = false; Cards[22].Name = "SIX"; Cards[22].Value = 6; Cards[22].Color = 3;
            Cards[23].Drawed = false; Cards[23].Name = "SIX"; Cards[23].Value = 6; Cards[23].Color = 4;
            Cards[24].Drawed = false; Cards[24].Name = "SEVEN"; Cards[24].Value = 7; Cards[24].Color = 1;
            Cards[25].Drawed = false; Cards[25].Name = "SEVEN"; Cards[25].Value = 7; Cards[25].Color = 2;
            Cards[26].Drawed = false; Cards[26].Name = "SEVEN"; Cards[26].Value = 7; Cards[26].Color = 3;
            Cards[27].Drawed = false; Cards[27].Name = "SEVEN"; Cards[27].Value = 7; Cards[27].Color = 4;
            Cards[28].Drawed = false; Cards[28].Name = "EIGHT"; Cards[28].Value = 8; Cards[28].Color = 1;
            Cards[29].Drawed = false; Cards[29].Name = "EIGHT"; Cards[29].Value = 8; Cards[29].Color = 2;
            Cards[30].Drawed = false; Cards[30].Name = "EIGHT"; Cards[30].Value = 8; Cards[30].Color = 3;
            Cards[31].Drawed = false; Cards[31].Name = "EIGHT"; Cards[31].Value = 8; Cards[31].Color = 4;
            Cards[32].Drawed = false; Cards[32].Name = "NINE"; Cards[32].Value = 9; Cards[32].Color = 1;
            Cards[33].Drawed = false; Cards[33].Name = "NINE"; Cards[33].Value = 9; Cards[33].Color = 2;
            Cards[34].Drawed = false; Cards[34].Name = "NINE"; Cards[34].Value = 9; Cards[34].Color = 3;
            Cards[35].Drawed = false; Cards[35].Name = "NINE"; Cards[35].Value = 9; Cards[35].Color = 4;
            Cards[36].Drawed = false; Cards[36].Name = "TEN"; Cards[36].Value = 10; Cards[36].Color = 1;
            Cards[37].Drawed = false; Cards[37].Name = "TEN"; Cards[37].Value = 10; Cards[37].Color = 2;
            Cards[38].Drawed = false; Cards[38].Name = "TEN"; Cards[38].Value = 10; Cards[38].Color = 3;
            Cards[39].Drawed = false; Cards[39].Name = "TEN"; Cards[39].Value = 10; Cards[39].Color = 4;
            Cards[40].Drawed = false; Cards[40].Name = "JACK"; Cards[40].Value = 10; Cards[40].Color = 1;
            Cards[41].Drawed = false; Cards[41].Name = "JACK"; Cards[41].Value = 10; Cards[41].Color = 2;
            Cards[42].Drawed = false; Cards[42].Name = "JACK"; Cards[42].Value = 10; Cards[42].Color = 3;
            Cards[43].Drawed = false; Cards[43].Name = "JACK"; Cards[43].Value = 10; Cards[43].Color = 4;
            Cards[44].Drawed = false; Cards[44].Name = "QUEEN"; Cards[44].Value = 10; Cards[44].Color = 1;
            Cards[45].Drawed = false; Cards[45].Name = "QUEEN"; Cards[45].Value = 10; Cards[45].Color = 2;
            Cards[46].Drawed = false; Cards[46].Name = "QUEEN"; Cards[46].Value = 10; Cards[46].Color = 3;
            Cards[47].Drawed = false; Cards[47].Name = "QUEEN"; Cards[47].Value = 10; Cards[47].Color = 4;
            Cards[48].Drawed = false; Cards[48].Name = "KING"; Cards[48].Value = 10; Cards[48].Color = 1;
            Cards[49].Drawed = false; Cards[49].Name = "KING"; Cards[49].Value = 10; Cards[49].Color = 2;
            Cards[50].Drawed = false; Cards[50].Name = "KING"; Cards[50].Value = 10; Cards[50].Color = 3;
            Cards[51].Drawed = false; Cards[51].Name = "KING"; Cards[51].Value = 10; Cards[51].Color = 4;





        }
    }
    internal class Card

    {

        public bool Drawed { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int Color { get; set; }
    }
    internal class Player

    {
        public List<Card> PlayerCards = new List<Card>();
        public int Cash { get; set; }

        public int GetTotalCards()
        {
            int temp = 0;

            foreach (var Card in PlayerCards)
            {
                temp += Card.Value;
            }
            if (temp > 21)
            {
                var fish = PlayerCards.FindAll(x => x.Name.Contains("ACE"));
                if (fish.Count > 0) //HER
                {
                    // do logic here
                }
            }


            return temp;
        }
        public void WriteCards()
        {
            foreach (var Card in PlayerCards)
            {
                System.Console.WriteLine(Card.Name);

            }
        }
    }




}

