namespace Plugin.Maui.BottomSheet.Tests;

public class BottomSheetContentTests
{
    [Fact]
    public void Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var bottomSheetContent = new BottomSheetContent();

        // Assert
        Assert.NotNull(bottomSheetContent);
        Assert.Null(bottomSheetContent.Content);
        Assert.Null(bottomSheetContent.ContentTemplate);
        Assert.Null(bottomSheetContent.Parent);
    }

    [Fact]
    public void Content_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var expectedContent = new Label { Text = "Test Content" };

        // Act
        bottomSheetContent.Content = expectedContent;

        // Assert
        Assert.Equal(expectedContent, bottomSheetContent.Content);
    }

    [Fact]
    public void ContentTemplate_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var expectedTemplate = new DataTemplate(() => new Label { Text = "Template Content" });

        // Act
        bottomSheetContent.ContentTemplate = expectedTemplate;

        // Assert
        Assert.Equal(expectedTemplate, bottomSheetContent.ContentTemplate);
    }

    [Fact]
    public void Parent_SetAndGet_ShouldWorkCorrectly()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var expectedParent = new ContentPage();

        // Act
        bottomSheetContent.Parent = expectedParent;

        // Assert
        Assert.Equal(expectedParent, bottomSheetContent.Parent);
    }

    [Fact]
    public void CreateContent_WithDirectContent_ShouldReturnContent()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var expectedContent = new Label { Text = "Direct Content" };
        var parentElement = new ContentPage();
        var bindingContext = new { TestProperty = "TestValue" };

        bottomSheetContent.Content = expectedContent;
        bottomSheetContent.Parent = parentElement;
        bottomSheetContent.BindingContext = bindingContext;

        // Act
        var result = bottomSheetContent.CreateContent();

        // Assert
        Assert.Equal(expectedContent, result);
        Assert.Equal(bindingContext, result.BindingContext);
        Assert.Equal(parentElement, result.Parent);
    }

    [Fact]
    public void CreateContent_WithContentTemplate_ShouldCreateAndReturnContent()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var template = new DataTemplate(() => new Label { Text = "Template Content" });
        var parentElement = new ContentPage();
        var bindingContext = new { TestProperty = "TestValue" };

        bottomSheetContent.ContentTemplate = template;
        bottomSheetContent.Parent = parentElement;
        bottomSheetContent.BindingContext = bindingContext;

        // Act
        var result = bottomSheetContent.CreateContent();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Label>(result);
        Assert.Equal("Template Content", ((Label)result).Text);
        Assert.Equal(bindingContext, result.BindingContext);
        Assert.Equal(parentElement, result.Parent);
        Assert.Equal(result, bottomSheetContent.Content); // Template content should be assigned to Content property
    }

    [Fact]
    public void CreateContent_WithBothContentAndTemplate_ShouldPrioritizeTemplate()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var directContent = new Label { Text = "Direct Content" };
        var template = new DataTemplate(() => new Label { Text = "Template Content" });

        bottomSheetContent.Content = directContent;
        bottomSheetContent.ContentTemplate = template;

        // Act
        var result = bottomSheetContent.CreateContent();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Label>(result);
        Assert.Equal("Template Content", ((Label)result).Text);
        Assert.NotEqual(directContent, result);
    }

    [Fact]
    public void CreateContent_WithNeitherContentNorTemplate_ShouldThrowException()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();

        // Act & Assert
        var exception = Assert.Throws<BottomSheetContentNotSetException>(() => bottomSheetContent.CreateContent());
        Assert.Contains("Content must be set before creating content", exception.Message);
    }

    [Fact]
    public void CreateContent_WithNullTemplateResult_ShouldThrowException()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var template = new DataTemplate(() => null); // Template returns null

        bottomSheetContent.ContentTemplate = template;

        // Act & Assert
        var exception = Assert.Throws<BottomSheetContentNotSetException>(() => bottomSheetContent.CreateContent());
        Assert.Contains("Content must be set before creating content", exception.Message);
    }

    [Fact]
    public void CreateContent_WithTemplateReturningNonView_ShouldThrowException()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var template = new DataTemplate(() => "Not a View"); // Template returns non-View object

        bottomSheetContent.ContentTemplate = template;

        // Act & Assert
        var exception = Assert.Throws<BottomSheetContentNotSetException>(() => bottomSheetContent.CreateContent());
        Assert.Contains("Content must be set before creating content", exception.Message);
    }

    [Fact]
    public void CreateContent_MultipleCallsWithSameContent_ShouldReturnSameInstance()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var content = new Label { Text = "Test Content" };
        bottomSheetContent.Content = content;

        // Act
        var result1 = bottomSheetContent.CreateContent();
        var result2 = bottomSheetContent.CreateContent();

        // Assert
        Assert.Same(result1, result2);
        Assert.Same(content, result1);
    }

    [Fact]
    public void CreateContent_ShouldPreserveContentProperties()
    {
        // Arrange
        var bottomSheetContent = new BottomSheetContent();
        var content = new Label 
        { 
            Text = "Test Content",
            BackgroundColor = Colors.Red,
            TextColor = Colors.Blue
        };
        bottomSheetContent.Content = content;

        // Act
        var result = bottomSheetContent.CreateContent();

        // Assert
        Assert.Equal("Test Content", ((Label)result).Text);
        Assert.Equal(Colors.Red, result.BackgroundColor);
        Assert.Equal(Colors.Blue, ((Label)result).TextColor);
    }

    [Fact]
    public void BindableProperties_ShouldBeCorrectlyDefined()
    {
        // Assert
        Assert.NotNull(BottomSheetContent.ContentProperty);
        Assert.NotNull(BottomSheetContent.ContentTemplateProperty);
        Assert.Equal(nameof(BottomSheetContent.Content), BottomSheetContent.ContentProperty.PropertyName);
        Assert.Equal(nameof(BottomSheetContent.ContentTemplate), BottomSheetContent.ContentTemplateProperty.PropertyName);
        Assert.Equal(typeof(View), BottomSheetContent.ContentProperty.ReturnType);
        Assert.Equal(typeof(DataTemplate), BottomSheetContent.ContentTemplateProperty.ReturnType);
    }
}