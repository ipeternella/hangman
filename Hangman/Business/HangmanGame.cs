using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman.Business
{
    public class HangmanGame : IHangmanGame
    {
        // utility
        public bool IsGuessedLetterInGuessWord(string guessLetter, string guessWord)
        {
            return guessWord.ToLower().Contains(guessLetter.ToLower());
        }

        public IEnumerable<string> GetGuessWordSoFar(IEnumerable<string> guessedLetters, string guessWord)
        {
            // ["*", "O", "L", "F"], guessWord: "wolf", guessLetters: ["l", "f", "o"]
            
            // naive solution
            // for each letter, check if its within guessed letters => if not, mark it with a "*"
            var guessedWordSoFar = new List<string>();
            var guessedLettersSoFar = string.Join("", guessedLetters.ToArray());

            foreach (var guessWordChar in guessWord)
            {
                // if guessed letters appears in the guess word, then it's added to final result, otherwise a "*" is added
                guessedWordSoFar.Add(guessedLettersSoFar.Contains(guessWordChar.ToString()) ? guessWordChar.ToString() : "*");
            }

            return guessedWordSoFar;
        }
        
        // public GameRound FinishGameRound();
        //
        // // when player misses a guess 
        // public GameRound ReducePlayerHealth(GameRound gameRound, int amountOfHits = 1);
        // public bool HasPlayerBeenHung(GameRound gameRound);
        //
        // // when player guesses a letter
        // public bool HasPlayerHasDiscoveredGuessWord();
    }
}