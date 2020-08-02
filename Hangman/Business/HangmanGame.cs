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
            guessedLetters ??= new List<string>() { };
            
            var guessedLettersSoFar = string.Join("", guessedLetters.ToArray());

            return guessWord.Select(guessWordChar =>
                guessedLettersSoFar.Contains(guessWordChar.ToString()) ? guessWordChar.ToString() : "*").ToList();
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