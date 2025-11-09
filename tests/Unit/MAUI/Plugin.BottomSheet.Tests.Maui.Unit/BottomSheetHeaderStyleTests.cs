using Plugin.Maui.BottomSheet;

namespace Plugin.BottomSheet.Tests.Maui.Unit;

public class BottomSheetHeaderStyleTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var style = new BottomSheetHeaderStyle();
        
        // Assert
        Assert.NotNull(style);
        Assert.IsAssignableFrom<BindableObject>(style);
    }

    [Fact]
    public void TitleTextColor_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedColor = Colors.Red;
        
        // Act
        style.TitleTextColor = expectedColor;
        var actualColor = style.TitleTextColor;
        
        // Assert
        Assert.Equal(expectedColor, actualColor);
    }

    [Fact]
    public void TitleTextFontSize_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedSize = 18.5;
        
        // Act
        style.TitleTextFontSize = expectedSize;
        var actualSize = style.TitleTextFontSize;
        
        // Assert
        Assert.Equal(expectedSize, actualSize);
    }

    [Fact]
    public void TitleTextFontAttributes_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedAttributes = FontAttributes.Bold | FontAttributes.Italic;
        
        // Act
        style.TitleTextFontAttributes = expectedAttributes;
        var actualAttributes = style.TitleTextFontAttributes;
        
        // Assert
        Assert.Equal(expectedAttributes, actualAttributes);
    }

    [Fact]
    public void TitleTextFontFamily_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedFontFamily = "Arial";
        
        // Act
        style.TitleTextFontFamily = expectedFontFamily;
        var actualFontFamily = style.TitleTextFontFamily;
        
        // Assert
        Assert.Equal(expectedFontFamily, actualFontFamily);
    }

    [Fact]
    public void TitleTextFontAutoScalingEnabled_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        
        // Act & Assert - Test true
        style.TitleTextFontAutoScalingEnabled = true;
        Assert.True(style.TitleTextFontAutoScalingEnabled);
        
        // Act & Assert - Test false
        style.TitleTextFontAutoScalingEnabled = false;
        Assert.False(style.TitleTextFontAutoScalingEnabled);
    }

    [Fact]
    public void CloseButtonHeightRequest_ShouldHaveDefaultValue()
    {
        // Arrange & Act
        var style = new BottomSheetHeaderStyle();
        
        // Assert
        Assert.Equal(40.0, style.CloseButtonHeightRequest);
    }

    [Fact]
    public void CloseButtonHeightRequest_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedHeight = 50.0;
        
        // Act
        style.CloseButtonHeightRequest = expectedHeight;
        var actualHeight = style.CloseButtonHeightRequest;
        
        // Assert
        Assert.Equal(expectedHeight, actualHeight);
    }

    [Fact]
    public void CloseButtonWidthRequest_ShouldHaveDefaultValue()
    {
        // Arrange & Act
        var style = new BottomSheetHeaderStyle();
        
        // Assert
        Assert.Equal(40.0, style.CloseButtonWidthRequest);
    }

    [Fact]
    public void CloseButtonWidthRequest_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedWidth = 60.0;
        
        // Act
        style.CloseButtonWidthRequest = expectedWidth;
        var actualWidth = style.CloseButtonWidthRequest;
        
        // Assert
        Assert.Equal(expectedWidth, actualWidth);
    }

    [Fact]
    public void CloseButtonTintColor_ShouldHaveDefaultValue()
    {
        // Arrange & Act
        var style = new BottomSheetHeaderStyle();
        
        // Assert
        Assert.NotNull(style.CloseButtonTintColor);
        // Note: The exact default color depends on platform compilation
    }

    [Fact]
    public void CloseButtonTintColor_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        var expectedColor = Colors.Blue;
        
        // Act
        style.CloseButtonTintColor = expectedColor;
        var actualColor = style.CloseButtonTintColor;
        
        // Assert
        Assert.Equal(expectedColor, actualColor);
    }

    [Fact]
    public void BindableProperties_ShouldBeConfiguredCorrectly()
    {
        // Assert - Verify property names match
        Assert.Equal(nameof(BottomSheetHeaderStyle.TitleTextColor), BottomSheetHeaderStyle.TitleTextColorProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.TitleTextFontSize), BottomSheetHeaderStyle.TitleTextFontSizeProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.TitleTextFontAttributes), BottomSheetHeaderStyle.TitleTextFontAttributesProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.TitleTextFontFamily), BottomSheetHeaderStyle.TitleTextFontFamilyProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.TitleTextFontAutoScalingEnabled), BottomSheetHeaderStyle.TitleTextFontAutoScalingEnabledProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.CloseButtonHeightRequest), BottomSheetHeaderStyle.CloseButtonHeightRequestProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.CloseButtonWidthRequest), BottomSheetHeaderStyle.CloseButtonWidthRequestProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetHeaderStyle.CloseButtonTintColor), BottomSheetHeaderStyle.CloseButtonTintColorProperty.PropertyName);
    }

    [Fact]
    public void BindableProperties_ShouldHaveCorrectReturnTypes()
    {
        // Assert
        Assert.Equal(typeof(Color), BottomSheetHeaderStyle.TitleTextColorProperty.ReturnType);
        Assert.Equal(typeof(double), BottomSheetHeaderStyle.TitleTextFontSizeProperty.ReturnType);
        Assert.Equal(typeof(FontAttributes), BottomSheetHeaderStyle.TitleTextFontAttributesProperty.ReturnType);
        Assert.Equal(typeof(string), BottomSheetHeaderStyle.TitleTextFontFamilyProperty.ReturnType);
        Assert.Equal(typeof(bool), BottomSheetHeaderStyle.TitleTextFontAutoScalingEnabledProperty.ReturnType);
        Assert.Equal(typeof(double), BottomSheetHeaderStyle.CloseButtonHeightRequestProperty.ReturnType);
        Assert.Equal(typeof(double), BottomSheetHeaderStyle.CloseButtonWidthRequestProperty.ReturnType);
        Assert.Equal(typeof(Color), BottomSheetHeaderStyle.CloseButtonTintColorProperty.ReturnType);
    }

    [Fact]
    public void BindableProperties_ShouldHaveCorrectDeclaringType()
    {
        // Assert
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.TitleTextColorProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.TitleTextFontSizeProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.TitleTextFontAttributesProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.TitleTextFontFamilyProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.TitleTextFontAutoScalingEnabledProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.CloseButtonHeightRequestProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.CloseButtonWidthRequestProperty.DeclaringType);
        Assert.Equal(typeof(BottomSheetHeaderStyle), BottomSheetHeaderStyle.CloseButtonTintColorProperty.DeclaringType);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(10.5)]
    [InlineData(100.0)]
    [InlineData(double.MaxValue)]
    public void CloseButtonDimensions_ShouldAcceptValidDoubleValues(double value)
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        
        // Act & Assert
        style.CloseButtonHeightRequest = value;
        Assert.Equal(value, style.CloseButtonHeightRequest);
        
        style.CloseButtonWidthRequest = value;
        Assert.Equal(value, style.CloseButtonWidthRequest);
    }

    [Fact]
    public void TitleTextFontFamily_ShouldAcceptNullValue()
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        
        // Act
        style.TitleTextFontFamily = null!;
        
        // Assert
        Assert.Null(style.TitleTextFontFamily);
    }

    [Theory]
    [InlineData(FontAttributes.None)]
    [InlineData(FontAttributes.Bold)]
    [InlineData(FontAttributes.Italic)]
    [InlineData(FontAttributes.Bold | FontAttributes.Italic)]
    public void TitleTextFontAttributes_ShouldAcceptValidFontAttributes(FontAttributes attributes)
    {
        // Arrange
        var style = new BottomSheetHeaderStyle();
        
        // Act
        style.TitleTextFontAttributes = attributes;
        
        // Assert
        Assert.Equal(attributes, style.TitleTextFontAttributes);
    }
}