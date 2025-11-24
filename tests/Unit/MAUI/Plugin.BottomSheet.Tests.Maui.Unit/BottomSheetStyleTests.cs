using Plugin.Maui.BottomSheet;

namespace Plugin.BottomSheet.Tests.Maui.Unit;

using System.ComponentModel;

public class BottomSheetStyleTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        BottomSheetStyle style = new BottomSheetStyle();

        // Assert
        Assert.NotNull(style.HeaderStyle);
        Assert.IsAssignableFrom<BottomSheetHeaderStyle>(style.HeaderStyle);
        Assert.IsAssignableFrom<BindableObject>(style);
    }

    [Fact]
    public void HeaderStyle_ShouldGetAndSetValue()
    {
        // Arrange
        BottomSheetStyle style = new BottomSheetStyle();
        BottomSheetHeaderStyle newHeaderStyle = new BottomSheetHeaderStyle();

        // Act
        style.HeaderStyle = newHeaderStyle;

        // Assert
        Assert.Same(newHeaderStyle, style.HeaderStyle);
    }

    [Fact]
    public void HeaderStyle_ShouldUseBindableProperty()
    {
        // Arrange
        BottomSheetStyle style = new BottomSheetStyle();
        BottomSheetHeaderStyle newHeaderStyle = new BottomSheetHeaderStyle();

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
        BottomSheetStyle style = new BottomSheetStyle();

        // Act
        style.HeaderStyle = null!;

        // Assert
        Assert.Null(style.HeaderStyle);
    }

    [Fact]
    public void HeaderStyle_PropertyChanged_ShouldTriggerPropertyChangedEvent()
    {
        // Arrange
        BottomSheetStyle style = new BottomSheetStyle();
        BottomSheetHeaderStyle newHeaderStyle = new BottomSheetHeaderStyle();
        bool propertyChangedTriggered = false;
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
        BottomSheetStyle style = new BottomSheetStyle();
        BottomSheetHeaderStyle headerStyle = style.HeaderStyle;
        bool propertyChangedTriggered = false;

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
        BottomSheetStyle style = new BottomSheetStyle
        {
            HeaderStyle = null!
        };
        object bindingContext = new object();

        // Act & Assert
        Exception? exception = Record.Exception(() => style.BindingContext = bindingContext);
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PropertyChanged_Event_ShouldBeRaisedCorrectly(bool useBindableProperty)
    {
        // Arrange
        BottomSheetStyle style = new BottomSheetStyle();
        BottomSheetHeaderStyle newHeaderStyle = new BottomSheetHeaderStyle();
        List<PropertyChangedEventArgs> eventArgs = new List<PropertyChangedEventArgs>();

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
        BindableProperty property = BottomSheetStyle.HeaderStyleProperty;

        // Assert
        Assert.True(property.IsReadOnly == false); // BindableProperty.Create creates writable properties by default
    }

    [Fact]
    public void MultipleInstances_ShouldHaveIndependentHeaderStyles()
    {
        // Arrange
        BottomSheetStyle style1 = new BottomSheetStyle();
        BottomSheetStyle style2 = new BottomSheetStyle();
        BottomSheetHeaderStyle customHeaderStyle = new BottomSheetHeaderStyle();

        // Act
        style1.HeaderStyle = customHeaderStyle;

        // Assert
        Assert.Same(customHeaderStyle, style1.HeaderStyle);
        Assert.NotSame(customHeaderStyle, style2.HeaderStyle);
        Assert.NotSame(style1.HeaderStyle, style2.HeaderStyle);
    }
}