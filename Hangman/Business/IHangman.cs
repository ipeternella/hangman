using System;

namespace Hangman.Business
{
    public interface IHangman
    {
        // utility
        public bool IsGuessedLetterInGuessWord(string guessLetter, string guessWord);
        public string[] GetFilledLetters();  // _ O _ F (wolf) --> [*, O, L, F];
        public GameRound FinishGameRound();
        
        // when player misses a guess 
        public GameRound ReducePlayerHealth(GameRound gameRound, int amountOfHits = 1);
        public bool HasPlayerBeenHung(GameRound gameRound);
        
        // when player guesses a letter
        public bool HasPlayerHasDiscoveredGuessWord();
    }
}