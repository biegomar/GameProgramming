using Sortings.Core.Display;

namespace Sortings.Core.Test;

/// <summary>
/// The display test suite.
/// </summary>
public class DisplayTest
{
    private int[] sortedArrayEven = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] sortedArrayEvenZickZack = new[] { 10, 1, 9, 2, 8, 3, 7, 4, 6, 5 };
    
    private int[] sortedArrayOdd = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int[] sortedArrayOddZickZack = new[] { 9, 1, 8, 2, 7, 3, 6, 4, 5 };
    
    [Fact]
    public void AscendingDisplayEvenTest()
    {
        var sut = new AscendingDisplay<int>();

        var expected = string.Join(',', sortedArrayEven);
        var actual = sut.Display(sortedArrayEven);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void DescandingDisplayEvenTest()
    {
        var sut = new DescendingDisplay<int>();

        var expected = string.Join(',', sortedArrayEven.Reverse());
        var actual = sut.Display(sortedArrayEven);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ZickZackDisplayEvenTest()
    {
        var sut = new ZickZackDisplay<int>();

        var expected = string.Join(',', sortedArrayEvenZickZack);
        var actual = sut.Display(sortedArrayEven);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void AscendingDisplayOddTest()
    {
        var sut = new AscendingDisplay<int>();

        var expected = string.Join(',', sortedArrayOdd);
        var actual = sut.Display(sortedArrayOdd);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void DescandingDisplayOddTest()
    {
        var sut = new DescendingDisplay<int>();

        var expected = string.Join(',', sortedArrayOdd.Reverse());
        var actual = sut.Display(sortedArrayOdd);
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ZickZackDisplayOddTest()
    {
        var sut = new ZickZackDisplay<int>();

        var expected = string.Join(',', sortedArrayOddZickZack);
        var actual = sut.Display(sortedArrayOdd);
        
        Assert.Equal(expected, actual);
    }
}