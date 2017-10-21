using GymProgUI.Services;
using GymProgUI.Views;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool DoesRequireLogin { get; set; }

        public BaseViewModel(bool doesRequireLogin=true)
        {
            DoesRequireLogin = doesRequireLogin;
        }


        protected void PushPage(Page page)
        {
            BaseViewModel pageBase = page.BindingContext as BaseViewModel;
            if (pageBase != null && pageBase.DoesRequireLogin && !BaseService.IsLogedIn)
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage() { Title = "Login", BindingContext = new LoginViewModel(page) });

            }
            else
            {
                Application.Current.MainPage.Navigation.PushAsync(page);
            }
        }

        public virtual void InitData()
        {
        }
    }
}