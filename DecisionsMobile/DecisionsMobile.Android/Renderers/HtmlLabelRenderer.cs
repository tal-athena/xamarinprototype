using Android.Content;
using Android.Text;
using Android.Widget;
using CustomRenderer;
using CustomRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace CustomRenderer.Android
{
    class HtmlLabelRenderer : LabelRenderer
    {

        public HtmlLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlLabel)Element;
            if (view == null) return;
            var text = view.Text != null ? view.Text.ToString() : "";
            Control.SetText(Html.FromHtml(text, FromHtmlOptions.ModeLegacy),
                TextView.BufferType.Spannable);
        }
    }
}