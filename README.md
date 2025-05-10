# Plugin.Maui.BottomSheet
<strong>Show native BottomSheets with .NET MAUI!</strong>
* Built-in NavigationService
* Open any ContentPage or View as BottomSheet
* Create BottomSheets in any layout
* Configurable header
* MVVM support
<br>

# Samples

<strong>Check out sample project to see the API in action!</strong>.

## Platforms

`iOS` at least iOS 15
<br>
<img src="screenshots/iOS/Showcase.gif" />
<br>

`iPad`
<br>
<img src="screenshots/iPad/Showcase.gif" />
<br>

`MacCatalyst` [implementation details](https://developer.apple.com/design/human-interface-guidelines/sheets)
<br>
<img src="screenshots/MacCatalyst/Showcase.gif" />
<br>

`Android` at least API 23
<br>
<img src="screenshots/Android/Showcase.gif" />
<img src="screenshots/Android/Tablet/Showcase.gif" />

# Setup

Install package [Plugin.Maui.BottomSheet](https://www.nuget.org/packages/Plugin.Maui.BottomSheet/9.0.1-pre)
Enable this plugin by calling `UseBottomSheet()` in your `MauiProgram.cs`

```cs
var builder = MauiApp.CreateBuilder();
builder
  .UseMauiApp<App>()
  .UseMauiBottomSheet()
  .ConfigureFonts(fonts =>
  {
    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
  })
  .RegisterPages()
  .RegisterViewModels()
  .PlatformServices();

#if DEBUG
builder.Logging.AddDebug();
#endif

return builder.Build();
```

# API

| Type                                        | Name                  | Description                                                                                                                                                                                                              |
|---------------------------------------------|-----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| bool                                        | IsModal               | Is interaction with content under BottomSheet enabled                                                                                                                                                                    |
| bool                                        | IsCancelable          | Can be closed by user either through gestures or clicking in background                                                                                                                                                  |
| bool                                        | HasHandle             | Show handle                                                                                                                                                                                                              |
| bool                                        | ShowHeader            | Show header                                                                                                                                                                                                              |
| bool                                        | IsOpen                | Open or close                                                                                                                                                                                                            |
| bool                                        | IsDraggable           | Can be dragged(Useful if drawing gestures are made inside bottom sheet)                                                                                                                                                  |
| List<[BottomSheetState](#bottomSheetState)> | States                | Allowed states. CurrentState must be a value of this collection.                                                                                                                                                         |
| [BottomSheetState](#bottomSheetState)       | CurrentState          | Current state                                                                                                                                                                                                            |
| [BottomSheetHeader](#bottomSheetHeader)     | Header                | Configure header                                                                                                                                                                                                         |
| double                                      | PeekHeight            | Set peek height(requires at least iOS 16 -- all other platforms are supported). The header height will be added to the `PeekHeight` internally. Use `BottomSheetPeekBehavior` to calculate the height based on a `View`. |
| [BottomSheetContent](#bottomSheetContent)   | Content               | Configure content                                                                                                                                                                                                        |
| double                                      | Padding               | Padding                                                                                                                                                                                                                  |
| Colors                                      | BackgroundColor       | Background color                                                                                                                                                                                                         |
| bool                                        | IgnoreSafeArea        | Ignore safe area(currently only implemented in iOS)                                                                                                                                                                      |
| float                                       | CornerRadius          | Top left and top right corner radius                                                                                                                                                                                     |
| [BottomSheetStyle](#bottomSheetStyle)       | BottomSheetStyle      | Style built in components                                                                                                                                                                                                |
| Color                                       | WindowBackgroundColor | Window background color. If BottomSheet is non-modal no window color is applied                                                                                                                                          |

### BottomSheetState
| Name   | Description        |
|--------|--------------------|
| Peek   | Fractional height  |
| Medium | Half screen height |
| Large  | Full screen height |

### BottomSheetHeader
| Type                                                                            | Name                | Description                                                                                          |
|---------------------------------------------------------------------------------|---------------------|------------------------------------------------------------------------------------------------------|
| string                                                                          | TitleText           | Title text                                                                                           |
| Button                                                                          | TopLeftButton       | Top left button                                                                                      |
| Button                                                                          | TopRightButton      | Top right button                                                                                     |
| DataTemplate                                                                    | HeaderDataTemplate  | Custom view. If set HeaderAppearance, TitleText and TopLeft-and Right buttons are ignored.           |
| [BottomSheetHeaderButtonAppearanceMode](#bottomSheetHeaderButtonAppearanceMode) | HeaderAppearance    | Set which buttons should be displayed.                                                               |
| bool                                                                            | ShowCloseButton     | Built in button to close the BottomSheet. Build in Button will replace top right or top left button. |
| [CloseButtonPosition](#closeButtonPosition)                                     | CloseButtonPosition | Show button on the left or right                                                                     |

### BottomSheetHeaderButtonAppearanceMode
| Name               | Description                         |
|--------------------|-------------------------------------|
| None               | Don't show a button                 |
| LeftAndRightButton | Show a button on the left and right |
| LeftButton         | Show a button on the left           |
| RightButton        | Show a button on the right          |

### CloseButtonPosition
| Name  | Description              |
|-------|--------------------------|
| Left  | Show button on the left  |
| Right | Show button on the right |

### BottomSheetContent
| Type         | Name            | Description                                                                       |
|--------------|-----------------|-----------------------------------------------------------------------------------|
| View         | Content         | View content. Xaml usage can be simplified as ContentProperty attribute is added. |
| DataTemplate | ContentTemplate | View template will be inflated when the BottomSheet is opened.                    |

> [!CAUTION]
> Be careful when using `Content` because the Content will be created even if the BottomSheet isn't open and this may have a negative performance impact.
> `Content` should only be used with navigation and not in `BottomSheets` added directly to a `Layout`.

### BottomSheetStyle
| Type                                              | Name        | Description |
|---------------------------------------------------|-------------|-------------|
| [BottomSheetHeaderStyle](#bottomSheetHeaderStyle) | HeaderStyle | Style class |

### BottomSheetHeaderStyle
| Type           | Name                            | Description                          |
|----------------|---------------------------------|--------------------------------------|
| Color          | TitleTextColor                  | Title text color                     |
| double         | TitleTextFontSize               | Title text font size                 |
| FontAttributes | TitleTextFontAttributes         | Title text font attributes           |
| string         | TitleTextFontFamily             | Title text font family               |
| bool           | TitleTextFontAutoScalingEnabled | Title text font auto scaling enabled |
| double         | CloseButtonHeightRequest        | Close button height request          |
| double         | CloseButtonWidthRequest         | Close button width request           |
| Color          | CloseButtonTintColor            | Close button tint color              |

## Interaction

#### Commands
`TopRightButtonCommand` `TopRightButtonCommandParameter`

`TopLeftButtonCommand` `TopLeftButtonCommandParameter`

`ClosingCommand` `ClosingCommandParameter`

`ClosedCommand` `ClosedCommandParameter`

`OpeningCommand` `OpeningCommandParameter`

`OpenedCommand` `OpenedCommandParameter`

#### Events
`Closing`
`Closed`
`Opening`
`Opened`

# Platform specifics

## MacCatalyst
By design sheets are always [modal](https://developer.apple.com/design/human-interface-guidelines/sheets#macOS).

## [Android](https://developer.android.com/reference/com/google/android/material/bottomsheet/BottomSheetDialog#BottomSheetDialog(android.content.Context))

#### Themes
`BottomSheets` are themed by default based on `ThemeOverlay.MaterialComponents.BottomSheetDialog`.
To enable EdgeToEdge by default the derived theme sets the `navigationBarColor` to transparent.
You can create you own themes and apply them to different `BottomSheets`.

To set a custom theme call the platform specific extension method before the sheet is opened.
```
MyBottomSheet.On<Android>().SetTheme(Resource.Style.My_Awesome_BottomSheetDialog)
```

#### Edge to edge
EdgeToEdge support is built-in and enabled by default. 
If you create your own theme make sure to derive from `ThemeOverlay.MaterialComponents.BottomSheetDialog` and that `navigationBarColor` is translucent. 
Otherwise, EdgeToEdge is disabled for that sheet. To disable EdgeToEdge you can also set `<item name="enableEdgeToEdge">false</item>` in your theme.

### [MaxHeight](https://developer.android.com/reference/com/google/android/material/bottomsheet/BottomSheetBehavior#setMaxHeight(int)) and [MaxWidth](https://developer.android.com/reference/com/google/android/material/bottomsheet/BottomSheetBehavior#setMaxWidth(int))
To override the MaxHeight or MaxWidth call the platform specific extension method before the sheet is opened.

```
MyBottomSheet.On<Android>().SetMaxHeight(MaxValue);
MyBottomSheet.On<Android>().SetMaxWidth(MaxValue);

or

xmlns:androidBottomsheet="http://pluginmauibottomsheet.com/platformconfiguration/android"
androidBottomsheet:BottomSheet.MaxWidth="300"
```

### [Margin](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/align-position?view=net-maui-9.0#position-controls)

Set the BottomSheet margin.
The margin will only be applied on the left on right.

```
MyBottomSheet.On<Android>().SetMargin(new Thickness(10, 0, 10, 0));

or

xmlns:androidBottomsheet="http://pluginmauibottomsheet.com/platformconfiguration/android"
androidBottomsheet:BottomSheet.Margin="10,0,10,0"
```

### [HalfExpandedRatio](https://developer.android.com/reference/com/google/android/material/bottomsheet/BottomSheetBehavior#setHalfExpandedRatio(float))
```
MyBottomSheet.On<Android>().SetHalfExpanedRatio = 0.8f;

or

xmlns:androidBottomsheet="http://pluginmauibottomsheet.com/platformconfiguration/android"
androidBottomsheet:BottomSheet.HalfExpandedRatio="0.8"
```
# Style

With `BottomSheet.BottomSheetStyle` built in components as e.g. `BottomSheetHeader.Title` or the Close button can be styled.
You can either style each BottomSheet individually or use [styles](https://learn.microsoft.com/dotnet/maui/user-interface/styles/xaml?view=net-maui-9.0).

```
<bottomsheet:BottomSheet>
    <bottomsheet:BottomSheet.BottomSheetStyle>
        <bottomsheet:BottomSheetStyle>
            <bottomsheet:BottomSheetStyle.HeaderStyle>
                <bottomsheet:BottomSheetHeaderStyle TitleTextColor="Aqua" CloseButtonTintColor="Aqua"/>
            </bottomsheet:BottomSheetStyle.HeaderStyle>
        </bottomsheet:BottomSheetStyle>
    </bottomsheet:BottomSheet.BottomSheetStyle>
</bottomsheet:BottomSheet>
```

```
<Style TargetType="bottomsheet:BottomSheet">
    <Setter Property="BottomSheetStyle">
        <Setter.Value>
            <bottomsheet:BottomSheetStyle>
                <bottomsheet:BottomSheetStyle.HeaderStyle>
                    <bottomsheet:BottomSheetHeaderStyle
                        CloseButtonHeightRequest="40"
                        CloseButtonTintColor="LightBlue"
                        CloseButtonWidthRequest="40"
                        TitleTextColor="Blue"
                        TitleTextFontAttributes="Bold"
                        TitleTextFontAutoScalingEnabled="True"
                        TitleTextFontFamily="OpenSansRegular"
                        TitleTextFontSize="20" />
                </bottomsheet:BottomSheetStyle.HeaderStyle>
            </bottomsheet:BottomSheetStyle>
        </Setter.Value>
    </Setter>
</Style>
```

# XAML usage

In order to make use of sheet within XAML you can use this namespace:
xmlns:bottomsheet="http://pluginmauibottomsheet.com"

`BottomSheet` is a `View` and can be added in any layout or control which accepts `View`.

To open/close a `BottomSheet` simply set `IsOpen` property to true/false.

```
<bottomsheet:BottomSheet
    x:Name="ModalBottomSheet"
    Padding="20"
    CornerRadius="{Binding CornerRadius}"
    HasHandle="{Binding HasHandle}"
    IgnoreSafeArea="True"
    IsCancelable="{Binding IsCancelable}"
    IsDraggable="{Binding IsDraggable}"
    IsModal="{Binding IsModal, Mode=TwoWay}"
    IsOpen="{Binding IsOpen}"
    ShowHeader="{Binding ShowHeader}"
    States="Peek,Medium,Large"
    WindowBackgroundColor="{Binding WindowBackgroundColor, Mode=TwoWay}">
    <bottomsheet:BottomSheet.Header>
        <bottomsheet:BottomSheetHeader
            CloseButtonPosition="{Binding CloseButtonPosition}"
            HeaderAppearance="{Binding HeaderButtonAppearanceMode}"
            ShowCloseButton="{Binding ShowCloseButton}"
            TitleText="{Binding Title}">
            <bottomsheet:BottomSheetHeader.TopLeftButton>
                <Button Command="{Binding TopLefButtonCommand}" Text="Top left" />
            </bottomsheet:BottomSheetHeader.TopLeftButton>
            <bottomsheet:BottomSheetHeader.TopRightButton>
                <Button Command="{Binding TopRightButtonCommand}" Text="Top right" />
            </bottomsheet:BottomSheetHeader.TopRightButton>
        </bottomsheet:BottomSheetHeader>
    </bottomsheet:BottomSheet.Header>
    <bottomsheet:BottomSheet.Content>
        <bottomsheet:BottomSheetContent>
            <bottomsheet:BottomSheetContent.ContentTemplate>
                <DataTemplate x:DataType="local:ShowCaseViewModel">
                    <VerticalStackLayout>
                        <ContentView>
                            <ContentView.Behaviors>
                                <bottomsheet:BottomSheetPeekBehavior />
                            </ContentView.Behaviors>
                            <Label Text="PeekHeight reference view " />
                        </ContentView>
                        <Grid>
                            <Label Text="Some other view" />
                        </Grid>
                    </VerticalStackLayout>
                </DataTemplate>
            </bottomsheet:BottomSheetContent.ContentTemplate>
        </bottomsheet:BottomSheetContent>
    </bottomsheet:BottomSheet.Content>
</bottomsheet:BottomSheet>
```
To set the PeekHeight based on a view inside the `BottomSheetContent` attach the `BottomSheetPeekBehavior` to the view.
```
<bottomsheet:BottomSheet.Content>
    <bottomsheet:BottomSheetContent>
        <bottomsheet:BottomSheetContent.ContentTemplate>
            <DataTemplate x:DataType="local:ShowCaseViewModel">
                <VerticalStackLayout>
                    <ContentView>
                        <ContentView.Behaviors>
                            <bottomsheet:BottomSheetPeekBehavior />
                        </ContentView.Behaviors>
                        <Label Text="Peek content"/>
                    </ContentView>
                    <Grid
                        <Label Text="Main content"/>
                    </Grid>
                </VerticalStackLayout>
            </DataTemplate>
        </bottomsheet:BottomSheetContent.ContentTemplate>
    </bottomsheet:BottomSheetContent>
</bottomsheet:BottomSheet.Content>
```
The peek height will be equal to the `ContentView` height.

# Navigation Awareness

To enable the `ViewModel` of a `BottomSheet` to participate in the navigation lifecycle, implement the `INavigationAware` interface. This interface defines two lifecycle methods:

- OnNavigatedTo(IBottomSheetNavigationParameters parameters): Called when the BottomSheet is navigated to.
- OnNavigatedFrom(IBottomSheetNavigationParameters parameters): Called when navigating away from the BottomSheet.

Implementing these methods allows the `ViewModel` to handle initialization, cleanup, or state persistence tasks based on the navigation context.
Additionally, any `BottomSheetNavigationParameters` passed during navigation will be available through the parameters argument, enabling the `ViewModel` to access incoming or outgoing data as needed.

> [!TIP]
> Not only can the BottomSheet’s `ViewModel` implement `INavigationAware`, but so can the `ViewModel` of the parent view that launched the `BottomSheet`. This allows the parent to respond when:
> - The first BottomSheet is opened (OnNavigatedFrom)
> - The last BottomSheet is closed (OnNavigatedTo)
>
> This is especially useful for suspending or restoring state in the parent view while modals are active.

> [!IMPORTANT]  
> When using the `IsOpen` to manage the visibility of a `BottomSheet`, the parent `ViewModel` can implement `INavigationAware` to respond to the lifecycle of the `BottomSheet`.
> This allows the parent `ViewModel` to handle actions when:

| Scenario                                       | `INavigationAware` on Parent Triggered? |
|------------------------------------------------|-----------------------------------------|
| BottomSheet opened via navigation              | ❌ No                                    |
| BottomSheet opened without navigation (IsOpen) | ✅ Yes                                   |

```
public class UserViewModel : IConfirmNavigationAsync
{
    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
    }
}
```

# Confirming navigation

A `BottomSheet` or its associated `ViewModel` can determine whether a navigation operation is allowed by implementing either the `IConfirmNavigation` or `IConfirmNavigationAsync` interface.

When one of these interfaces is implemented, the navigation system checks the result of the confirmation method:

- If the method returns `true`, the navigation proceeds—this may result in the current `BottomSheet` being closed or another `BottomSheet` being opened above it.
- If the method returns `false`, the navigation is canceled—the current `BottomSheet` remains open, and no new one is shown.

For `BottomSheets` that are added directly to a layout (not navigated to), the navigation system checks whether `BottomSheet.Parent` or its `ViewModel` implements `IConfirmNavigation` or `IConfirmNavigationAsync`.

> **Note:** `IBottomSheetNavigationParameters` are only passed when a `BottomSheet` is shown or closed via navigation. If a `BottomSheet` is closed with a gesture (such as swipe down).

```
public class UserViewModel : IConfirmNavigationAsync
{
    public Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters? parameters)
    {
        return Shell.Current.CurrentPage.DisplayAlert(
            "Warning",
            "Would you like to save?",
            "Yes",
            "No");
    }
}

public class UserViewModel : IConfirmNavigation
{
    public Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters? parameters)
    {
        return true;
    }
}
```

# Navigation

> [!CAUTION]
> Never mix navigation with `BottomSheet.IsOpen`. This will lead to unexpected behavior and is not supported.
> Either open and close `BottomSheet` using `IsOpen` or use navigation.

`IBottomSheetNavigationService` is be registered automatically and can be resolved. 

```
private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

public MainViewModel(IBottomSheetNavigationService bottomSheetNavigationService)
{
  _bottomSheetNavigationService = bottomSheetNavigationService;
}
```

Register named `BottomSheets` for navigation

```
builder.Services.AddBottomSheet<ShowCasePage>("Showcase");
```

Navigate to the registered `BottomSheet`

```
_bottomSheetNavigationService.NavigateTo("Showcase");
```

Close a top most `BottomSheet`
```
_bottomSheetNavigationService.GoBack();
```

Close all open `BottomSheets`(Last In - First Out)
```
_bottomSheetNavigationService.ClearBottomSheetStack();
```

By default `ShowCasePage.BindingContext` will be assigned to `BottomSheet.BindingContext` as in [Shell navigation](https://learn.microsoft.com/dotnet/architecture/maui/dependency-injection).

Wire your `BottomSheet` to a `ViewModel` to simplify navigation.

```
builder.Services.AddBottomSheet<SomeBottomSheet, SomeViewModel>("SomeBottomSheet");
```

To manually set the `ViewModel` it has to be available in the container
```
builder.Services.AddTransient<SomeViewModel>();

_bottomSheetNavigationService.NavigateTo<SomeViewModel>("Showcase");
```

If `CopyPagePropertiesToBottomSheet` is enabled all applicable properties will be copied from the source page to the `BottomSheet` during navigation.

```
.UseBottomSheet(config => config.CopyPagePropertiesToBottomSheet = true);
```

Add a default `BottomSheet` navigation configuration

```
builder.Services.AddBottomSheet<ShowCasePage>("Showcase",
    (sheet, page) =>
    {
        sheet.States = [BottomSheetState.Medium, BottomSheetState.Large];
        sheet.CurrentState = BottomSheetState.Large;
        sheet.ShowHeader = true;
        sheet.Header = new BottomSheetHeader()
        {
            TitleText = page.Title,
        };
    });
```
To override the default configuration
```
_bottomSheetNavigationService.NavigateTo("Showcase", configure: (sheet) =>
{
    sheet.Header.TitleText = "My new title";
});
```

You can pass parameters on each navigation as you are used to it from [Shell navigation](https://learn.microsoft.com/dotnet/maui/fundamentals/shell/navigation?view=net-maui-9.0#process-navigation-data-using-a-single-method).

```
_bottomSheetNavigationService.NavigateTo("Showcase", new BottomSheetNavigationParameters()
{
    ["SomeKey"] = "SomeValue",
});
```
