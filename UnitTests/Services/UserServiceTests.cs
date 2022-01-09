using Infrastructure.Identity;
using Xunit;

namespace UnitTests.Services
{
    public class UserServiceTests
    {
        //deklauje metode testowa, ktora nastepnie jest uruchamiana przez program test runner
        [Fact]
        public void is_user_email_confirmed_should_return_true_when_input_is_true()
        {
            //Arrange
            //definiujemy wszystkie dane wejściowe (przygotowanie danych)
            ApplicationUser user = new ApplicationUser();
            user.UserName = "piotr";
            user.EmailConfirmed = true;

            UserService service = new UserService();

            //Act
            //definiujemy działanie na funkcji metodzie czy klasie testowanej
            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            //Assert
            //upewniamy sie ze zwrocone wartosci sa zgodne z oczekiwanymi
            Assert.True(isEmailConfirmed);
        }

        [Fact]
        public void is_user_email_confirmed_should_return_false_when_input_is_false()
        {
            //Arrange
            //definiujemy wszystkie dane wejściowe (przygotowanie danych)
            ApplicationUser user = new ApplicationUser();
            user.UserName = "piotr";
            user.EmailConfirmed = false;

            UserService service = new UserService();

            //Act
            //definiujemy działanie na funkcji metodzie czy klasie testowanej
            bool isEmailConfirmed = service.IsUserEmailConfirmed(user);

            //Assert
            //upewniamy sie ze zwrocone wartosci sa zgodne z oczekiwanymi
            Assert.False(isEmailConfirmed);
        }
    }
}
