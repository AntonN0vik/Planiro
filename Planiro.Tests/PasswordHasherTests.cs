using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planiro.Application.Services;

[TestClass]
public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new PasswordHasher();

    [TestMethod]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        var hash = _hasher.HashPassword("password123");
        Assert.IsFalse(string.IsNullOrEmpty(hash));
    }

    [TestMethod]
    public void VerifyPassword_ShouldReturnTrueForValidPassword()
    {
        var password = "password123";
        var hash = _hasher.HashPassword(password);
        Assert.IsTrue(_hasher.VerifyPassword(password, hash));
    }

    [TestMethod]
    public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
    {
        var hash = _hasher.HashPassword("password123");
        Assert.IsFalse(_hasher.VerifyPassword("wrongpassword", hash));
    }
}