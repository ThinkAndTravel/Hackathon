using AppTandT.BLL.Help;
using AppTandT.BLL.Model;
using AppTandT.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamvvm;

namespace AppTandT.Pages.UserPages.Identity
{
    public class RegistryPageModel : BasePageModel
    {
        private RegistryModel registryModel { get; set; }

        public string Login {
            get;
            set;
        }
        public string Pas {
            get;
            set;
        }
        public string Email {
            get;
            set;
        }
        public string ConfirmPas {
            get;
            set;
        }
        public string FirstName {
            get;
            set;
        }
        public string LastName {
            get;
            set;
        }



        public RegistryPageModel()
        {
            LoginButtonCommand = BaseCommand.FromTask<string>((param) =>
            LoginButtonCommandExecute(param));

            RegistryButtonCommand = BaseCommand.FromTask<string>((param) =>
            RegistryButtonCommandExecute(param));

        }
        /// <summary>
        /// Кнопка для реєстрації
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task RegistryButtonCommandExecute(string param)
        {
            try
            {
                var registryModel = new RegistryModel()
                {
                    Login = Login,
                    Email = Email,
                    Pas = Pas,
                    ConfirmPas = ConfirmPas,
                    FirstName = FirstName,
                    LastName = LastName
                };

                if (registryModel.isValid() && App.isConnected())
                {
                    var message = await IdentityService.RegistryAsync(registryModel);
                    await this.PopPageAsync();
                }
            }
            catch (ServiceException e)
            {
             /*   var conf = new AlertConfig
                {
                    Message = e.Message,
                    Title = e.Title,
                    OkText = e.OkText
                };
                await UserDialogs.Instance.AlertAsync(conf);*/
            }
            catch
            {
             //   await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
            }
        }
        public ICommand RegistryButtonCommand {
            get;
            set;
        }

        /// <summary>
        /// Кнопка для повернення на логін сторінку
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task LoginButtonCommandExecute(string param)
        {
            try
            {
                await this.PopPageAsync();
            }
            catch (ServiceException e)
            {
            /*    var conf = new AlertConfig
                {
                    Message = e.Message,
                    Title = e.Title,
                    OkText = e.OkText
                };
                await UserDialogs.Instance.AlertAsync(conf);*/
            }
            catch
            {
             //   await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
            }

        }
        public ICommand LoginButtonCommand {
            get;
            set;
        }

    }
}
