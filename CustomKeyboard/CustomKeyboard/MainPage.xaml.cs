﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomKeyboard
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnKeyboardButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new KeyboardPage());
        }
    }
}
