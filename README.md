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
<img src="screenshots/iOS/Showcase.gif" />
<br>

`iPad`
<img src="screenshots/iPad/Showcase.gif" />
<br>

`MacCatalyst` [implementation details](https://developer.apple.com/design/human-interface-guidelines/sheets)
<img src="screenshots/MacCatalyst/Showcase.gif" />
<br>

`Android` at least API 21
<img src="screenshots/Android/Showcase.gif" />
<img src="screenshots/Android/Tablet/Showcase.gif" />

# Setup

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

| Type                                        | Name | Description                                                                    |
|---------------------------------------------|-----|--------------------------------------------------------------------------------|
| bool                                        | IsCancelable    | Can be closed by user either through gestures or clicking in background        |
| bool                                        | HasHandle    | Show handle                                                                    |
| bool                                        | ShowHeader    | Show header                                                                    |
| bool                                        | IsOpen    | Open or close                                                                  |
| bool                                        | IsDraggable    | Can be dragged(Useful if drawing gestures are made inside bottom sheet)        |
| List<[BottomSheetState](#bottomSheetState)> | States    | Allowed states. CurrentState must be a value of this collection.               |
| [BottomSheetState](#bottomSheetState)       | CurrentState    | Current state                                                                  |
| [BottomSheetHeader](#bottomSheetHeader)     | Header    | Configure header                                                               |
| [BottomSheetPeek](#bottomSheetPeek)         | Peek    | Configure peek(requieres at least iOS 16 -- all other platforms are supported) |
| [BottomSheetContent](#bottomSheetContent)   | Content    | Configure content                                                              |
| double                                      | Padding    | Padding                                                                        |
| Colors                                      | BackgroundColor    | Background color                                                               |
| bool                                        | IgnoreSafeArea    | Ignore safe area(currently only implemented in iOS)                            |

### BottomSheetState
| Name   | Description        |
|--------|--------------------|
| Peek   | Fractional height  |
| Medium | Half screen height |
| Large  | Full screen height |

### BottomSheetHeader
| Type                                                                            | Name           | Description                                                                                |
|---------------------------------------------------------------------------------|----------------|--------------------------------------------------------------------------------------------|
| string                                                                          | TitleText      | Title text                                                                                 |
| Button                                                                          | TopLeftButton  | Top left button                                                                            |
| Button                                                                          | TopRightButton | Top right button                                                                           |
| DataTemplate                                                                    | HeaderDataTemplate | Custom view. If set HeaderAppearance, TitleText and TopLeft-and Right buttons are ignored. |
| [BottomSheetHeaderButtonAppearanceMode](#bottomSheetHeaderButtonAppearanceMode) | HeaderAppearance | Set wich buttons should be displayed.                                                      |

### BottomSheetHeaderButtonAppearanceMode
| Name | Description         |
|------|---------------------|
| None | Don't show a button |
| LeftAndRightButton | Show a button on the left and right |
| LeftButton | Show a button on the left |
| RightButton | Show a button on the right |

### BottomSheetPeek
| Type   | Name | Description                                                              |
|--------|------|--------------------------------------------------------------------------|
| double | PeekHeight | Fixed peek detent height                                                 |
| DataTemplate | PeekViewDataTemplate | Peek view. Height will be calculated automatically if PeekHeight is NaN. |

### BottomSheetContent
| Type   | Name | Description   |
|--------|------|---------------|
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

# XAML usage

In order to make use of sheet within XAML you can use this namespace:

'xmlns:bottomsheet="clr-namespace:Maui.BottomSheet;assembly=Maui.BottomSheet"'
<br> <strong>or</strong> <br>
xmlns:bottomsheet="http://pluginmauibottomsheet.com"</strong>

`BottomSheet` is a `View` and can be added in any layout or control which accepts `View`.
To open/close a BottomSheet simply set `IsOpen` property to true/false. You can have <strong>multiple</strong> BottomSheets on one page.
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

`IBottomSheetNavigationService` is be registered automatically and can be resolved by `DI`. 

```
private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

public MainViewModel(IBottomSheetNavigationService bottomSheetNavigationService)
{
  _bottomSheetNavigationService = bottomSheetNavigationService;
}
```

To navigate to a `BottomSheet` you have to [register](https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection) `BottomSheets` and `ViewModels`
```
builder.Services.AddTransient<BottomSheetVMViewModel>();
builder.Services.AddTransient<BottomSheetGoBackViewModel>();

builder.Services.AddTransient<IBottomSheet, BottomSheetNoVM>();
builder.Services.AddTransient<IBottomSheet, BottomSheetVM>();
builder.Services.AddTransient<IBottomSheet, BottomSheetGoBack>();
```
Navigate to a `BottomSheet` and wire it automatically to specified `ViewModel` or navigate to a `BottomSheet` without a `ViewModel`

```
_bottomSheetNavigationService.NavigateTo<BottomSheetNoVM>();
_bottomSheetNavigationService.NavigateTo<BottomSheetVM, BottomSheetVMViewModel>();
```
To close a `BottomSheet` simply call `GoBack` or `ClearBottomSheetStack`(if you have multiple sheets open and want to close all of them)

You can pass parameters on each navigation(this follows principle of shell navigation)
Pass an instance of `BottomSheetNavigationParameters` to navigation and if target `ViewModel` implements `IQueryAttributable` parameters will be applied.

<img src="screenshots/navigation.gif" height="400" />

# BottomSheet Builder

<strong>Open any ContentPage or View as BottomSheet</strong>

Use `IBottomSheetBuilder` to build a `BottomSheet` from a `ContentPage` or `View`.
Use `IBottomSheetBuilderFactory` to create a new instance of `IBottomSheetBuilder`.
By default both instance types are registered and can be resolved by `DI`(<strong>may change in future if custom services are needed!</strong>)

## API

| Method    | Decription |
| --- | --- |
| FromView  | Build View as BottomSheet |
| FromContentPage | Build ContentPage as BottomSheet    |
| ConfigureBottomSheet    | Configure BottomSheet with known API    |
| WithParameters    | Create NavigationParameters    |
| WireTo    | Wire BottomSheet to a specified ViewModel    |
| TryAutoWire    | Set BindingContext of ContentPage as BindingContext of BottomSheet   |
| Open    | Open built BottomSheet  |

## Usage
```
public MainViewModel(IBottomSheetBuilderFactory bottomSheetBuilderFactory)
{
    _bottomSheetBuilderFactory = bottomSheetBuilderFactory;
}
```

```
private void OpenContentPageAsBottomSheet()
{
    _bottomSheetBuilderFactory.Create()
	.FromContentPage<NewPageA>()
	.ConfigureBottomSheet((sheet) =>
	{
	    sheet.SheetStates = BottomSheetState.All;
	})
	.WireTo<NewPageAViewModel>()
	.Open();
}
```

```
private void OpenViewAsBottomSheet()
{
    _bottomSheetBuilderFactory.Create()
        .FromView<ContentA>()
        .ConfigureBottomSheet((sheet) =>
        {
            sheet.SheetStates = BottomSheetState.Medium;
        })
        .WireTo<ContentAViewModel>()
        .Open();
}
```

