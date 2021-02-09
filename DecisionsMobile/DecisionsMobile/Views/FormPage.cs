using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DecisionsMobile.Views
{

    public class FormPage : ContentPage
    {
        private FormViewModel viewModel;

        private bool mounted = true;
        private bool useNamedSession;

        // if we wanted to update individual controls on the fly, we'd need to easily
        // find them by ID or data name, etc. This could just be a list if we don't though.
        Dictionary<string, FormControlWrapper> controls = new Dictionary<string, FormControlWrapper>();

        public FormPage(FormViewModel formViewModel, bool useNamedSession = false)
        {
            InitModel(formViewModel);
            this.useNamedSession = useNamedSession;
        }

        private void InitModel(FormViewModel formViewModel)
        {
            BindingContext = viewModel = formViewModel;
            Content = new ScrollView();
            this.Title = formViewModel.Title;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RenderForm(viewModel.FormInfo);

            Device.StartTimer(TimeSpan.FromMinutes(2), () =>
            {
                if (!mounted)
                {
                    return false;
                }
                // this doesn't need to be awaited, it's ok for it to be fire and forget
                _ = FormService.KeepFormAlive(this.viewModel.FormInfo.FormSessionInfoId);
                return true;
            });
        }


        protected override void OnDisappearing()
        {
            mounted = false;
            if (viewModel.Submitted != true)
            {
                // TODO service call that form was closed.
                FormService.FormClosed(viewModel.FlowTrackingId, viewModel.StepTrackingId);
            }
            base.OnDisappearing();
        }

        // holdover from initial "offline" proof of concept, for reference.
        //private StandAloneFormSessionInfo LoadForm()
        //{
        //    // testing this as synchronous... because aysnc was swallowing errors...
        //    StandAloneFormSessionInfo formList = ((FormsDataStore)viewModel.DataStore).GetGridSample() as StandAloneFormSessionInfo;
        //    StandAloneFormSessionInfo info = formList; // derrrrrpp
        //    RenderForm(info);
        //    return info;
        //}

        private void RenderForm(StandAloneFormSessionInfo info)
        {
            // initialize collection:
            controls = new Dictionary<string, FormControlWrapper>();
            // root container:
            ComponentContainer container = info.FormSurface.RootContainer;

            if (container.ServerType.Contains("SilverHorizontalStack"))
            {
                ((ScrollView)Content).HorizontalOptions = LayoutOptions.Fill;
                ((ScrollView)Content).Orientation = ScrollOrientation.Horizontal;
            }
            ((ScrollView)Content).Content = RenderLayout(container, info);
            // tell the sever we're done loading the form;
            _ = FormService.FormLoadComplete(info.FormSessionInfoId);
        }

        private View RenderLayout(ComponentContainer container, StandAloneFormSessionInfo info)
        {
            if (container.ServerType.Contains("GridLayout"))
            {
                return RenderGrid(container, info);
            }
            else if (container.ServerType.Contains("SilverHorizontalStack"))
            {
                return RenderStackLayout(container, info, StackOrientation.Horizontal);
                
            }
            else if (container.ServerType.Contains("SilverVerticalStack"))
            {
                return RenderStackLayout(container, info, StackOrientation.Vertical);                
            }
            else
            {
                return new Grid();
            }
        }
       
        private StackLayout RenderStackLayout(ComponentContainer decisionsLayout, StandAloneFormSessionInfo info, StackOrientation orientation)
        {
            StackLayout layout = new StackLayout
            {
                Orientation = orientation,
            };
            foreach (var element in decisionsLayout.Children)
            {
                View view = null;

                if (element.Child.ServerType.Contains("GridLayout") || element.Child.ServerType.Contains("SilverVerticalStack") || element.Child.ServerType.Contains("SilverHorizontalStack"))
                {
                    view = RenderLayout(element.Child, info);
                } 
                else
                {
                    FormControlWrapper control = FormUtils.GetComponentView(element, info);
                    view = control.View;
                
                    // track elements by id:
                    controls.Add(element.Child.ComponentId, control);

                    // add any button handlers also during this iteration:
                    if (control.GetType().Equals(typeof(ButtonControl)))
                    {
                        AddButtonHandler((ButtonControl)control);
                    }
                    else if (control.GetType().Equals(typeof(ImageButtonControl)))
                    {
                        AddImageButtonHandler((ImageButtonControl)control);
                    }
                }

                if (orientation == StackOrientation.Horizontal)
                    view.VerticalOptions = LayoutOptions.Start;
                layout.Children.Add(view);
            }

            return layout;
        }


        private Grid RenderGrid(ComponentContainer decisionsGrid, StandAloneFormSessionInfo info)
        {
            Grid grid = new Grid
            {
                Margin = decisionsGrid.MarginTop,
                RowSpacing = decisionsGrid.RowGap,
                ColumnSpacing = decisionsGrid.ColumnGap
            };

            foreach (GridLayoutPart row in decisionsGrid.Rows)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(row.RequestedSize, FormUtils.GetGridUnitType(row.LayoutType))
                });
            }

            foreach (GridLayoutPart col in decisionsGrid.Columns)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(col.RequestedSize, FormUtils.GetGridUnitType(col.LayoutType))
                });
            }

            foreach (ChildElement element in decisionsGrid.Children)
            {
                View view = null;
                if (element.Child.ServerType.Contains("GridLayout") || element.Child.ServerType.Contains("SilverVerticalStack") || element.Child.ServerType.Contains("SilverHorizontalStack"))
                {
                    view = RenderLayout(element.Child, info);
                }
                else
                {
                    FormControlWrapper control = FormUtils.GetComponentView(element, info);

                    view = control.View;
                  
                    // track elements by id:
                    controls.Add(element.Child.ComponentId, control);

                    // add any button handlers also during this iteration:
                    if (control.GetType().Equals(typeof(ButtonControl)))
                    {
                        AddButtonHandler((ButtonControl)control);
                    }
                    else if (control.GetType().Equals(typeof(ImageButtonControl)))
                    {
                        AddImageButtonHandler((ImageButtonControl)control);
                    }
                }

                grid.Children.Add(view, element.Column, element.Row);
                Grid.SetColumnSpan(view, element.ColumnSpan);
                Grid.SetRowSpan(view, element.RowSpan);
            }

            return grid;
        }

        private void AddButtonHandler(ButtonControl btn)
        {
            (btn.View as Button).Clicked += (sender, args) =>
            {
                OnButtonClick(btn.Button, btn.ElementData);
            };
        }

        private void AddImageButtonHandler(ImageButtonControl btn)
        {
            (btn.View as Button).Clicked += (sender, args) =>
            {
                OnButtonClick(btn.ImageButton, btn.ElementData);
            };
        }

        private async void OnButtonClick(Button button, ChildElement elementData)
        {
            button.IsEnabled = false;
            IsBusy = true;
            try
            {
                string outcomePathName = elementData.Child.OutcomePathName;

                string validationMessage = "";
                if (!FormService.Validate(controls, outcomePathName, out validationMessage))
                {
                    await DisplayAlert("Validation Issues", validationMessage, "OK");
                    return;
                }

                var data = FormUtils.CollectData(controls);
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    DecisionsFormInfoEvent e = await FormService.SelectPath(viewModel.FormInfo.FormSessionInfoId, outcomePathName, data, useNamedSession);

                    Debug.Write(e);
                    if (e == null)
                    {
                        await DisplayAlert("Error", "Problem with next form instruction.", "OK");
                        return;
                    }

                    var validationMessages = e.GetValidationMessages();

                    if (validationMessages.Length > 0)
                    {
                        string message = "";
                        Array.ForEach(validationMessages, msg => message += $"{msg}.\n\n");
                        await DisplayAlert("Validation Issues", message, "OK");

                        // TODO highlight the offending components
                        Array.ForEach(e.CurrentValidations, validation =>
                        {
                            FormControlWrapper control;
                            controls.TryGetValue(validation.ComponentID, out control);
                            // just show breaking issues for cut 1:
                            var breakingIssue = Array.Find(validation.ValidationIssues, issue => issue.BreakLevel == BreakLevel.Fatal);
                            if (control != null && breakingIssue != null)
                            {
                                control.SetValidation(breakingIssue.BreakLevel);
                            }
                        });
                    }

                    // if there were no validations to show, there should be next instructions
                    if (e.IsFlowCompletedInstructionEvent())
                    {
                        // Close this page. The flow's forms are done.
                        viewModel.Submitted = true;
                        await Navigation.PopAsync();

                        DependencyService.Get<ISnackbar>().ShortAlert("Form submitted successfully.");
                    }
                    else if (!String.IsNullOrEmpty(e.FlowTrackingId) && !String.IsNullOrEmpty(e.StepTrackingId))
                    {
                        var flowInstr = await FlowExecutionService.GetInstructionsForStep(e.FlowTrackingId, e.StepTrackingId, useNamedSession);
                        if (FlowExecutionService.IsShowFormType(flowInstr))
                        {
                            // render the next form in this page.
                            ShowNextForm(flowInstr);
                        }
                        else
                        {
                            // close this page? Not sure how we'd get here...
                            Console.WriteLine("Found Next Instruction was not a form instruction!");
                            viewModel.Submitted = true;
                            await Navigation.PopAsync();

                            DependencyService.Get<ISnackbar>().ShortAlert("Form submitted successfully.");

                            if (useNamedSession)
                            {
                                MessagingCenter.Send(this, Message.ACCOUNT_CREATED);
                            }
                        }
                    }
                    else
                    {
                        // did I missunderstand the structure?
                        Debug.WriteLine(e);
                    }
                }
                else
                {
                    if (viewModel.CanRunOffline)
                    {
                        var ret = await OfflineService.Instance.SaveOfflineFormSubmission(viewModel.ServiceCategoryId, data, outcomePathName, viewModel.FormInfo.FormRules);
                        viewModel.Submitted = true;
                        await Navigation.PopAsync();

                        DependencyService.Get<ISnackbar>().ShortAlert("Form saved, and will submit when online.");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Connection lost. Please, try again later.", "OK");
                    }

                }
            } catch (Exception ex)
            {
                Debug.WriteLine("FormPage Submit---", ex.Message);
                await DisplayAlert("Error", "Something went wrong! Please try again later.", "OK");
            }
            finally
            {
                IsBusy = false;
                button.IsEnabled = true;
            }
            
        }

        private async void ShowNextForm(FlowExecutionStateInstruction instruction)
        {
            // get the form JSON
            StandAloneFormSessionInfo formModel = await FormService.GetFormSessionSurfaceJson(instruction, useNamedSession);
            FormViewModel formViewModel = new FormViewModel(formModel, instruction);

            if (formModel == null)
            {
                await DisplayAlert("Error", "Problem loading form data.", "OK");
                return;
            }
            InitModel(formViewModel);
            RenderForm(formModel);
        }
    }
}

