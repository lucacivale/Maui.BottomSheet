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

| Type                                        | Name                  | Description                                                                                                                                     |
|---------------------------------------------|-----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| bool                                        | IsModal               | Is interaction with content under BottomSheet enabled                                                                                           |
| bool                                        | IsCancelable          | Can be closed by user either through gestures or clicking in background                                                                         |
| bool                                        | HasHandle             | Show handle                                                                                                                                     |
| bool                                        | ShowHeader            | Show header                                                                                                                                     |
| bool                                        | IsOpen                | Open or close                                                                                                                                   |
| bool                                        | IsDraggable           | Can be dragged(Useful if drawing gestures are made inside bottom sheet)                                                                         |
| List<[BottomSheetState](#bottomSheetState)> | States                | Allowed states. CurrentState must be a value of this collection.                                                                                |
| [BottomSheetState](#bottomSheetState)       | CurrentState          | Current state                                                                                                                                   |
| [BottomSheetHeader](#bottomSheetHeader)     | Header                | Configure header                                                                                                                                |
| double                                      | PeekHeight            | Set peek height(requires at least iOS 16 -- all other platforms are supported). The header height will be added to the `PeekHeight` internally. |
| [BottomSheetContent](#bottomSheetContent)   | Content               | Configure content                                                                                                                               |
| double                                      | Padding               | Padding                                                                                                                                         |
| Colors                                      | BackgroundColor       | Background color                                                                                                                                |
| bool                                        | IgnoreSafeArea        | Ignore safe area(currently only implemented in iOS)                                                                                             |
| float                                       | CornerRadius          | Top left and top right corner radius                                                                                                            |
| Color                                       | WindowBackgroundColor | Window background color. If BottomSheet is non-modal no window color is applied                                                                 |

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
Otherwise, EdgeToEdge is disabled for that sheet. To disable EdgeToEdge you can also set `<item name="enableEdgeToEdge">false</item>` in your theme.

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
                            <Grid
                                Margin="0,10,0,10"
                                ColumnDefinitions="*,*"
                                ColumnSpacing="10"
                                RowDefinitions="40,40,40,40,40"
                                RowSpacing="10">
                                <Label
                                    Grid.Row="0"
                                    Text="Title"
                                    VerticalTextAlignment="Center" />
                                <Entry
                                    Grid.Row="0"
                                    Grid.Column="1"
                                    Text="{Binding Title}" />
                                <Button
                                    Grid.Row="1"
                                    Command="{Binding HeaderButtonAppearanceModeNoneCommand}"
                                    Text="None" />
                                <Button
                                    Grid.Row="1"
                                    Grid.Column="1"
                                    Command="{Binding HeaderButtonAppearanceModeLeftCommand}"
                                    Text="Left" />
                                <Button
                                    Grid.Row="2"
                                    Command="{Binding HeaderButtonAppearanceModeRightCommand}"
                                    Text="Right" />
                                <Button
                                    Grid.Row="2"
                                    Grid.Column="1"
                                    Command="{Binding HeaderButtonAppearanceModeLeftAndRightCommand}"
                                    Text="LeftAndRight" />
                                <Label
                                    Grid.Row="3"
                                    Text="Corner radius"
                                    VerticalOptions="Center" />
                                <Slider
                                    Grid.Row="3"
                                    Grid.Column="1"
                                    Maximum="50"
                                    Minimum="0"
                                    VerticalOptions="Center"
                                    Value="{Binding CornerRadius}" />
                                <Label
                                    Grid.Row="4"
                                    Text="Is modal?"
                                    VerticalOptions="Center" />
                                <Switch
                                    Grid.Row="4"
                                    Grid.Column="1"
                                    HorizontalOptions="End"
                                    IsToggled="{Binding IsModal}"
                                    VerticalOptions="Center" />
                            </Grid>
                        </ContentView>
                        <Grid
                            ColumnDefinitions="*, 50"
                            RowDefinitions="40,40,40,40,40,40,40"
                            RowSpacing="10">
                            <Label Text="Has handle?" />
                            <Label Grid.Row="1" Text="Is cancelable?" />
                            <Label Grid.Row="2" Text="Show header?" />
                            <Label Grid.Row="3" Text="Is draggable?" />
                            <Label Grid.Row="4" Text="Show close button?" />
                            <Switch Grid.Column="1" IsToggled="{Binding HasHandle}" />
                            <Switch
                                Grid.Row="1"
                                Grid.Column="1"
                                IsToggled="{Binding IsCancelable}" />
                            <Switch
                                Grid.Row="2"
                                Grid.Column="1"
                                IsToggled="{Binding ShowHeader}" />
                            <Switch
                                Grid.Row="3"
                                Grid.Column="1"
                                IsToggled="{Binding IsDraggable}" />
                            <Switch
                                Grid.Row="4"
                                Grid.Column="1"
                                IsToggled="{Binding ShowCloseButton}" />
                            <Grid
                                Grid.Row="5"
                                Grid.ColumnSpan="2"
                                ColumnDefinitions="*,*"
                                ColumnSpacing="10">
                                <Button Command="{Binding TopLeftCloseButtonCommand}" Text="Top left close button" />
                                <Button
                                    Grid.Column="1"
                                    Command="{Binding TopRightCloseButtonCommand}"
                                    Text="Top right close button" />
                            </Grid>
                            <Button
                                Grid.Row="6"
                                Grid.ColumnSpan="2"
                                Command="{Binding ChangeWindowBackgroundColorCommand}"
                                Text="Change window background color" />
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


