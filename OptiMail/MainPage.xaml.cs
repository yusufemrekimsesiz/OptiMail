using Microsoft.Extensions.DependencyInjection;
using OptiMail.ViewModels;

namespace OptiMail
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnSettingsClicked(object? sender, EventArgs e)
        {
            var settingsPage = App.Current!.Handler!.MauiContext!.Services.GetRequiredService<SettingsPage>();
            await Navigation.PushAsync(settingsPage);
        }
    }
}