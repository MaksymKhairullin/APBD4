using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{

    [Fact]
    public void AddUser_Should_Return_False_When_First_Name_Is_InCorrect()
    {
        var userS = new UserService();

        var result = userS.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        Assert.Equal(false,result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Last_Name_Is_InCorrect()
    {
        var userS = new UserService();

        var result = userS.AddUser("John", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        Assert.Equal(false,result);
    }
    
    
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_and_Dot()
    {
        var userS = new UserService();
        
        var result = userS.AddUser("Joe", "Doe", "johndoegmailcom", DateTime.Parse("1982-03-21"), 1);

        
        Assert.Equal(false,result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Age_IsUnder21()
    {
        
        var userService = new UserService();

        
        var result = userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(2023, 3, 21), 3); // Assuming current year is 2024

        
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Set_User_Has_Credit_Limit_To_False_For_Very_Important_Client()
    {
        
        var clientId = 1;
        var client = new Client { Type = "VeryImportantClient" }; 
        var user = new User();

        var userService = new UserService();

        
        userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), clientId);

        
        Assert.False(user.HasCreditLimit);
    }
    
    [Fact]
    public void AddUser_Should_Set_User_Has_Credit_Limit_To_False_For_Important_Client()
    {
       
        var clientId = 1;
        var client = new Client { Type = "ImportantClient" };
        var user = new User(); 

        
        var userService = new UserService();
        
        userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), clientId);

        
        Assert.False(user.HasCreditLimit);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Credit_Limit_And_User_Has_Less_500()
    {
        
        var userService = new UserService();
        var user = new User();
        user.CreditLimit = 5000;
        user.HasCreditLimit = false;
        
        var result = userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), 3); // Assuming current year is 2024

        
        Assert.Equal(false,user.HasCreditLimit && user.CreditLimit <500);
    }
    
    
    [Fact]
    public void AddUser_Should_Set_User_Has_Not_Credit_Limit_For_Very_Important_Client()
    {
       
        var clientId = 1;
        var client = new Client { Type = "VeryImportantClient" };

        client.Type = "VeryImportantClient";
        var user = new User();
        user.HasCreditLimit = false;

        
        var userService = new UserService();
        
        userService.AddUser("John", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), clientId);

        
        Assert.False(user.HasCreditLimit);
    }

    [Fact]
    public void AddUser_Should_Return_False_For_Client_Very_Important()
    {
        User user = new User();
        var client = UserService.CreateClient("John", "Doe", "asdasda@gmail.com", new DateTime(1982, 3, 05), 1,out user);

        client.Type = "VeryImportantClient";
        user.HasCreditLimit = false;

        UserService userService = new UserService();

        bool res = userService.AddUser("John","Doe","johndoe@gmail.com",new DateTime(1982,03,05),1);
        
        Assert.Equal(true,res);
    }

    [Fact]
    public void AddUser_Should_Return_False_For_User_Has_Credit_Limit_And_Credit_Limit_Less_500()
    {
        User user = new User();
        var client = UserService.CreateClient("John", "Doe", "asdasda@gmail.com", new DateTime(1982, 3, 05), 1,out user);

        client.Type = "NormalClient";
        user.HasCreditLimit = true;
        user.CreditLimit = 400;

        UserService userService = new UserService();
        bool resforcreditlimit = user.CreditLimit < 500;

        bool res = userService.AddUser("John","Doe","johndoe@gmail.com",new DateTime(1982,03,05),1);
        
        Assert.Equal(true,res);
    }

}
