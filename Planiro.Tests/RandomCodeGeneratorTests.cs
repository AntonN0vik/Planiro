using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planiro.Application.Services;

[TestClass]
public class RandomCodeGeneratorTests
{
    [TestMethod]
    public void GenerateCode_ShouldReturnCorrectLength()
    {
        // Act
        var code = RandomCodeGenerator.GenerateCode(8);
        
        // Assert
        Assert.AreEqual(8, code.Length);
    }

    [TestMethod]
    public void GenerateCode_ShouldContainOnlyAllowedCharacters()
    {
        // Act
        var code = RandomCodeGenerator.GenerateCode(100);
        
        // Assert
        Assert.IsTrue(code.All(c => 
            char.IsLetterOrDigit(c) && 
            !char.IsSymbol(c)));
    }
}