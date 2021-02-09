using System;

namespace DecisionsMobile.Views
{

    public class MainMenuPageMenuItem
    {
        public MainMenuPageMenuItem()
        {
            TargetType = typeof(MainMenuPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}