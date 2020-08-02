using System.Collections.Generic;
using Hangman.Business;
using Xunit;

namespace Tests.Hangman.Unit.Business
{
    public class HangmanTests
    {
        private readonly HangmanGame _gameLogic; 
        
        public HangmanTests()
        {
            _gameLogic = new HangmanGame();
        }

        [Fact(DisplayName = "Should return true if guessed letter belongs to guess word")]
        public void TestShouldReturnTrueIfGuessedLetterBelongsToGuessedWord()
        {
            var guessedLetter = "i";
            var guessWord = "igor";

            Assert.True(_gameLogic.IsGuessedLetterInGuessWord(guessedLetter, guessWord));
        }
        
        [Fact(DisplayName = "Should return true if guessed letter belongs to guess word ignoring case")]
        public void TestShouldReturnTrueIfGuessedLetterBelongsToGuessedWordIgnoringCase()
        {
            const string guessedLetter = "G";
            const string guessWord = "igor";

            Assert.True(_gameLogic.IsGuessedLetterInGuessWord(guessedLetter, guessWord));
        }
        
        [Fact(DisplayName = "Should display some guessed letters of the guess word")]
        public void TestShouldDisplaySomeGuessedLettersOfGuessWord()
        {
            // arrange
            var guessedLetters = new List<string>() {"r", "o", "g"};
            const string guessWord = "igooor";

            // act
            var guessWordSoFar = _gameLogic.GetGuessWordSoFar(guessedLetters, guessWord);
            
            // assert
            var expectedGuessWordSoFar = new List<string>() {"*", "g", "o", "o", "o", "r"};
            Assert.Equal(expectedGuessWordSoFar, guessWordSoFar);
        }
        
        [Fact(DisplayName = "Should display all guessed letters of the guess word")]
        public void TestShouldDisplayAllGuessedLettersOfGuessWord()
        {
            // arrange
            var guessedLetters = new List<string>() {"r", "o", "g", "i", "k"};  // random order
            const string guessWord = "igooor";

            // act
            var guessWordSoFar = _gameLogic.GetGuessWordSoFar(guessedLetters, guessWord);
            
            // assert
            var expectedGuessWordSoFar = new List<string>() {"i", "g", "o", "o", "o", "r"};
            Assert.Equal(expectedGuessWordSoFar, guessWordSoFar);
        }
        
        [Fact(DisplayName = "Should display no guessed letters of the guess word")]
        public void TestShouldDisplayNoGuessedLettersOfGuessWord()
        {
            // arrange
            var guessedLetters = new List<string>() {};
            const string guessWord = "igooor";

            // act
            var guessWordSoFar = _gameLogic.GetGuessWordSoFar(guessedLetters, guessWord);
            
            // assert
            var expectedGuessWordSoFar = new List<string>() {"*", "*", "*", "*", "*", "*"};
            Assert.Equal(expectedGuessWordSoFar, guessWordSoFar);
        }
        
        [Fact(DisplayName = "Should display no guessed letters of the guess word if null is passed")]
        public void TestShouldDisplayNoGuessedLettersOfGuessWordWhenNullIsPassed()
        {
            // arrange
            const string guessWord = "igooor";

            // act
            var guessWordSoFar = _gameLogic.GetGuessWordSoFar(null, guessWord);
            
            // assert
            var expectedGuessWordSoFar = new List<string>() {"*", "*", "*", "*", "*", "*"};
            Assert.Equal(expectedGuessWordSoFar, guessWordSoFar);
        }
    }
}