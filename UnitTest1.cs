using Xunit;
using RandomStringNumbers;
using System.Collections.Generic;
using System.Linq;

namespace Tests;

public class NumberProcessorTests
{
    private readonly NumberProcessor _processor = new();

    [Fact]
    public void Generate_ReturnsCorrectCount()
    {
        // Arrange
        int count = 100;

        // Act
        var result = _processor.Generate(count);

        // Assert
        Assert.Equal(count, result.Count);
        Assert.All(result, item => Assert.True(item.Length >= 4 && item.Length <= 8));
    }

    [Fact]
    public void GetEvens_ReturnsOnlyEvenNumbers()
    {
        // Arrange
        var input = new List<string> { "1234", "1235", "2222", "3333", "1000" };

        // Act
        var result = _processor.GetEvens(input);

        // Assert
        Assert.Contains("1234", result.LinqResult);
        Assert.Contains("2222", result.LinqResult);
        Assert.Contains("1000", result.LinqResult);
        Assert.DoesNotContain("1235", result.LinqResult);
        Assert.DoesNotContain("3333", result.LinqResult);

        // Проверка сортировки по убыванию
        Assert.Equal(new[] { "2222", "1234", "1000" }, result.LinqResult);
    }

    [Fact]
    public void GetSpecialSum_CalculatesSecondAndPenultimateCorrectly()
    {
        // Arrange
        var input = new List<string>
        {
            "1235", // 2-я = 2, предпоследняя = 3. Сумма = 5 (не подходит)
            "1432", // 2-я = 4, предпоследняя = 3. Сумма = 7 (не подходит)
            "1240", // 2-я = 2, предпоследняя = 4. Сумма = 6 (подходит)
            "222",  // 3 цифры (не подходит по условию > 3)
            "1530"  // 2-я = 5, предпоследняя = 3. Сумма = 8 (не подходит)
        };

        // Act
        var result = _processor.GetSpecialSum(input);

        // Assert
        Assert.Single(result.LinqResult);
        Assert.Equal("1240", result.LinqResult[0]);
    }

    [Fact]
    public void GetDigitSum13_FindsCorrectNumbers()
    {
        // Arrange
        var input = new List<string>
        {
            "1237", // 1+2+3+7 = 13 (подходит)
            "1111", // 1+1+1+1 = 4 (не подходит)
            "9022", // 9+0+2+2 = 13 (подходит)
            "55"    // 5+5 = 10 (не подходит)
        };

        // Act
        var result = _processor.GetDigitSum13(input);

        // Assert
        Assert.Equal(2, result.LinqResult.Count);
        // Проверка сортировки по убыванию
        Assert.Equal(new[] { "9022", "1237" }, result.LinqResult);
    }
}