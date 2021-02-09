using System;
using System.Linq;
using Xamarin.Forms;

namespace DecisionsMobile.Elements
{
    public class RadioButtonListView : StackLayout
    {
        private string _groupName;
        public RadioButtonListView(string groupName)
        {
            _groupName = groupName;
        }

        public event EventHandler SelectedIndexChanged;
        public object SelectedItem
        {
            get
            {
                var checkedRadio = this.Children.Where(x => ((RadioButtonView)x).IsChecked).FirstOrDefault();
                if (checkedRadio == null)
                    return null;
                return ((RadioButtonView)checkedRadio).Text;
            }
            set
            {
                var candidateRadio = this.Children.Where(x => ((RadioButtonView)x).Text == (string)value).FirstOrDefault();
                if (candidateRadio != null)
                    ((RadioButtonView)candidateRadio).IsChecked = true;
            }
        }

        public void Add(string outputValue)
        {
            RadioButtonView radioButtonWrapper = new RadioButtonView(_groupName)
            {
                Text = outputValue
                
            };           
            radioButtonWrapper.CheckedChanged += RadioButton_CheckedChanged;

            this.Children.Add(radioButtonWrapper);
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }
    }

    public class RadioButtonView : StackLayout
    {
        private RadioButton _radioButton = new RadioButton() { VerticalOptions = LayoutOptions.Center, GroupName = "asdf" };
        private Label _label = new Label() { VerticalOptions = LayoutOptions.Center };

        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }
        public bool IsChecked
        {
            get { return _radioButton.IsChecked; }
            set { _radioButton.IsChecked = value; }
        }
        public Color Color
        {
            get { return _radioButton.TextColor; }
            set { _radioButton.TextColor = value; _label.TextColor = value; }
        }

        public EventHandler<CheckedChangedEventArgs> CheckedChanged;

        public RadioButtonView(string groupName)
        {
            Orientation = StackOrientation.Horizontal;

            _radioButton.GroupName = groupName;
            _radioButton.CheckedChanged += _radioButton_CheckedChanged;
            this.Children.Add(_radioButton);
            this.Children.Add(_label);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                _radioButton.IsChecked = !_radioButton.IsChecked;
            };

            _label.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private void _radioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckedChanged?.Invoke(sender, e);
        }
    }
}
