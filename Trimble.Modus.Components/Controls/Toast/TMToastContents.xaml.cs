using Microsoft.Maui.Platform;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Popup.Pages.PopupPage

{
    private const int DELAYTIME = 5000;
    public ImageSource LeftIconSource { get; set; }
    public string Message { get; set; }

    public string RightIconText { get; set; }

    public ImageSource RightIconSource { get; set; }

    public double ToastWidthRequest { get; set; }

    public Color ToastTheme { get; set; }

    public Color TextColor { get; set; }


    PopupNavigation popupNavigation;

    internal TMToastContents(string message, ImageSource leftIcon, string actionButtonText, Object popupNavigation,ToastTheme theme,Action action )
    {
  
        InitializeComponent();
        SetTheme(theme.ToString());
        this.popupNavigation = (PopupNavigation) popupNavigation;
        PopupData(message, leftIcon, actionButtonText, action);
        BindingContext = this;
        Close();
    }

    private void SetTheme(String toastTheme)
    {
        ToastTheme = (Color)BaseComponent.colorsDictionary()[toastTheme];
        if (string.Equals(toastTheme, "ToastBlue"))
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];

        }
        if (string.Equals(toastTheme, "ToastBlack"))
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastWhite"];
        }
        else
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastBlack"];
        }
    }

    public TMToastContents()
    {
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
         popupNavigation.RemovePageAsync(this, true);
       
    }
    public void Close()
    {
        Task.Run(async () => {
            await Task.Delay(DELAYTIME);
            await popupNavigation.RemovePageAsync(this, true);
         });
    }

    private void PopupData(string message, ImageSource leftIcon , string actionButtonText, Action action)
    {
        LeftIconSource = leftIcon;
        RightIconText = actionButtonText;
        TMButton rightIcon = new TMButton();
        rightIcon.Title = RightIconText;
       // rightIcon.TextColor = TextColor;
        if (ToastTheme.Equals((Color)BaseComponent.colorsDictionary()["ToastBlue"]))
        {
            rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.blue_close_icon.png");
        }
        else if (ToastTheme.Equals((Color)BaseComponent.colorsDictionary()["ToastBlack"]))
        {
            rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.white_close_icon.png");
        }
        else
        {
            rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
        }
        rightIcon.Clicked += CloseButton_Clicked;
        if (!string.IsNullOrEmpty(RightIconText)) {
            rightIcon.IconSource = null;
            rightIcon.Clicked += (sender, args) => {
                action.Invoke();
            };
        
        }
        rightIcon.VerticalOptions = LayoutOptions.Center;
        rightIcon.HorizontalOptions = LayoutOptions.End;
        rightIcon.BackgroundColor = this.BackgroundColor;
        rightIcon.Size = Enums.Size.XSmall;
        rightIcon._iconWidth = 16;
        rightIcon._iconHeight = 16;
      //  rightIcon.BorderColor = Colors.Transparent;
        contentLayout.Children.Add(rightIcon);
        var idiom = Device.Idiom;
        setWidth(rightIcon,idiom);
        Message = GetWrappedLabelText(message,idiom);
    }

    private void setWidth(TMButton rightIcon,TargetIdiom idiom)
    {
        double minimumTabletWidth = 480; 
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        if (idiom == TargetIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16,0,16,10);
            rightIcon.Size = Enums.Size.XSmall;
        }
        else if (idiom == TargetIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            rightIcon.Size = Enums.Size.Large;
        }

    }
    private string GetWrappedLabelText(string text, TargetIdiom idiom)
    {
        const string ellipsis = "...";
        if (idiom == TargetIdiom.Phone)
        {
            if (text.Length > 106)
            {
                text = text.Substring(0, 106) + ellipsis;
            }
        }
        else if (idiom == TargetIdiom.Tablet)
        {
            if (text.Length > 206)
            {
                text = text.Substring(0, 206) + ellipsis;
            }
        }
        return text;
    }



}
