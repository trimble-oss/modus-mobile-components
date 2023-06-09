﻿using Trimble.Modus.Components.Contant;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{
    public class TMRadioButton : ContentView, IDisposable
    {
        #region Fields

        private readonly Label _label;
        private readonly Image _icon;
        private readonly int _defaultDimension = 24, _largeDimension = 32;
        private readonly int _defaultFontSize = 14, _largeFontSize = 16;
        private readonly TapGestureRecognizer _tapGesture = new();
        internal bool CreatedFromItemSource = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Triggered when <see cref="IsSelected"/> changes.
        /// </summary>
        public event EventHandler<CheckedChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Is Enabled state for button
        /// </summary>
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            internal set { SetValue(IsEnabledPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the size of this <see cref="TMRadioButton"/>
        /// </summary>
        public CheckboxSize Size
        {
            get => (CheckboxSize)GetValue(SizeProperty);
            internal set => SetValue(SizePropertyKey, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the text of this <see cref="TMRadioButton"/>
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the selected state of this <see cref="TMRadioButton"/>
        /// </summary>
        /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating the identifier of this <see cref="TMRadioButton"/>
        /// </summary>
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Bindable Properties

        internal static readonly BindablePropertyKey IsEnabledPropertyKey = BindableProperty.CreateReadOnly(nameof(IsEnabled), typeof(bool), typeof(TMRadioButton), true, propertyChanged: OnIsEnabledChanged);

        public static new readonly BindableProperty IsEnabledProperty = IsEnabledPropertyKey.BindableProperty;

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TMRadioButton), string.Empty);

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TMRadioButton), false, propertyChanged: OnSelectedChanged);

        public static readonly BindablePropertyKey SizePropertyKey = BindableProperty.CreateReadOnly(nameof(Size), typeof(CheckboxSize), typeof(TMRadioButton), CheckboxSize.Default, propertyChanged: OnSizeChanged);

        public static readonly BindableProperty SizeProperty = SizePropertyKey.BindableProperty;

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(TMRadioButton), null);

        #endregion

        #region Constructor
        public TMRadioButton()
        {
            _label = new Label()
            {
                FontSize = _defaultFontSize,
                VerticalOptions = LayoutOptions.Center
            };
            _label?.SetBinding(
                Label.TextProperty,
                new Binding(nameof(Text), BindingMode.TwoWay, source: this)
            );

            _icon = new Image
            {
                Source = ImageSource.FromFile(ImageConstants.DefaultRadioButton),
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = _defaultDimension,
                WidthRequest = _defaultDimension,
                Margin = new Thickness(0, 0, 4, 0)
            };
            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { _icon, _label, }
            };
            _tapGesture.Tapped += OnTapGestureTapped;
            GestureRecognizers.Add(_tapGesture);
            Margin = new Thickness(0, 0, 5, 5);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Change dimension and font size based on size property
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var radioButton = (TMRadioButton)bindable;

            if (radioButton.Size == CheckboxSize.Large)
            {
                radioButton._label.FontSize = radioButton._largeFontSize;
                radioButton._icon.WidthRequest = radioButton._largeDimension;
                radioButton._icon.HeightRequest = radioButton._largeDimension;
            }
            else
            {
                radioButton._label.FontSize = radioButton._defaultFontSize;
                radioButton._icon.WidthRequest = radioButton._defaultDimension;
                radioButton._icon.HeightRequest = radioButton._defaultDimension;
            }
        }

        /// <summary>
        /// Change the enabled state of the button based on IsEnabled property
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var radioButton = (TMRadioButton)bindable;
            radioButton.UpdateEnabledState();
        }

        /// <summary>
        /// Triggered when <see cref="IsSelected"/> changes, updates the UI accordingly and invokes the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not TMRadioButton radioButton)
                return;

            radioButton.SelectionChanged?.Invoke(
                radioButton,
                new CheckedChangedEventArgs((bool)newValue)
            );

            radioButton._icon.Source = radioButton.IsSelected
                ? ImageSource.FromFile(ImageConstants.SelectedRadioButton)
                : ImageSource.FromFile(ImageConstants.DefaultRadioButton);
        }

        /// <summary>
        /// For Selected state while tapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGestureTapped(object sender, TappedEventArgs e)
        {
            if (IsEnabled && !IsSelected)
            {
                IsSelected = !IsSelected;
            }
        }

        /// <summary>
        /// Set The Disabled State
        /// </summary>
        private void UpdateEnabledState()
        {
            _icon.IsEnabled = IsEnabled;
            _label.IsEnabled = IsEnabled;
            Opacity = IsEnabled ? 1 : 0.5;
        }

        public void Dispose()
        {
            _tapGesture.Tapped -= OnTapGestureTapped;
            GestureRecognizers.Clear();
        }
        #endregion
    }
}
