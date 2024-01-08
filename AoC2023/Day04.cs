using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2023
{
    internal class Day04:DayBase
    {
        public Day04() : base(4)
        {
        }
        public override int SolveProblem_1()
        {
            int result = 0;
            foreach (string text in _textInput)
            {
                var card = ParseInputToCard(text);
                result += card.MatchingNumbers;
            }
            return result;
        }

        public override int SolveProblem_2()
        {
            int result = 0;
            List<Card> cards = new List<Card>();

            foreach (string text in _textInput)
            {
                var card = ParseInputToCard(text);
                cards.Add(card);
            }

            foreach (var card in cards)
            {
                GetCopies(card, cards);
            }
            
            foreach (var card in cards)
            {
                result += card.CountCopiesRecursively();
            }
            result += cards.Count;
            return result;
        }


        private void GetCopies(Card card, List<Card> cards)
        {
            // get copies
            List<Card> nextCards = cards.Skip(card.ID).Take(card.CommonValues.Count()).ToList();
            //card.Copies.AddRange(nextCards);
            foreach (var c in nextCards)
            {
                // create copies
                var copy = c.Copy();
                card.Copies.Add(copy);
                GetCopies(copy, cards);
            }
        }

        private Card ParseInputToCard(string inputText)
        {
            var test = inputText.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            // Extracting the game number using regex
            Match cardMatch = Regex.Match(inputText, @"Card\s+(\d+):");
            int cardID = 0;
            if (cardMatch.Success)
            {
                cardID = int.Parse(cardMatch.Groups[1].Value);
            }

            // Splitting the text after the colon (;)
            string[] parts = Regex.Split(inputText.Substring(cardMatch.Index + cardMatch.Length), @"\|");

            Card card = new Card(cardID);
            card.FirstDeck = ParseDeck(parts[0]);
            card.SecondDeck = ParseDeck(parts[1]);

            return card;
        }

        List<int> ParseDeck(string deckString)
        {
            return deckString.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }

    class Card
    {
        public int ID { get; set; }
        public List<int> FirstDeck { get; set; }
        public List<int> SecondDeck { get; set; }
        public IEnumerable<int> CommonValues => SecondDeck.Intersect(FirstDeck);
        public List<Card> Copies { get; set; } = new List<Card>();

        public int MatchingNumbers 
        { 
            get
            {
                IEnumerable<int> commonValues = SecondDeck.Intersect(FirstDeck);

                var pts = 0;
                for (int i = 0; i < commonValues.Count(); i++)
                {
                    pts = i == 0 ? 1 : pts * 2;
                }
                return pts;
            }
        }

        public Card(int iD)
        {
            ID = iD;
        }

        // Copy method to create a new Card instance
        public Card Copy()
        {
            Card newCard = new Card(ID)
            {
                FirstDeck = new List<int>(FirstDeck),
                SecondDeck = new List<int>(SecondDeck),
                Copies = new List<Card>(Copies)
            };
            return newCard;
        }

        public int CountCopiesRecursively()
        {
            int totalCopies = Copies.Count;

            foreach (var card in Copies)
            {
                totalCopies += card.CountCopiesRecursively();
            }

            return totalCopies;
        }
    }
}
