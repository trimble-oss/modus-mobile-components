﻿using Android.Views;
using Android.Widget;
using Trimble.Modus.Components.Popup.Interfaces;
using Trimble.Modus.Components.Popup.Pages;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Droid.Implementation;

internal class AndroidPopups : IPopupPlatform
{
    private static FrameLayout? DecoreView => Platform.CurrentActivity?.Window?.DecorView as FrameLayout;

    public static bool SendBackPressed(Action? backPressedHandler = null)
    {
        var popupNavigationInstance = PopupService.Instance;

        if (popupNavigationInstance.PopupStack.Count > 0)
        {
            var lastPage = popupNavigationInstance.PopupStack[popupNavigationInstance.PopupStack.Count - 1];

            var isPreventClose = lastPage.SendBackButtonPressed();

            if (!isPreventClose)
            {
                popupNavigationInstance.PopAsync();
            }

            return true;
        }

        backPressedHandler?.Invoke();

        return false;
    }

    public Task AddAsync(PopupPage page)
    {
        try
        {
            HandleAccessibility(true);

            page.Parent = MauiApplication.Current.Application.Windows[0].Content as Element;
            var AndroidNativeView = IPopupPlatform.GetOrCreateHandler<PopupPageHandler>(page).PlatformView as Android.Views.View;
            DecoreView?.AddView(AndroidNativeView);

            return PostAsync(AndroidNativeView);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task RemoveAsync(PopupPage page)
    {
        var renderer = IPopupPlatform.GetOrCreateHandler<PopupPageHandler>(page);

        if (renderer != null)
        {
            HandleAccessibility(false);

            DecoreView?.RemoveView(renderer.PlatformView as Android.Views.View);
            renderer.DisconnectHandler(); //?? no clue if works
            page.Parent = null;

            return PostAsync(DecoreView);
        }

        return Task.CompletedTask;
    }

    static void HandleAccessibility(bool showPopup)
    {
        Page? mainPage = Application.Current?.MainPage;

        if (mainPage is null)
        {
            return;
        }

        int navCount = mainPage.Navigation.NavigationStack.Count;
        int modalCount = mainPage.Navigation.ModalStack.Count;

        ProcessView(showPopup, mainPage.Handler?.PlatformView as Android.Views.View);

        if (navCount > 0)
        {
            ProcessView(showPopup, mainPage.Navigation?.NavigationStack[navCount - 1]?.Handler?.PlatformView as Android.Views.View);
        }

        if (modalCount > 0)
        {
            ProcessView(showPopup, mainPage.Navigation?.ModalStack[modalCount - 1]?.Handler?.PlatformView as Android.Views.View);
        }

        static void ProcessView(bool showPopup, Android.Views.View? view)
        {
            if (view is null)
            {
                return;
            }

            // Screen reader
            view.ImportantForAccessibility = showPopup ? ImportantForAccessibility.NoHideDescendants : ImportantForAccessibility.Auto;

            // Keyboard navigation
            ((ViewGroup)view).DescendantFocusability = DescendantFocusability.AfterDescendants;
            view.ClearFocus();
        }
    }

    Task<bool> PostAsync(Android.Views.View? nativeView)
    {
        if (nativeView == null)
        {
            return Task.FromResult(true);
        }

        var tcs = new TaskCompletionSource<bool>();

        nativeView.Post(() => tcs.SetResult(true));

        return tcs.Task;
    }
}
