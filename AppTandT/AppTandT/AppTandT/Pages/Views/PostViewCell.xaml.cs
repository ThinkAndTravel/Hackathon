﻿using AppTandT.Pages.UserPages;
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
      
        public PostViewCell()
        {
            InitializeComponent();

            
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
        
        }
    
    }
}