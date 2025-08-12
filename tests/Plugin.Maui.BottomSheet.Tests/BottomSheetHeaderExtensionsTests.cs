namespace Plugin.Maui.BottomSheet.Tests;

/// <summary>
/// Unit tests for BottomSheetHeaderExtensions.
/// </summary>
public class BottomSheetHeaderExtensionsTests
{
    [Fact]
    public void HasTopLeftButton_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftButton_WhenHeaderAppearanceIsNotLeftOrBoth_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.RightButton,
            TopLeftButton = new Button(),
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftButton_WhenTopLeftButtonIsNull_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftButton,
            TopLeftButton = null,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftButton_WhenHasTopLeftCloseButton_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftButton,
            TopLeftButton = new Button(),
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopLeft,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftButton_WhenAllConditionsMet_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftButton,
            TopLeftButton = new Button(),
            ShowCloseButton = false,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTopLeftButton_WhenHeaderAppearanceIsLeftAndRightButton_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton,
            TopLeftButton = new Button(),
            ShowCloseButton = false,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTopLeftCloseButton_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasTopLeftCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftCloseButton_WhenShowCloseButtonIsFalse_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = false,
            CloseButtonPosition = CloseButtonPosition.TopLeft,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftCloseButton_WhenCloseButtonPositionIsNotTopLeft_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopRight,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopLeftCloseButton_WhenAllConditionsMet_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopLeft,
        };

        // Act
        var result = bottomSheetHeader.HasTopLeftCloseButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTopRightButton_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightButton_WhenHeaderAppearanceIsNotRightOrBoth_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftButton,
            TopRightButton = new Button(),
        };

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightButton_WhenTopRightButtonIsNull_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.RightButton,
            TopRightButton = null,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightButton_WhenHasTopRightCloseButton_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.RightButton,
            TopRightButton = new Button(),
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopRight,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightButton_WhenAllConditionsMet_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.RightButton,
            TopRightButton = new Button(),
            ShowCloseButton = false,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTopRightButton_WhenHeaderAppearanceIsLeftAndRightButton_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            HeaderAppearance = BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton,
            TopRightButton = new Button(),
            ShowCloseButton = false,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTopRightCloseButton_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasTopRightCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightCloseButton_WhenShowCloseButtonIsFalse_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = false,
            CloseButtonPosition = CloseButtonPosition.TopRight,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightCloseButton_WhenCloseButtonPositionIsNotTopRight_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopLeft,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightCloseButton();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTopRightCloseButton_WhenAllConditionsMet_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ShowCloseButton = true,
            CloseButtonPosition = CloseButtonPosition.TopRight,
        };

        // Act
        var result = bottomSheetHeader.HasTopRightCloseButton();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasTitle_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasTitle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTitle_WhenTitleTextIsNull_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            TitleText = null,
        };

        // Act
        var result = bottomSheetHeader.HasTitle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTitle_WhenTitleTextIsEmpty_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            TitleText = string.Empty,
        };

        // Act
        var result = bottomSheetHeader.HasTitle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTitle_WhenTitleTextIsWhitespace_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            TitleText = "   ",
        };

        // Act
        var result = bottomSheetHeader.HasTitle();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasTitle_WhenTitleTextHasValue_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            TitleText = "Test Title",
        };

        // Act
        var result = bottomSheetHeader.HasTitle();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasHeaderView_WhenBottomSheetHeaderIsNull_ReturnsFalse()
    {
        // Arrange
        BottomSheetHeader? bottomSheetHeader = null;

        // Act
        var result = bottomSheetHeader.HasHeaderView();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasHeaderView_WhenBothHeaderDataTemplateAndContentAreNull_ReturnsFalse()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ContentTemplate = null,
            Content = null,
        };

        // Act
        var result = bottomSheetHeader.HasHeaderView();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasHeaderView_WhenHeaderDataTemplateIsNotNull_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ContentTemplate = new DataTemplate(() => new Label()),
            Content = null,
        };

        // Act
        var result = bottomSheetHeader.HasHeaderView();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasHeaderView_WhenContentIsNotNull_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ContentTemplate = null,
            Content = new Label(),
        };

        // Act
        var result = bottomSheetHeader.HasHeaderView();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasHeaderView_WhenBothHeaderDataTemplateAndContentAreNotNull_ReturnsTrue()
    {
        // Arrange
        var bottomSheetHeader = new BottomSheetHeader
        {
            ContentTemplate = new DataTemplate(() => new Label()),
            Content = new Label(),
        };

        // Act
        var result = bottomSheetHeader.HasHeaderView();

        // Assert
        Assert.True(result);
    }
}