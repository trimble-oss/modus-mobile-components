using CommunityToolkit.Maui.Behaviors;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : PopupPage

{
    #region Private Properties

    private const int DELAYTIME = 5000;

    #endregion

    #region Public Properties

    public string Message { get; set; }

    public string RightIconText { get; set; }

    public double ToastWidthRequest { get; set; }

    #endregion

    public static readonly BindableProperty ToastBackgroundColorProperty =
        BindableProperty.Create(nameof(ToastBackgroundColor), typeof(Color), typeof(TMToast), Colors.Black);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TMToast), Colors.White);

    public static readonly BindableProperty LeftIconTintColorProperty =
        BindableProperty.Create(nameof(LeftIconTintColor), typeof(Color), typeof(TMToast), Colors.White,
            propertyChanged: OnLeftIconTintColorChanged);

    public static readonly BindableProperty RightIconTintColorProperty =
    BindableProperty.Create(nameof(RightIconTintColor), typeof(Color), typeof(TMToast), Colors.White,
        propertyChanged: OnRightIconTintColorChanged);


    internal TMToastContents(string message, string actionButtonText, ToastTheme theme, Action action, bool isDismissable)
    {
        InitializeComponent();
        SetTheme(theme.ToString());
        SetDynamicResource(StyleProperty, theme.ToString());
        PopupData(message, actionButtonText, action, isDismissable);
        BindingContext = this;
        CloseAfterDelay();
    }
    public Color ToastBackgroundColor
    {
        get { return (Color)GetValue(ToastBackgroundColorProperty); }
        set { SetValue(ToastBackgroundColorProperty, value); }
    }
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }
    public Color LeftIconTintColor
    {
        get => (Color)GetValue(LeftIconTintColorProperty);
        set => SetValue(LeftIconTintColorProperty, value);
    }
    public Color RightIconTintColor
    {
        get => (Color)GetValue(RightIconTintColorProperty);
        set => SetValue(RightIconTintColorProperty, value);
    }

    private static void OnLeftIconTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMToastContents tmToastContent && DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            tmToastContent.UpdateIconColor();
        }
    }

    private static void OnRightIconTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMToastContents tmToastContent && DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            tmToastContent.UpdateIconColor();
        }
    }

    private void UpdateIconColor()
    {
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            leftIconImage.Behaviors.Clear();
            if (LeftIconTintColor != null)
            {
                var behavior = new IconTintColorBehavior
                {
                    TintColor = LeftIconTintColor
                };
                leftIconImage.Behaviors.Add(behavior);
            }
            closeButton.Behaviors.Clear();
            if (RightIconTintColor != null)
            {
                var behavior = new IconTintColorBehavior
                {
                    TintColor = RightIconTintColor
                };
                closeButton.Behaviors.Add(behavior);
            }
        }
    }

    #region Private Methods
    private void SetTheme(String toastTheme)
    {
        ToastTheme theme = (ToastTheme)Enum.Parse(typeof(ToastTheme), toastTheme);
        switch (theme)
        {
            case ToastTheme.Dark:               
                leftIconImage.Source = ImageSource.FromFile(ImageConstants.WhiteInfoIcon);
                closeButton.Source = ImageSource.FromFile(ImageConstants.ToastWhiteCloseIcon);
                break;

            case ToastTheme.Primary:
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastPrimaryCloseIcon");
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastPrimaryInfoIcon");
                break;

            case ToastTheme.Secondary:
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastCloseIcon");
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastSecondaryInfoIcon");
                break;

            case ToastTheme.Danger:
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastCloseIcon");
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastDangerIcon");
                break;

            case ToastTheme.Warning:
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastCloseIcon");
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastWarningIcon");
                break;

            case ToastTheme.Success:
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastSuccessIcon");
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastCloseIcon");
                break;

            default:
                closeButton.SetDynamicResource(ImageButton.SourceProperty, "ToastCloseIcon");
                leftIconImage.SetDynamicResource(Image.SourceProperty, "ToastDefaultInfoIcon");
                break;
        }
        UpdateIconColor();

    }
    private void CloseButtonClicked(object sender, EventArgs e)

    {
        PopupService.Instance.RemovePageAsync(this, true);

    }
    public void CloseAfterDelay()
    {
        Task.Run(async () =>
        {
            await Task.Delay(DELAYTIME);
            await PopupService.Instance.RemovePageAsync(this, true);
        });
    }

    private void PopupData(string message, string actionButtonText, Action action, bool isDismissable)
    {
        RightIconText = actionButtonText;
        actionButton.Text = RightIconText;
        actionButton.TextColor = TextColor;
        actionButton.BackgroundColor = ToastBackgroundColor;

        if (string.IsNullOrEmpty(RightIconText))
        {
            closeButton.IsVisible = isDismissable;
            actionButton.IsVisible = false;
            if (isDismissable)
            {
                closeButton.Clicked += (sender, args) =>
                {
                    action?.Invoke();
                };
            }
        }
        else
        {
            closeButton.IsVisible = false;
            actionButton.IsVisible = true;
            actionButton.Clicked += (sender, args) =>
            {
                action?.Invoke();
            };
        }

        var idiom = DeviceInfo.Current.Idiom;
        setWidth(idiom);
        Message = message;
    }

    private void setWidth(DeviceIdiom idiom)
    {
        double minimumTabletWidth = 480;
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        if (idiom == DeviceIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16, 0, 16, 10);

        }
        else if (idiom == DeviceIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;

        }
    }
    #endregion
}
