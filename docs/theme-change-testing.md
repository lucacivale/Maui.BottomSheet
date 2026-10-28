# Native OS theme revision testing

This local revision follows both OS appearance and MAUI application theme changes on Android and iOS/MacCatalyst, and popup theme changes on Windows. Explicit sheet background colors remain overrides.

## Test the original failure

1. Set the app to follow the system (`Application.Current.UserAppTheme = AppTheme.Unspecified`).
2. Open a sheet without a `BackgroundColor`, `Background`, or background style. The sample `SomeBottomSheet` is a suitable starting point; verify no global style assigns its background.
3. While the sheet remains open, switch the OS from light to dark and back. On Android use `adb shell cmd uimode night yes` and `adb shell cmd uimode night no`; on iOS Simulator use `xcrun simctl ui booted appearance dark` and `xcrun simctl ui booted appearance light`.
4. Confirm the sheet surface changes in both directions without closing or recreating the sheet. Check the sheet surface separately from its content and surrounding backdrop.
5. Close the sheet, change the OS appearance, and reopen it. Confirm the new default is used. Repeat several open/close cycles.

## Preserve app colors

- Repeat with a fixed explicit background color. It must stay fixed when OS appearance changes.
- Repeat with a background supplied by `AppThemeBinding`. It must follow the binding, including when the app forces a theme that differs from the OS.
- Verify modal and nonmodal sheets retain their configured backdrop and corner appearance.
- On Windows, repeat after changing appearance while the sheet is closed, to check subscription cleanup and refresh on reopening.

## Isolate iOS

The iOS adapter synchronizes the presented controller's `OverrideUserInterfaceStyle` with `Application.UserAppTheme` on opening and on `RequestedThemeChanged`. Light and Dark force native appearance; Unspecified inherits it. The subscription is removed on close, cleanup, and disposal. The default background uses dynamic `UIColor.SystemBackground`; explicit colors remain unchanged. A targeted UIKit appearance-trait registration handles OS changes, including returning to system mode when the effective color did not change and MAUI emitted no event. Systems before iOS/MacCatalyst 17 use the compatibility trait callback. The window observer also forwards the native OS theme notification through `IApplication.ThemeChanged` so MAUI resources stay synchronized while its underlying controller is covered by the sheet. Both observers are released with the sheet.

The sample's **Test native theme switching** button opens a sheet with no background colors configured and Light, Dark, and Follow system buttons. For automated simulator checks, launch with `SIMCTL_CHILD_BOTTOMSHEET_THEME_TEST=1`. Results are written to `Library/theme-appearance-results.json` in the app data container. Add `SIMCTL_CHILD_BOTTOMSHEET_THEME_TEST_SYSTEM=1` and change Simulator appearance after the results report `waiting-for-os-switch` to verify an open sheet follows OS appearance after returning to system mode.

Compilation does not verify native event delivery or appearance. Record device/simulator, OS version, and results for each case before updating PR #261.

## Android app appearance

The Android adapter subscribes to `RequestedThemeChanged` while the sheet is open and releases the subscription on close or disposal. It resolves the existing dialog theme in a separate configuration context with the app-selected night mode, preserving native theme resources without changing the activity or global AppCompat appearance. Configuration changes refresh the same background; clearing an explicit background restores this native default. Test Light/Dark while the OS stays fixed, Follow system with OS changes, fixed colors, clearing overrides, and reopening with an existing app preference.

The Android sample also provides **Run Android theme checks**. It verifies native colors across app switches, explicit-color preservation and clearing, subscription cleanup, and opening a fresh sheet with an existing dark preference. Results are saved to `files/theme-appearance-results.json` and can be read with `adb shell run-as com.companyname.plugin.maui.bottomsheet.sample cat files/theme-appearance-results.json`. Same-instance Android content reuse currently encounters a native parent-attachment error; that separate lifecycle issue is not covered by these theme checks.
