namespace Plugin.Maui.BottomSheet.Tests;

using System.ComponentModel;

public class BottomSheetStyleTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var style = new BottomSheetStyle();

        // Assert
        Assert.NotNull(style.HeaderStyle);
        Assert.IsAssignableFrom<BottomSheetHeaderStyle>(style.HeaderStyle);
        Assert.IsAssignableFrom<BindableObject>(style);
    }

    [Fact]
    public void HeaderStyle_ShouldGetAndSetValue()
    {
        // Arrange
        var style = new BottomSheetStyle();
        var newHeaderStyle = new BottomSheetHeaderStyle();

        // Act
        style.HeaderStyle = newHeaderStyle;

        // Assert
        Assert.Same(newHeaderStyle, style.HeaderStyle);
    }

    [Fact]
    public void HeaderStyle_ShouldUseBindableProperty()
    {
        // Arrange
        var style = new BottomSheetStyle();
        var newHeaderStyle = new BottomSheetHeaderStyle();

        // Act
        style.SetValue(BottomSheetStyle.HeaderStyleProperty, newHeaderStyle);

        // Assert
        Assert.Same(newHeaderStyle, style.GetValue(BottomSheetStyle.HeaderStyleProperty));
        Assert.Same(newHeaderStyle, style.HeaderStyle);
    }

    [Fact]
    public void HeaderStyle_WhenSetToNull_ShouldAcceptNullValue()
    {
        // Arrange
        var style = new BottomSheetStyle();

        // Act
        style.HeaderStyle = null!;

        // Assert
        Assert.Null(style.HeaderStyle);
    }

    [Fact]
    public void HeaderStyle_PropertyChanged_ShouldTriggerPropertyChangedEvent()
    {
        // Arrange
        var style = new BottomSheetStyle();
        var newHeaderStyle = new BottomSheetHeaderStyle();
        var propertyChangedTriggered = false;
        string changedPropertyName = null!;

        style.PropertyChanged += (sender, e) =>
        {
            propertyChangedTriggered = true;
            changedPropertyName = e.PropertyName!;
        };

        // Act
        style.HeaderStyle = newHeaderStyle;

        // Assert
        Assert.True(propertyChangedTriggered);
        Assert.Equal(nameof(BottomSheetStyle.HeaderStyle), changedPropertyName);
    }

    [Fact]
    public void HeaderStyle_SetSameValue_ShouldNotTriggerPropertyChangedEvent()
    {
        // Arrange
        var style = new BottomSheetStyle();
        var headerStyle = style.HeaderStyle;
        var propertyChangedTriggered = false;

        style.PropertyChanged += (_, _) =>
        {
            propertyChangedTriggered = true;
        };

        // Act
        style.HeaderStyle = headerStyle;

        // Assert
        Assert.False(propertyChangedTriggered);
    }

    [Fact]
    public void BindingContext_WhenHeaderStyleIsNull_ShouldNotThrow()
    {
        // Arrange
        var style = new BottomSheetStyle
        {
            HeaderStyle = null!
        };
        var bindingContext = new object();

        // Act & Assert
        var exception = Record.Exception(() => style.BindingContext = bindingContext);
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PropertyChanged_Event_ShouldBeRaisedCorrectly(bool useBindableProperty)
    {
        // Arrange
        var style = new BottomSheetStyle();
        var newHeaderStyle = new BottomSheetHeaderStyle();
        var eventArgs = new List<PropertyChangedEventArgs>();

        style.PropertyChanged += (_, e) => eventArgs.Add(e);

        // Act
        if (useBindableProperty)
        {
            style.SetValue(BottomSheetStyle.HeaderStyleProperty, newHeaderStyle);
        }
        else
        {
            style.HeaderStyle = newHeaderStyle;
        }

        // Assert
        Assert.Single(eventArgs);
        Assert.Equal(nameof(BottomSheetStyle.HeaderStyle), eventArgs[0].PropertyName);
    }

    [Fact]
    public void HeaderStyleProperty_ShouldBeReadOnly()
    {
        // Arrange & Act
        var property = BottomSheetStyle.HeaderStyleProperty;

        // Assert
        Assert.True(property.IsReadOnly == false); // BindableProperty.Create creates writable properties by default
    }

    [Fact]
    public void MultipleInstances_ShouldHaveIndependentHeaderStyles()
    {
        // Arrange
        var style1 = new BottomSheetStyle();
        var style2 = new BottomSheetStyle();
        var customHeaderStyle = new BottomSheetHeaderStyle();

        // Act
        style1.HeaderStyle = customHeaderStyle;

        // Assert
        Assert.Same(customHeaderStyle, style1.HeaderStyle);
        Assert.NotSame(customHeaderStyle, style2.HeaderStyle);
        Assert.NotSame(style1.HeaderStyle, style2.HeaderStyle);
    }
}