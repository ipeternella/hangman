using System;
using System.Collections.Generic;

namespace Hangman.Business
{
    public interface IHangmanGame
    {
        // utility
        public bool IsGuessedLetterInGuessWord(string guessLetter, string guessWord);

        public IEnumerable<string> GetGuessWordSoFar(IEnumerable<string> guessedLetters, string guessWord);
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