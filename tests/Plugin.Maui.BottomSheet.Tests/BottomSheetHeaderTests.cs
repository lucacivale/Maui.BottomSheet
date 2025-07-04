namespace Plugin.Maui.BottomSheet.Tests;

public class BottomSheetHeaderTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var header = new BottomSheetHeader();

        // Assert
        Assert.Null(header.TitleText);
        Assert.Null(header.TopLeftButton);
        Assert.Null(header.TopRightButton);
        Assert.False(header.ShowCloseButton);
        Assert.Equal(CloseButtonPosition.TopRight, header.CloseButtonPosition);
        Assert.Null(header.HeaderDataTemplate);
        Assert.Null(header.Content);
        Assert.Equal(BottomSheetHeaderButtonAppearanceMode.None, header.HeaderAppearance);
        Assert.Null(header.Parent);
    }

    [Fact]
    public void TitleText_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        const string expectedTitle = "Test Title";

        // Act
        header.TitleText = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, header.TitleText);
    }

    [Fact]
    public void TopLeftButton_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var button = new Button { Text = "Left Button" };

        // Act
        header.TopLeftButton = button;

        // Assert
        Assert.Equal(button, header.TopLeftButton);
    }

    [Fact]
    public void TopRightButton_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var button = new Button { Text = "Right Button" };

        // Act
        header.TopRightButton = button;

        // Assert
        Assert.Equal(button, header.TopRightButton);
    }

    [Fact]
    public void ShowCloseButton_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act
        header.ShowCloseButton = true;

        // Assert
        Assert.True(header.ShowCloseButton);
    }

    [Theory]
    [InlineData(CloseButtonPosition.TopLeft)]
    [InlineData(CloseButtonPosition.TopRight)]
    public void CloseButtonPosition_ShouldSetAndGetValue(CloseButtonPosition position)
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act
        header.CloseButtonPosition = position;

        // Assert
        Assert.Equal(position, header.CloseButtonPosition);
    }

    [Fact]
    public void HeaderDataTemplate_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var template = new DataTemplate(() => new Label { Text = "Template Content" });

        // Act
        header.HeaderDataTemplate = template;

        // Assert
        Assert.Equal(template, header.HeaderDataTemplate);
    }

    [Fact]
    public void Content_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var content = new Label { Text = "Direct Content" };

        // Act
        header.Content = content;

        // Assert
        Assert.Equal(content, header.Content);
    }

    [Theory]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.None)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton)]
    public void HeaderAppearance_ShouldSetAndGetValue(BottomSheetHeaderButtonAppearanceMode appearance)
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act
        header.HeaderAppearance = appearance;

        // Assert
        Assert.Equal(appearance, header.HeaderAppearance);
    }

    [Fact]
    public void Parent_ShouldSetAndGetValue()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var parent = new ContentPage();

        // Act
        header.Parent = parent;

        // Assert
        Assert.Equal(parent, header.Parent);
    }

    [Fact]
    public void CreateContent_WithDirectContent_ShouldReturnContent()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var content = new Label { Text = "Test Content" };
        var bindingContext = new { Title = "Test" };
        var parent = new ContentPage();

        header.Content = content;
        header.BindingContext = bindingContext;
        header.Parent = parent;

        // Act
        var result = header.CreateContent();

        // Assert
        Assert.Equal(content, result);
        Assert.Equal(bindingContext, result.BindingContext);
        Assert.Equal(parent, result.Parent);
    }

    [Fact]
    public void CreateContent_WithDataTemplate_ShouldCreateAndReturnContent()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var template = new DataTemplate(() => new Label { Text = "Template Content" });
        var bindingContext = new { Title = "Test" };
        var parent = new ContentPage();

        header.HeaderDataTemplate = template;
        header.BindingContext = bindingContext;
        header.Parent = parent;

        // Act
        var result = header.CreateContent();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Label>(result);
        Assert.Equal("Template Content", ((Label)result).Text);
        Assert.Equal(bindingContext, result.BindingContext);
        Assert.Equal(parent, result.Parent);
        Assert.Equal(result, header.Content); // Content should be set after template creation
    }

    [Fact]
    public void CreateContent_WithBothTemplateAndContent_ShouldPrioritizeTemplate()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var directContent = new Label { Text = "Direct Content" };
        var template = new DataTemplate(() => new Label { Text = "Template Content" });

        header.Content = directContent;
        header.HeaderDataTemplate = template;

        // Act
        var result = header.CreateContent();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Label>(result);
        Assert.Equal("Template Content", ((Label)result).Text);
        Assert.NotEqual(directContent, result); // Should not be the direct content
    }

    [Fact]
    public void CreateContent_WithNeitherContentNorTemplate_ShouldThrowException()
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act & Assert
        var exception = Assert.Throws<BottomSheetContentNotSetException>(() => header.CreateContent());
        Assert.Contains("Content must be set before creating content", exception.Message);
    }

    [Fact]
    public void CreateContent_WithNullTemplateResult_ShouldThrowException()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var template = new DataTemplate(() => null); // Template returns null

        header.HeaderDataTemplate = template;

        // Act & Assert
        var exception = Assert.Throws<BottomSheetContentNotSetException>(() => header.CreateContent());
        Assert.Contains("Content must be set before creating content", exception.Message);
    }

    [Fact]
    public void CloseButtonPositionProperty_ShouldHaveCorrectDefaultValue()
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act
        var defaultValue = header.CloseButtonPosition;

        // Assert
        Assert.Equal(CloseButtonPosition.TopRight, defaultValue);
    }

    [Fact]
    public void CreateContent_ShouldPreserveBindingContextFromHeader()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var content = new Label { Text = "Test Content" };
        var bindingContext = new { TestProperty = "TestValue" };

        header.Content = content;
        header.BindingContext = bindingContext;

        // Act
        var result = header.CreateContent();

        // Assert
        Assert.Equal(bindingContext, result.BindingContext);
    }

    [Fact]
    public void CreateContent_ShouldSetParentFromHeader()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var content = new Label { Text = "Test Content" };
        var parent = new ContentPage();

        header.Content = content;
        header.Parent = parent;

        // Act
        var result = header.CreateContent();

        // Assert
        Assert.Equal(parent, result.Parent);
    }
}