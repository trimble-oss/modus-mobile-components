using DemoApp.ViewModels;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class TMSpinnerPage : ContentPage
{
    private SpinnerPageViewModel spinnerPageViewModel;
    public TMSpinnerPage()
    {
        spinnerPageViewModel = new SpinnerPageViewModel();
        InitializeComponent();
        BindingContext = spinnerPageViewModel;
    }

    private void SelectedRadioButtonChanged(object sender, Trimble.Modus.Components.TMRadioButtonEventArgs e)
    {
        spinnerPageViewModel.SpinnerColor = ((string)e.Value == "Primary") ? SpinnerColor.Primary : SpinnerColor.Secondary;
    }
}
