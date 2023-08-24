﻿using System.Runtime.CompilerServices;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components.Controls.BaseInput;

public partial class BaseInput : ContentView
{
    #region Private Properties

    internal const double disabledOpacity = 0.4;
    protected Border InputBorder { get; set; }
    protected Label HelperLabel { get; set; }
    protected Image HelperIcon { get; set; }
    protected HorizontalStackLayout HelperLayout { get; set; }
    protected Label InputLabel { get; set; }

    private ValidationResponse _validationResponse;

    #endregion

    #region Bindable Properties

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(BaseInput), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnTextChanged);
    /// <summary>
    /// Gets or sets the type of keyboard is used when the entry is focused.
    /// </summary>
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(BaseInput), Keyboard.Default);
    /// <summary>
    /// Gets or sets the placeholder text of the text input
    /// </summary>
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(BaseInput), string.Empty);

    /// <summary>
    /// Used to set the return command of the entry
    /// </summary>
    public static readonly BindableProperty ReturnCommandProperty =
        BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(BaseInput), null);

    /// <summary>
    /// Used to set the properties for the return command
    /// </summary>
    public static readonly BindableProperty ReturnCommandParameterProperty =
        BindableProperty.Create(nameof(ReturnCommandParameter), typeof(object), typeof(BaseInput), null, BindingMode.OneWay, null);

    /// <summary>
    /// Gets or sets the type of the return button in the keyboard
    /// </summary>
    public static readonly BindableProperty ReturnTypeProperty =
        BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(BaseInput), ReturnType.Default);

    /// <summary>
    /// Gets or sets the maximum allowed length of the text entry
    /// </summary>
    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(BaseInput), int.MaxValue);

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(BaseInput), null);

    /// <summary>
    /// Gets or sets the required boolean for the label in the control
    /// </summary>
    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(BaseInput), false);

    /// <summary>
    /// Gets or sets the text for helper text label in the control
    /// </summary>
    public static readonly BindableProperty HelperTextProperty =
        BindableProperty.Create(nameof(HelperText), typeof(string), typeof(BaseInput), null, propertyChanged: OnInfoTextsChanged);
    /// <summary>
    /// Gets or sets the text for error text in the control
    /// </summary>
    public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(BaseInput), null, propertyChanged: OnInfoTextsChanged);
    /// <summary>
    /// Gets or sets the text for success text in the control
    /// </summary>
    public static readonly BindableProperty SuccessTextProperty =
        BindableProperty.Create(nameof(SuccessText), typeof(string), typeof(BaseInput), null, propertyChanged: OnInfoTextsChanged);
    /// <summary>
    /// Gets or sets value that indicates whether the input control is enabled or not.
    /// </summary>
    public static new readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(BaseInput), true, propertyChanged: OnEnabledPropertyChanged);

    /// <summary>
    /// Gets or sets value that indicates whether the input control is readonly or not.
    /// </summary>
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(BaseInput), false, propertyChanged: OnReadOnlyPropertyChanged);

    /// <summary>
    /// CommandProperty for Focused Event
    /// </summary>
    public static readonly BindableProperty FocusedCommandProperty =
         BindableProperty.Create(nameof(FocusedCommand), typeof(ICommand), typeof(BaseInput), null);

    /// <summary>
    /// CommandProperty for UnFocused Event
    /// </summary>
    public static readonly BindableProperty UnFocusedCommandProperty =
         BindableProperty.Create(nameof(UnFocusedCommand), typeof(ICommand), typeof(BaseInput), null);
    #endregion

    #region Public Properties
    /// <summary>
    /// Delegate for the input validation function
    /// </summary>
    /// <param name="sender">Reference to the sender input control</param>
    /// <returns>Tuple of bool and string, bool represent the result of the text validation and string represent the text to be displayed</returns>
    public delegate Tuple<bool, string> InputValidationHandler(object sender);
    /// <summary>
    /// Public event handler to be invoked when text is changed
    /// </summary>
    public event EventHandler<TextChangedEventArgs> TextChanged;
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    /// <summary>
    /// Gets or sets the text color
    /// </summary>
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the readonly property of the control
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// Gets or sets the keyboard type property for text input
    /// </summary>
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }
    /// <summary>
    /// Gets or sets the Placeholder value 
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets the Placeholder value 
    /// </summary>
    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets the Return command
    /// </summary>
    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the return command parameter
    /// </summary>
    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the title text
    /// </summary>
    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the required property
    /// </summary>
    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }


    /// <summary>
    /// Gets or sets the maximum length
    /// </summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }
    /// <summary>
    /// Gets or sets the helper text
    /// </summary>
    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }
    /// <summary>
    /// Gets or sets the Error text
    /// </summary>
    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }
    /// <summary>
    /// Gets or sets the Success text
    /// </summary>
    public string SuccessText
    {
        get => (string)GetValue(SuccessTextProperty);
        set => SetValue(SuccessTextProperty, value);
    }
    /// <summary>
    /// Command for Focused Event
    /// </summary>
    public ICommand FocusedCommand
    {
        get => (ICommand)GetValue(FocusedCommandProperty);
        set => SetValue(FocusedCommandProperty, value);
    }
    /// <summary>
    /// Command for UnFocused Event
    /// </summary>
    public ICommand UnFocusedCommand
    {
        get => (ICommand)GetValue(UnFocusedCommandProperty);
        set => SetValue(UnFocusedCommandProperty, value);
    }
    #endregion
    public BaseInput()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        RetrieveAndProcessChildElement();
        SetDefault(this);
    }

    protected virtual void RetrieveAndProcessChildElement()
    {

    }

    #region Private Methods

    /// <summary>
    /// Default method to set the default values to input
    /// </summary>
    /// <param name="tmInput"></param>
    private static void SetDefault(BaseInput tmInput)
    {
        tmInput._validationResponse = ValidationResponse.Info;
        SetBorderColor(tmInput);
    }

    /// <summary>
    /// Triggered when the IsReadOnly property is changed
    /// </summary>
    /// <param name="bindable">Object</param>
    /// <param name="oldValue">Old value</param>
    /// <param name="newValue">New value</param>
    private static void OnReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BaseInput tmInput)
        {
            tmInput.UpdateBorderColors(tmInput);
        }
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BaseInput tmInput)
        {
            tmInput.TextChanged?.Invoke(tmInput, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }
    }

    protected static void SetBorderColor(BaseInput tmInput)
    {
        bool isFocused = tmInput.GetCoreContent().IsFocused;
        bool hasError = !string.IsNullOrEmpty(tmInput.ErrorText);
        bool hasSuccess = !string.IsNullOrEmpty(tmInput.SuccessText);
        tmInput.InputBorder.StrokeThickness = isFocused ? 2 : 1;

        if (isFocused)
        {
            tmInput.FocusedCommand?.Execute(new FocusEventArgs(tmInput, true));
            if (hasError)
            {
                tmInput.HelperLayout.IsVisible = true;
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
                tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.Error_icon_outline);
                tmInput.HelperLabel.Text = tmInput.ErrorText;
            }
            else if (hasSuccess)
            {
                tmInput.HelperLayout.IsVisible = true;
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Green);
                tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.Success_icon_outline);
                tmInput.HelperLabel.Text = tmInput.SuccessText;
            }
            else
            {
                if (!string.IsNullOrEmpty(tmInput.HelperText))
                {
                    tmInput.HelperLayout.IsVisible = true;
                }
                else
                {
                    tmInput.HelperLayout.IsVisible = false;
                }
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.BlueInfoOutlineIcon);
                tmInput.HelperLabel.Text = tmInput.HelperText;
            }

        }
        else
        {
            tmInput.UnFocusedCommand?.Execute(new FocusEventArgs(tmInput, false));
            if (hasError)
            {
                tmInput.HelperLayout.IsVisible = true;
                tmInput.HelperLabel.Text = tmInput.ErrorText;
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
                tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.Error_icon_outline);
            }
            else if (tmInput.IsRequired && string.IsNullOrEmpty(tmInput.Text))
            {
                tmInput.HelperLayout.IsVisible = true;
                tmInput.HelperLabel.Text = "Field is Required";
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
                tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.Error_icon_outline);
                    
            }
            else
            {
                if(!string.IsNullOrEmpty(tmInput.HelperText))
                {
                    tmInput.HelperLayout.IsVisible = true;
                    tmInput.HelperLabel.Text = tmInput.HelperText;
                    tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
                    tmInput.HelperIcon.Source = ImageSource.FromFile(ImageConstants.BlueInfoOutlineIcon);

                }
                else
                {

                    tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
                    tmInput.HelperLayout.IsVisible = false;
                }
            }
        }
    }

    private static void OnInfoTextsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BaseInput tmInput)
        {
            SetBorderColor(tmInput);
        }
    }
    private void UpdateBorderColors(BaseInput tmInput)
    {
        if (tmInput.IsReadOnly)
        {
            SetReadOnlyStyles(tmInput);
        }
        else
        {
            if (tmInput.IsEnabled)
            {
                tmInput.InputBorder.Opacity = tmInput.InputLabel.Opacity = tmInput.HelperLayout.Opacity = 1;
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
                tmInput.InputBorder.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                tmInput.GetCoreContent().BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                SetBorderColor(tmInput);
            }
            else
            {
                tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                tmInput.InputBorder.StrokeThickness = 1;
                tmInput.InputBorder.Opacity = tmInput.InputLabel.Opacity = tmInput.HelperLayout.Opacity = disabledOpacity;
            }
        }
    }
    private static void SetReadOnlyStyles(BaseInput tmInput)
    {
        tmInput.InputBorder.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleReadOnlyGray);
        tmInput.InputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleReadOnlyGray);
        tmInput.InputBorder.StrokeThickness = 0;
        tmInput.InputBorder.Opacity = tmInput.InputLabel.Opacity = tmInput.HelperLayout.Opacity = 1;
    }


    private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BaseInput tmInput)
        {
            tmInput.UpdateBorderColors(tmInput);
        }
    }

    #endregion

    #region Protected Methods
    internal virtual View GetCoreContent()
    {
        return null;
    }

    #endregion
}
