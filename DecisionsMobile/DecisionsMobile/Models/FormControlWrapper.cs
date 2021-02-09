using DecisionsMobile.Elements;
using DecisionsMobile.Models.FormService;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DecisionsMobile.Models
{
    public abstract class FormControlWrapper
    {
        public FormControlWrapper(ChildElement elementData, View view)
        {
            ElementData = elementData;            
            View = view;
        }

        public View View { get; set; }

        public ChildElement ElementData { get; set; }

        public abstract DataPair[] GetValue();

        public virtual void SetValidation(BreakLevel level)
        {
            /* NOOP override to implement */
        }

        public virtual void ClearValidation()
        {
            /* NOOP override to implement */
        }

        public virtual bool IsEmpty(out string dataName)
        {
            dataName = ElementData.Child.DataName;
            return false;
        }
    }

    public class ButtonControl : FormControlWrapper
    {
        public ButtonControl(ChildElement elementData, Button view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            // DataPair pair = new DataPair();
            // pair.Name = ElementData.Child.DataName;
            // pair.OutputValue = wait, we shouldn't need this for button.
            return null;
        }

        public Button Button { get => (Button)View; }
    }

    public class LabelControl : FormControlWrapper
    {
        public LabelControl(ChildElement elementData, Label view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            // DataPair pair = new DataPair();
            // pair.Name = ElementData.Child.DataName;
            // pair.OutputValue = wait, we shouldn't need this for Label either...
            return null;
        }

    }

    public class CheckBoxControl : FormControlWrapper
    {
        public CheckBoxControl(ChildElement elementData, CheckBoxView view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {

            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as CheckBoxView).IsChecked;
            return new DataPair[] { pair };
        }
    }

    public class RadioButtonControl : FormControlWrapper
    {
        public RadioButtonControl(ChildElement elementData, RadioButton view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as RadioButton).IsChecked;
            return new DataPair[] { pair };
        }


        public RadioButton RadioButton { get => (RadioButton)View; }

    }

    public class ImageControl : FormControlWrapper
    {
        public ImageControl(ChildElement elementData, Image view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {

            return null;
        }

        public Image Image { get => (Image)View; }

    }

    public class SliderControl : FormControlWrapper
    {
        public SliderControl(ChildElement elementData, Slider view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as Slider).Value;
            return new DataPair[] { pair };
        }

        public Slider Slider { get => (Slider)View; }

    }

    public class ImageButtonControl : FormControlWrapper
    {
        public ImageButtonControl(ChildElement elementData, Button view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            return null; // button doesn't have a value
        }

        public Button ImageButton { get => (Button)View; }

    }

    public class ToggleControl : FormControlWrapper
    {
        public ToggleControl(ChildElement elementData, Switch view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as Switch).IsToggled;
            return new DataPair[] { pair };
        }

        public Switch Switch { get => (Switch)View; }

    }

    public class TextAreaControl : FormControlWrapper
    {
        public TextAreaControl(ChildElement elementData, Editor view) : base(elementData, view)
        {
            view.TextChanged += (sender, args) => ClearValidation();
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as Editor).Text;
            return new DataPair[] { pair };
        }

        public override void SetValidation(BreakLevel level)
        {
            ((Editor)View).BackgroundColor = (Color)Application.Current.Resources["WarnLight"];
        }

        public override void ClearValidation()
        {
            ((Editor)View).BackgroundColor = Color.Transparent;
        }

        public override bool IsEmpty(out string dataName)
        {
            dataName = ElementData.Child.DataName;
            return string.IsNullOrEmpty(((Editor)View).Text);
        }
    }

    public class DateControl : FormControlWrapper
    {
        public DateControl(ChildElement elementData, DatePicker view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as DatePicker).Date.ToShortDateString(); // TODO formats
            return new DataPair[] { pair };
        }

    }

    public class TimePickerControl : FormControlWrapper
    {
        public TimePickerControl(ChildElement elementData, TimePicker view) : base(elementData, view)
        {
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;
            pair.OutputValue = (View as TimePicker).Time.ToString(); // TODO formats
            return new DataPair[] { pair };
        }
    }

    public class TextBoxControl : FormControlWrapper
    {
        public TextBoxControl(ChildElement elementData, Entry view) : base(elementData, view)
        {
            view.TextChanged += (sender, args) => ClearValidation();
        }

        public override DataPair[] GetValue()
        {
            DataPair pair = new DataPair();
            pair.Name = ElementData.Child.DataName;

            string text = (View as Entry).Text, outputValue = string.Empty;
            string placeholder = (View as Entry).Placeholder;
            
            if (ElementData.Child.ServerType.Contains("SilverNumberBox"))
            {   
                if (ElementData.Child.OutputType == (byte)NumberBoxOutputType.Double)
                {
                    double t = 0;
                    if (Double.TryParse(text, out t))
                        outputValue = t.ToString();
                } else if (ElementData.Child.OutputType == (byte)NumberBoxOutputType.Int)
                {
                    int t = 0;
                    if (Int32.TryParse(text, out t))
                        outputValue = t.ToString();
                } else if (ElementData.Child.OutputType == (byte)NumberBoxOutputType.Decimal)
                {
                    decimal t = 0;
                    if (Decimal.TryParse(text, out t))
                        outputValue = t.ToString();
                }
                if (string.IsNullOrEmpty(outputValue) && !string.IsNullOrEmpty(placeholder))
                    outputValue = placeholder;
            } else
            {
                outputValue = text;
            }

            pair.OutputValue = outputValue;
            return new DataPair[] { pair };
        }

        public override void SetValidation(BreakLevel level)
        {
            View.BackgroundColor = (Color)Application.Current.Resources["WarnLight"];
        }

        public override void ClearValidation()
        {
            View.BackgroundColor = Color.Transparent;
        }

        public override bool IsEmpty(out string dataName)
        {
            dataName = ElementData.Child.DataName;
            return string.IsNullOrEmpty((View as Entry).Text);
        }
    }

    public class ComboControl : FormControlWrapper
    {
        private DataPair[] options;
        public ComboControl(ChildElement elementData, Picker view, DataPair[] options) : base(elementData, view)
        {
            this.options = options;
            view.SelectedIndexChanged += (sender, args) => ClearValidation();
        }

        public void SetSelected(DataPair selectedOption)
        {
            ((Picker)View).SelectedItem = selectedOption.OutputValue;
        }

        public override DataPair[] GetValue()
        {
            // typedInTextValue
            var stringValue = (string)(View as Picker).SelectedItem;
            DataPair typedInText = new DataPair();
            typedInText.Name = $"TypedInTextFor$${ElementData.Child.ComponentId}"; // interesting magic string...
            typedInText.OutputValue = stringValue;

            // get the GUID of the selcted item
            DataPair selectedItem = new DataPair();
            // control doesn't allow free text so this should always resolve for us...
            var selectedOption = Array.Find(options, option => option.OutputValue.Equals(stringValue));
            string selectedItemId = selectedOption?.Name;
            //var selectedItemIds = new string[] { selectedItemId };
            var selectedItemIds = new List<string> { selectedItemId };
            selectedItem.Name = ElementData.Child.SelectedItemDataNameKey; // __selected<GUID>
            selectedItem.OutputValue = selectedItemIds;

            return new DataPair[] { selectedItem, typedInText };
        }
        public override void SetValidation(BreakLevel level)
        {
            View.BackgroundColor = (Color)Application.Current.Resources["WarnLight"];
        }

        public override void ClearValidation()
        {
            View.BackgroundColor = Color.Transparent;
        }
        public override bool IsEmpty(out string dataName)
        {
            dataName = ElementData.Child.SelectedItemDataName;
            var dataPairs = GetValue();
            if (dataPairs != null && dataPairs.Length == 2 && !string.IsNullOrEmpty((dataPairs[0].OutputValue as List<string>)[0]))
                return false;
            return true;
        }
    }
    public class RadioListControl : FormControlWrapper
    {
        private DataPair[] options;
        public RadioListControl(ChildElement elementData, RadioButtonListView view, DataPair[] options) : base(elementData, view)
        {
            this.options = options;
            view.SelectedIndexChanged += (sender, args) => ClearValidation();
        }

        public void SetSelected(DataPair selectedOption)
        {
            ((RadioButtonListView)View).SelectedItem = selectedOption.OutputValue;
        }

        public override DataPair[] GetValue()
        {
            // typedInTextValue
            var stringValue = (string)(View as RadioButtonListView).SelectedItem;

            // get the GUID of the selcted item
            DataPair selectedItem = new DataPair();
            // control doesn't allow free text so this should always resolve for us...
            var selectedOption = Array.Find(options, option => option.OutputValue.Equals(stringValue));
            string selectedItemId = selectedOption?.Name;
            //var selectedItemIds = new string[] { selectedItemId };
            var selectedItemIds = new List<string> { selectedItemId };
            selectedItem.Name = ElementData.Child.SelectedItemDataNameKey; // __selected<GUID>
            selectedItem.OutputValue = selectedItemIds;

            return new DataPair[] { selectedItem };
        }
        public override void SetValidation(BreakLevel level)
        {
            View.BackgroundColor = (Color)Application.Current.Resources["WarnLight"];
        }

        public override void ClearValidation()
        {
            View.BackgroundColor = Color.Transparent;
        }

        public override bool IsEmpty(out string dataName)
        {
            dataName = ElementData.Child.SelectedItemDataName;
            var dataPairs = GetValue();
            if (dataPairs != null && dataPairs.Length > 0 && !string.IsNullOrEmpty((dataPairs[0].OutputValue as List<string>)[0]))
                return false;
            return true;
        }
    }
}
