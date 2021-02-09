using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DecisionsMobile.Elements
{
    public class CheckBoxView : StackLayout
    {
        private CheckBox _checkBox { get; set; } = new CheckBox() { VerticalOptions = LayoutOptions.Center };
        private Label _label = new Label() { VerticalOptions = LayoutOptions.Center };
        public bool IsChecked
        {
            get
            {
                return _checkBox.IsChecked;
            }
            set
            {
                _checkBox.IsChecked = value;
            }
        }
        public string Text
        {
            get
            {
                return _label.Text;
            } set
            {
                _label.Text = value;
            }
        }
        public CheckBoxView()
        {
            Orientation = StackOrientation.Horizontal;

            this.Children.Add(_checkBox);
            this.Children.Add(_label);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                _checkBox.IsChecked = !_checkBox.IsChecked;
            };

            _label.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
