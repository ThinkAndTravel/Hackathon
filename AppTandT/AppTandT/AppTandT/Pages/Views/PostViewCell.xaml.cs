using AppTandT.Pages.UserPages;
using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;

namespace AppTandT.Pages.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostViewCell : ViewCell
    {
        readonly SvgCachedImage heart = null;
        readonly SvgCachedImage chat = null;
        readonly SvgCachedImage sent = null;
        readonly SvgCachedImage bookmark = null;
        public int h = 0;

        public PostViewCell()
        {
            InitializeComponent();

            heart = new SvgCachedImage()
            {
                HorizontalOptions = LayoutOptions.Start,
                Source = "resource://TandT_App.Icons.svg.like.svg",
                HeightRequest = 40,
            };
            chat = new SvgCachedImage()
            {
                HorizontalOptions = LayoutOptions.Start,
                Source = "resource://TandT_App.Icons.svg.chat.svg",
                HeightRequest = 40,
            };
            sent = new SvgCachedImage()
            {
                HorizontalOptions = LayoutOptions.Start,
                Source = "resource://TandT_App.Icons.svg.sent.svg",
                HeightRequest = 40,
            };
            bookmark = new SvgCachedImage()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Source = "resource://TandT_App.Icons.svg.save.svg",
                HeightRequest = 40,
            };
            View.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new BaseCommand((arg) =>
                {
                    System.Diagnostics.Debug.WriteLine("POST TAPPED");
                })
            });
        }




        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var item = BindingContext as PostItem;
            if (item == null)
                return;
            MainImage.Source = item.MainImageUrl;
            LoginView.Text = item.Login;
            AvatarView.Source = item.AvatarUrl;
            AboutView.Text = item.About;
            var sh = "resource://TandT_App.Icons.svg.like.svg";
            if (item.h != 0) sh = "resource://TandT_App.Icons.svg.liked.svg";
            heart.Source = sh;
            item.HeartCommand = new BaseCommand((arg) =>
            {
                item.h = (item.h + 1) % 2;
                sh = "resource://TandT_App.Icons.svg.like.svg";
                if (item.h != 0) sh = "resource://TandT_App.Icons.svg.liked.svg";
                heart.Source = sh;
                System.Diagnostics.Debug.WriteLine("Like TAPPED");

            });
            heart.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = item.HeartCommand
            });

            SvgIcons.Children.Add(heart);

            chat.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = item.ChatCommand
            });
            SvgIcons.Children.Add(chat);

            sent.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = item.SentCommand
            });
            SvgIcons.Children.Add(sent);

            bookmark.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = item.SaveCommand
            });
            SvgIcons.Children.Add(bookmark);
        }
    }
}