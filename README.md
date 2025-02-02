# Plugin.Maui.BottomSheet
<strong>Show native BottomSheets with .NET MAUI!</strong>
* Built-in NavigationService
* Open any ContenPage or View as BottomSheet
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

| Type                                        | Name                  | Description                                                                     |
|---------------------------------------------|-----------------------|---------------------------------------------------------------------------------|
| bool                                        | IsModal               | Is interaction with content under BottomSheet enabled                           |
| bool                                        | IsCancelable          | Can be closed by user either through gestures or clicking in background         |
| bool                                        | HasHandle             | Show handle                                                                     |
| bool                                        | ShowHeader            | Show header                                                                     |
| bool                                        | IsOpen                | Open or close                                                                   |
| bool                                        | IsDraggable           | Can be dragged(Useful if drawing gestures are made inside bottom sheet)         |
| List<[BottomSheetState](#bottomSheetState)> | States                | Allowed states. CurrentState must be a value of this collection.                |
| [BottomSheetState](#bottomSheetState)       | CurrentState          | Current state                                                                   |
| [BottomSheetHeader](#bottomSheetHeader)     | Header                | Configure header                                                                |
| [BottomSheetPeek](#bottomSheetPeek)         | Peek                  | Configure peek(requieres at least iOS 16 -- all other platforms are supported)  |
| [BottomSheetContent](#bottomSheetContent)   | Content               | Configure content                                                               |
| double                                      | Padding               | Padding                                                                         |
| Colors                                      | BackgroundColor       | Background color                                                                |
| bool                                        | IgnoreSafeArea        | Ignore safe area(currently only implemented in iOS)                             |
| float                                       | CornerRadius          | Top left and top right corner radius                                            |
| Color                                       | WindowBackgroundColor | Window background color. If BottomSheet is non-modal no window color is applied |

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

### BottomSheetPeek
| Type         | Name                 | Description                                                              |
|--------------|----------------------|--------------------------------------------------------------------------|
| double       | PeekHeight           | Fixed peek detent height                                                 |
| DataTemplate | PeekViewDataTemplate | Peek view. Height will be calculated automatically if PeekHeight is NaN. |

### BottomSheetContent
| Type         | Name            | Description   |
|--------------|-----------------|---------------|
| DataTemplate | ContentTemplate | Content view. |

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

## Android

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
Otherwise EdgeToEdge is disabled for that sheet. To disable EdgeToEdge you can also set `<item name="enableEdgeToEdge">false</item>` in your theme.

### MaxHeight and MaxWidth
To override the MaxHeight or MaxWidth call the platform specific extension method before the sheet is opened.

```
MyBottomSheet.On<Android>().SetMaxHeight(MaxValue);
MyBottomSheet.On<Android>().SetMaxWidth(MaxValue);

or

xmlns:androidBottomsheet="http://pluginmauibottomsheet.com/platformconfiguration/android"
androidBottomsheet:BottomSheet.MaxWidth="300"
```

### Margin

Set the [`BottomSheet` margin](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/align-position?view=net-maui-9.0#position-controls).
The margin will only be applied on the left on right.

```
MyBottomSheet.On<Android>().SetMargin(new Thickness(10, 0, 10, 0));

or

xmlns:androidBottomsheet="http://pluginmauibottomsheet.com/platformconfiguration/android"
androidBottomsheet:BottomSheet.Margin="10,0,10,0"
```

# XAML usage

In order to make use of sheet within XAML you can use this namespace:
xmlns:bottomsheet="http://pluginmauibottomsheet.com"

`BottomSheet` is a `View` and can be added in any layout or control which accepts `View`.

To open/close a `BottomSheet` simply set `IsOpen` property to true/false.

```
    <bottomsheet:BottomSheet
        Padding="20"
        IsOpen="{Binding IsOpen}"
        States="Peek,Medium,Large"
        HasHandle="{Binding HasHandle}"
        IsCancelable="{Binding IsCancelable}"
        ShowHeader="{Binding ShowHeader}"
        IsDraggable="{Binding IsDraggable}">
        <bottomsheet:BottomSheet.Header>
            <bottomsheet:BottomSheetHeader 
                TitleText="{Binding Title}"
                HeaderAppearance="{Binding HeaderButtonAppearanceMode}">
                <bottomsheet:BottomSheetHeader.TopLeftButton>
                    <Button Text="Top left"  Command="{Binding TopLefButtonCommand}"></Button>
                </bottomsheet:BottomSheetHeader.TopLeftButton>
                <bottomsheet:BottomSheetHeader.TopRightButton>
                    <Button Text="Top right" Command="{Binding TopRightButtonCommand}"></Button>
                </bottomsheet:BottomSheetHeader.TopRightButton>
            </bottomsheet:BottomSheetHeader>
        </bottomsheet:BottomSheet.Header>
        <bottomsheet:BottomSheet.Peek>
            <bottomsheet:BottomSheetPeek>
                <bottomsheet:BottomSheetPeek.PeekViewDataTemplate>
                    <DataTemplate x:DataType="local:ShowCaseViewModel">
                        <Grid Margin="0,10,0,10"
                            ColumnSpacing="10"
                            RowSpacing="10" 
                            RowDefinitions="40,40,40" 
                            ColumnDefinitions="*,*">
                            <Label VerticalTextAlignment="Center" Grid.Row="0" Text="Title"/>
                            <Entry Grid.Row="0" Grid.Column="1" Text="{Binding Title}"/>
                            <Button Grid.Row="1" Text="None" Command="{Binding HeaderButtonAppearanceModeNoneCommand}"></Button>
                            <Button Grid.Row="1" Grid.Column="1" Text="Left" Command="{Binding HeaderButtonAppearanceModeLeftCommand}"></Button>
                            <Button Grid.Row="2" Text="Right" Command="{Binding HeaderButtonAppearanceModeRightCommand}"></Button>
                            <Button Grid.Row="2" Grid.Column="1" Text="LeftAndRight" Command="{Binding HeaderButtonAppearanceModeLeftAndRightCommand}"></Button>
                        </Grid>
                    </DataTemplate>
                </bottomsheet:BottomSheetPeek.PeekViewDataTemplate>
            </bottomsheet:BottomSheetPeek>
        </bottomsheet:BottomSheet.Peek>
        <bottomsheet:BottomSheet.Content>
            <bottomsheet:BottomSheetContent>
                <bottomsheet:BottomSheetContent.ContentTemplate>
                    <DataTemplate x:DataType="local:ShowCaseViewModel">
                        <Grid RowDefinitions="40,40,40,40" RowSpacing="10" ColumnDefinitions="*, 50">
                            <Label Text="Has handle?"></Label>
                            <Label Grid.Row="1" Text="Is cancelable?"></Label>
                            <Label Grid.Row="2" Text="Show header?"></Label>
                            <Label Grid.Row="3" Text="Is draggable?"></Label>
                            <Switch Grid.Column="1" IsToggled="{Binding HasHandle}"></Switch>
                            <Switch Grid.Row="1" Grid.Column="1" IsToggled="{Binding IsCancelable}"></Switch>
                            <Switch Grid.Row="2" Grid.Column="1" IsToggled="{Binding ShowHeader}"></Switch>
                            <Switch Grid.Row="3" Grid.Column="1" IsToggled="{Binding IsDraggable}"></Switch>
                        </Grid>
                    </DataTemplate>
                </bottomsheet:BottomSheetContent.ContentTemplate>
            </bottomsheet:BottomSheetContent>
        </bottomsheet:BottomSheet.Content>
    </bottomsheet:BottomSheet>
```

# Navigation

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


