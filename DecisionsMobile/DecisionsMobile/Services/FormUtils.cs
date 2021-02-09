using DecisionsMobile.Constants;
using DecisionsMobile.Elements;
using DecisionsMobile.Helper;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace DecisionsMobile.Services
{
    /// <summary>
    /// Utility class for converting Decisions Form Control data to Xamarin Form elements.
    /// </summary>
    public static class FormUtils
    {
        public static GridUnitType GetGridUnitType(LayoutElementType type)
        {
            switch (type)
            {
                case LayoutElementType.Fixed:
                    return GridUnitType.Absolute;
                case LayoutElementType.Grow:
                    return GridUnitType.Auto;
                default:
                    return GridUnitType.Star;
            }
        }

        public static TextAlignment GetLabelHorizontalAlign(long? type)
        {   
            if (type == 2) return TextAlignment.End;
            if (type == 3) return TextAlignment.Center;
            else return TextAlignment.Start;
        }
        public static TextAlignment GetLabelVerticalAlign(long? type)
        {   
            if (type == 1) return TextAlignment.Start;
            if (type == 3) return TextAlignment.End;
            else return TextAlignment.Center;
        }

        // TODO fix these Child -> ChildChild names...
        public static FormControlWrapper GetComponentView(ChildElement element, StandAloneFormSessionInfo info)
        {
            ComponentContainer child = element.Child;

            if (child.ServerType.Contains("SilverImage"))
            {
                switch (child.ComponentName)
                {
                    case "Image":
                        return new ImageControl(element, GetImage(child, info));

                    case "Image Button":
                        return new ImageButtonControl(element, GetImageButton(child, info));

                }
                // Textbox maps to Image https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.Image
            }

            if (child.ServerType.Contains("SilverToggleButton"))
            {
                // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/checkbox

                return new ToggleControl(element, GetToggle(child, info));

            }
            /*
            if (child.ComponentName.Contains("RadioButton"))
            {
                // Textbox maps to RadioButton https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.RadioButton
                return new RadioButtonControl(element, GetRadioButton(child, info));
            }
            */
            if (child.ServerType.Contains("SilverRadioButtonList"))
            {
                var options = GetRadioButtonListOptions(child, info);
                var selectedOption = GetSelectedRadioButtonListOption(child, info, options);
                var control = new RadioListControl(element, GetRadioButtonListView(child, options), options);
                if (selectedOption != null)
                {
                    control.SetSelected(selectedOption);
                }
                return control;
            }
            if (child.ServerType.Contains("SilverButton"))
            {
                // Button maps to Button https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/button
                return new ButtonControl(element, GetButton(child));
            }

            if (child.ServerType.Contains("SilverLabel"))
            {
                // Label maps to Label https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/text/label
                return new LabelControl(element, GetLabel(child, info));
            }

            if (child.ServerType.Contains("SilverTextArea,"))
            {
                // Text Area gets mapped to Editor https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/text/editor
                return new TextAreaControl(element, GetEditor(child, info));
            }

            if (child.ServerType.Contains("SilverDatePicker"))
            {
                // Datepicker https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.datepicker
                return new DateControl(element, GetDatePicker(child, info));
            }

            if (child.ServerType.Contains("SilverTimePicker"))
            {
                // TimePicker https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.timepicker
                return new TimePickerControl(element, GetTimePicker(child, info));
            }

            if (child.ServerType.Contains("SilverTextBox"))
            {
                // Textbox maps to Entry https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.entry
                return new TextBoxControl(element, GetEntry(child, info));
            }

            if (child.ServerType.Contains("SilverNumberBox"))
            {
                return new TextBoxControl(element, GetEntry(child, info));
            }

            if (child.ServerType.Contains("SilverCheckBox"))
            {
                // Textbox maps to Checkbox https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.Checkbox
                return new CheckBoxControl(element, GetCheckBox(child, info));
            }

            //if (child.ComponentName.Replace(" ", string.Empty) == "ImageButton")
            //{
            //    return new ImageButtonControl(element, GetImageButton(child, info));
            //}


            if (child.ServerType.Contains("SilverCombo"))
            {
                // Dropdown maps to https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.picker
                var options = GetPickerOptions(child, info);
                var selectedOption = GetSelectedPickerOption(child, info, options);
                var control = new ComboControl(element, GetPicker(child, options), options);
                if (selectedOption != null)
                {
                    control.SetSelected(selectedOption);
                }
                return control;
            }

            if (child.ServerType.Contains("SilverSlider"))
            {
                return new SliderControl(element, GetSlider(child, info));
            }
            // Slider https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.slider
            // Checkbox maps to Checkbox https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/checkbox

            if (child.ServerType.Contains("SilverPasswordBox"))
            {
                return new TextBoxControl(element, GetEntry(child, info));
            }

            return new LabelControl(element,
                new Label()
                {
                    Text = "Not yet implemented."
                }
            );
        }

        public static Button GetButton(ComponentContainer silverButton)
        {
            Button button = new Button()
            {
                Text = silverButton.Text,
                //BackgroundColor silverButton.
            };

            return button;
        }
        public static Image GetImage(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            Image image = new Image()
            {
                Source = GetImageData<string>(child, info),
                //BackgroundColor silverButton.
            };

            return image;
        }
        public static Button GetImageButton(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            Button imagebtn = new Button()
            {
                ImageSource = ImageSource.FromUri(new Uri(GetImageButtonData(child, info))),
                BackgroundColor = Color.Transparent
            };

            return imagebtn;
        }

        public static RadioButton GetRadioButton(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            RadioButton radioButton = new RadioButton()
            {
                IsChecked = GetRadioData<bool>(child, info)
                //BackgroundColor silverButton.
            };

            return radioButton;
        }

        public static CheckBoxView GetCheckBox(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            CheckBoxView checkBox = new CheckBoxView()
            {
                IsChecked = GetControlData<bool>(child, info),
                Text = child.DataName
            };

            return checkBox;
        }

        public static Label GetLabel(ComponentContainer child, StandAloneFormSessionInfo info)
        { 
            Label label = new Label()
            {
                Text = child.Text,
                HorizontalTextAlignment = GetLabelHorizontalAlign(child.LabelAlignment),
                VerticalTextAlignment = GetLabelVerticalAlign(child.LabelVerticalAlignment),                
                //TextColor = GetColor(silverLabel.Attributes.LabelColor),
            };

            return label;
        }

        public static Editor GetEditor(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            Editor editor = new Editor()
            {
                Text = GetControlData<string>(child, info),
                //TextColor = GetColor(silverLabel.Attributes.LabelColor),
            };

            return editor;
        }

        public static Entry GetEntry(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            string text = string.Empty, placeholder = string.Empty;
            Keyboard keyboard = Keyboard.Default;
            
            if (child.ServerType.Contains("SilverNumberBox"))
            {
                keyboard = Keyboard.Numeric;
                object t = GetControlData<object>(child, info);
                if (t != null)
                {
                    if (child.OutputType == (byte)NumberBoxOutputType.Int)
                    {
                        int val = Convert.ToInt32(t);
                        if (val != 0) placeholder = text = val.ToString();
                        else placeholder = "0";
                    } else
                    {
                        double val = Convert.ToDouble(t);
                        if (val != 0) placeholder = text = val.ToString();
                        else placeholder = "0";
                    }

                }
                
            } else
            {
                text = GetControlData<string>(child, info);
            }



            Entry entry = new Entry()
            {
                // Text = child.Text,
                Text = text,
                Placeholder = placeholder,
                Keyboard = keyboard,
                IsPassword = child.ServerType.Contains("SilverPasswordBox")
                //TextColor = GetColor(silverLabel.Attributes.LabelColor),
            };

            return entry;
        }
        public static Xamarin.Forms.Switch GetToggle(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            Xamarin.Forms.Switch Switch = new Xamarin.Forms.Switch()
            {
                IsToggled = GetToggleData<bool>(child, info)
                // Text = child.Text,m
                //   Text = GetControlData<string>(child, info)
                //TextColor = GetColor(silverLabel.Attributes.LabelColor),
            };

            return Switch;
        }

        public static Slider GetSlider(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            Slider slider = new Slider()
            {
                Minimum = child.Min ?? 0,
                Maximum = child.Max ?? 1,
                Value = GetControlData<double>(child, info),
                MaximumTrackColor = (Color)Application.Current.Resources["LightTextColor"]
            };

            return slider;
        }

        public static Picker GetPicker(ComponentContainer child, DataPair[] options)
        {
            Picker picker = new Picker();
            Array.ForEach(options, option => picker.Items.Add((string)option.OutputValue));
            return picker;
        }

        public static RadioButtonListView GetRadioButtonListView(ComponentContainer child, DataPair[] options)
        {   
            RadioButtonListView radioButtonListView = new RadioButtonListView(child.ComponentId);
            Array.ForEach(options, option => radioButtonListView.Add((string)option.OutputValue));
            return radioButtonListView;
        }

        private static string GetImageData<T>(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            string name = child.ComponentId ?? child.ComponentName;
            string sessionid = info.FormSessionInfoId;
            string gen = Array.Find(info.InitialControlsData,
            data => data.Name.Equals(name)).OutputValue.ToString();
            
            var uriTask = RestConstants.GetUriAsync("FormService", "GetFileStream?");
            uriTask.Wait();
            string url = uriTask.Result.AbsoluteUri +
                $"formSessionInfoId={sessionid}&componentId={name}&fileId=silverimagefile&downloadFile=true&gen={gen}";
            return url;
        }

        private static string GetImageButtonData(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            return ImageHelper.GetImageInfoUrl(child.ImageInfo);

        }

        private static T GetToggleData<T>(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            string name = child.DataName ?? child.ComponentId;
            return (T)Array.Find(info.InitialControlsData,
                data => data.Name.Equals(name)).OutputValue;
        }

        private static bool GetRadioData<T>(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            string name = child.ComponentId ?? child.DataName;
            //return (T)Array.Find(info.InitialControlsData,
            //data => data.Name.Equals(name)).OutputValue;
            return true;
        }

        public static DataPair[] GetPickerOptions(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            child.DataName = null;
            var obj = GetControlData<object>(child, info);
            var arrayOfObject = ((IEnumerable)obj).Cast<object>().ToArray();
            var dict = new Dictionary<string, string>();

            DataPair[] options = new DataPair[arrayOfObject.Length];

            // it's giving me such casting trouble here, because
            // these are still NewtonSoft Json objects, so we need to re-deserilze them
            for (int i = 0; i < arrayOfObject.Length; i++)
            {
                var stringObj = JsonConvert.SerializeObject(arrayOfObject[i]);
                options[i] = JsonConvert.DeserializeObject<DataPair>(stringObj);
            }
            return options;
        }

        public static DataPair GetSelectedPickerOption(ComponentContainer child, StandAloneFormSessionInfo info, DataPair[] options)
        {
            var pair = Array.Find(info.InitialControlsData,
                    data => data.Name.Equals(child.SelectedItemDataNameKey));
            JArray outputValue = (JArray)pair.OutputValue;
            var tokens = outputValue.ToArray();
            var selectedId = tokens.Length > 0 ? tokens.First().ToString() : "";
            return Array.Find(options, option => option.Name.Equals(selectedId));
        }
        public static DataPair[] GetRadioButtonListOptions(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            child.DataName = null;
            var obj = GetControlData<object>(child, info);
            var arrayOfObject = ((IEnumerable)obj).Cast<object>().ToArray();
            var dict = new Dictionary<string, string>();

            DataPair[] options = new DataPair[arrayOfObject.Length];

            // it's giving me such casting trouble here, because
            // these are still NewtonSoft Json objects, so we need to re-deserilze them
            for (int i = 0; i < arrayOfObject.Length; i++)
            {
                var stringObj = JsonConvert.SerializeObject(arrayOfObject[i]);
                options[i] = JsonConvert.DeserializeObject<DataPair>(stringObj);
            }
            return options;
        }



        public static DataPair GetSelectedRadioButtonListOption(ComponentContainer child, StandAloneFormSessionInfo info, DataPair[] options)
        {
            var pair = Array.Find(info.InitialControlsData,
                    data => data.Name.Equals(child.SelectedItemDataNameKey));
            JArray outputValue = (JArray)pair.OutputValue;
            var tokens = outputValue.ToArray();
            var selectedId = tokens.Length > 0 ? tokens.First().ToString() : "";
            return Array.Find(options, option => option.Name.Equals(selectedId));
        }

        public static DatePicker GetDatePicker(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            DatePicker datePicker = new DatePicker();
            string dateString = GetControlData<string>(child, info);
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                datePicker.Date = date;
            }
            else
            {
                Console.Write($"Unable to parse date-string for control {child.DataName}");
            }
            return datePicker;
        }

        public static TimeSpan ParseTime(string input)
        {
            // TODO culture variation of time values...
            DateTime output;
            var ok = DateTime.TryParseExact(input, @"hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out output);
            if (ok != true)
            {
                Console.Write($"Unable to parse date-string {input}");
            }
            return output.Subtract(output.Date);
        }

        public static TimePicker GetTimePicker(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            TimePicker timePicker = new TimePicker();
            string timeString = GetControlData<string>(child, info);
            var span = ParseTime(timeString);
            timePicker.Time = span;
            return timePicker;
        }

        public static DataPair[] CollectData(Dictionary<string, FormControlWrapper> controls)
        {
            List<DataPair> list = new List<DataPair>();
            // iterate these things and get out data... as expcted data pairs
            foreach (KeyValuePair<string, FormControlWrapper> entry in controls)
            {
                DataPair[] value = entry.Value.GetValue();
                // filter out things that don't return a data-pair value, such as
                // labels or buttons. This _does not mean_ filtering DataPairs with
                // null OutputValue
                if (value != null)
                {
                    list.AddRange(value);
                }
            }
            return list.ToArray();
        }

        private static T GetControlData<T>(ComponentContainer child, StandAloneFormSessionInfo info)
        {
            string name = child.DataName ?? child.ComponentId;
            return (T)Array.Find(info.InitialControlsData,
                data => data.Name.Equals(name))?.OutputValue;
        }
    }
}
