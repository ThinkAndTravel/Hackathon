using Acr.UserDialogs;
using AppTandT.BLL.Help;
using AppTandT.BLL.Model;
using AppTandT.BLL.Services;
using AppTandT.Pages.Menu;
using AppTandT.Pages.UserPages.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace AppTandT.Pages.UserPages
{
    public class LoginPageModel : BasePageModel
    {

        public string Login {
            get;
            set;
        }
        public string Password {
            get;
            set;
        }

        public LoginPageModel()
        {
            LoginButtonCommand = new BaseCommand((arg) =>
           LoginButtonCommandExecute());

            RegistryButtonCommand = new BaseCommand((arg) =>
            RegistryButtonCommandExecute());

        }

        /// <summary>
        /// Кнопка для реєстрації
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task RegistryButtonCommandExecute()
        {
            try
            {
                var pageToPush = this.GetPageFromCache<RegistryPageModel>();
                await this.PushPageAsync(pageToPush);
            }
            catch (ServiceException e)
            {
                var conf = new AlertConfig
                {
                    Message = e.Message,
                    Title = e.Title,
                    OkText = e.OkText
                };
                await UserDialogs.Instance.AlertAsync(conf);
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
            }
        }
        public ICommand RegistryButtonCommand {
            get;
            set;
        }

        /// <summary>
        /// Кнопка для входу
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task LoginButtonCommandExecute()
        {
            try
            {
                var loginModel = new LoginModel()
                {
                    Username = Login,
                    Password = Password
                };
                if (loginModel.isValid())
                {
                    var res = await IdentityService.LoginAsync(loginModel);
                    if (res != null)
                        throw new ServiceException(res);
                    var factory = new XamvvmFormsFactory(App.Current);
                    factory.RegisterNavigationPage<MainNavigationPageModel>(() => this.GetPageFromCache<MainMasterDetailPageModel>());
                    XamvvmCore.SetCurrentFactory(factory);
                    App.Current.MainPage = this.GetPageFromCache<MainNavigationPageModel>() as NavigationPage;

                }
            }
            catch (ServiceException e)
            {
                var conf = new AlertConfig
                {
                    Message = e.Message,
                    Title = e.Title,
                    OkText = e.OkText
                };
                await UserDialogs.Instance.AlertAsync(conf);
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
            }
        }
        public ICommand LoginButtonCommand {
            get;
            set;
        }
    }
}
