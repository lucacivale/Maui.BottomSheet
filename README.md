# Plugin.Maui.BottomSheet
<strong>Show native BottomSheets with .NET MAUI!</strong>
* Built-in NavigationService
* Open any ContenPage or View as BottomSheet
* Create BottomSheets in any layout
* Configurable header
* MVVM support
<br>


<img src="screenshots/welcome.png?raw=true" height="400"/>  <img src="screenshots/demo.gif" height="400" />  <img src="screenshots/navigation.gif" height="400" />

# Samples

<strong>Check out sample project to explore all features!</strong>.

## Prerequisites

`iOS` at least iOS 15

`Android` at least API 21

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

# Bottom Sheet Control

## Properties
All properties expect `ContentTemplate`, `TitleViewTemplate` and `Peek.PeekViewDataTemplate` are `BindableProperties`

`IsOpen` Open or close BottomSheet

`ContentTemplate` Content of BottomSheet.

`Peek` Peek settings (<strong>available from iOS 16</strong>)

| Setting  | Description  |
|---|---|
| IgnoreSafeArea  | Bottom safe area will either be ignored or not  |
| PeekHeight  | Fixed value for peek height  |
| PeekViewDataTemplate  |  If set view will be placed above `ContentTemplate` and it's height will be set as peek height |

### Appearance

`IsDraggable` Disable/Enable dragging(especially usefull if drawing gestures are made inside bottom sheet)

`HasHandle` Display a drag handle at top of BottomSheet

`IsCancelable` Is BottomSheet cancelable

#### Header
`ShowHeader` Display a header at top of BottomSheet

`TopLeftButtonText` Text of top left button

`TopRightButtonText` Text of top right button

`TitleText` BottomSheet title

`TitleViewTemplate` Custom title view.

`HeaderAppearance` Define look of header

| BottomSheetHeaderAppearanceMode    | Decription |
| --- | --- |
| None  | Do not show a button |
| LeftAndRightButton | Show a button at top lef and at top right     |
| LeftButton    | Show a button at top left    |
| RightButton    | Show a button at top right    |

#### States
`SheetStates` Allowed states of BottomSheet
| BottomSheetState    | Decription |
| -------- | ------- |
| Unknown  | BottomSheet can be all available states |
| Peek | Only `BottomSheet.Peek` is visible. Expanding not allowed     |
| Medium    | BottomSheet height will be half of sceen. Expanding/collapsing not allowed    |
| Large    | BottomSheet will be display in full screen. Expanding/collapsing not allowed    |
| All    | BottomSheet can be all available states    |

`SelectedSheetState` Change current Sheet state. Sheet will be expanded/collapsed if selected state is allowed.

### Interaction

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

`BottomSheet` is a `View` and can be added in any layout or control which accepts `View`.
To open/close a BottomSheet simply set `IsOpen` property to true/false. You can have <strong>multiple</strong> BottomSheets on one page.
```
<bottomsheet:BottomSheet IsOpen="True">
    <bottomsheet:BottomSheet.ContentTemplate>
        <DataTemplate>
            <VerticalStackLayout>
                <Label Text="I'm a simple BottomSheet!"/>
            </VerticalStackLayout>
        </DataTemplate>
    </bottomsheet:BottomSheet.ContentTemplate>
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

To navigate to a `BottomSheet` you have to [register](https://learn.microsoft.com/dotnet/architecture/maui/dependency-injection) `BottomSheets` and `ViewModels`
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

