using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace AppTandT.Pages.Menu
{
    public class MenuPageMasterModel : BasePageModel
    {
        public ObservableCollection<MenuItem> Items { get; set; }

        public MenuItem SelectedItem { get; set; }

        public ICommand ItemSelectedCommand { get; set; }

        public MenuPageMasterModel()
        {
            ItemSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
            {
                SelectedItem = null;
            });


            Items = new ObservableCollection<MenuItem>();

            var menuItems = new List<MenuItem>()
            {
                new MenuItem() {
                    Title = "Home",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            var page = this.GetPageFromCache<UserPostsPageModel>();
                            var masterDetailPage =
                                this.GetPageFromCache<MainMasterDetailPageModel>();
                            masterDetailPage.GetPageModel().SetDetail(page);
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    }),
                    Icon = "https://d30y9cdsu7xlg0.cloudfront.net/png/77002-200.png"
                },

                new MenuItem()
                {
                    Title = "Friends",
                    BackColor = Color.BlueViolet,
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            var masterDetailPage = this.GetPageFromCache<MainMasterDetailPageModel>();
                            throw new TandT.XBLL.Helpers.ServiceException("Friends. This will be implemented in the future!");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "Tasks",
                    Details ="Get tasks for city",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            var page = this.GetPageFromCache<CityTasksPageModel>();
                            var masterDetailPage =
                                this.GetPageFromCache<MainMasterDetailPageModel>();
                            masterDetailPage.GetPageModel().SetDetail(page);
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "News",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            throw new TandT.XBLL.Helpers.ServiceException("News. This will be implemented in the future!");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "Tasks",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            throw new TandT.XBLL.Helpers.ServiceException("Tasks. This will be implemented in the future!");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "My tasks",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            throw new TandT.XBLL.Helpers.ServiceException("My tasks. This will be implemented in the future!");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "Plans",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            throw new TandT.XBLL.Helpers.ServiceException("Plans. This will be implemented in the future!");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "Logout",

                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            TandT.XBLL.Sesion.CloseSession();
                            var page = this.GetPageFromCache<LoginPageModel>();
                            await this.PushPageAsync(page);
                            this.ClearPageCache();
                            throw new TandT.XBLL.Helpers.ServiceException("We hope to see you again ;)");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },

                new MenuItem() {
                    Title = "CV",
                    Command = new BaseCommand(async (param) =>
                    {
                        try{
                            throw new TandT.XBLL.Helpers.ServiceException("Doozer's stuff");
                        }catch (TandT.XBLL.Helpers.ServiceException e)
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
                    })
                },



            };

            Items = new ObservableCollection<MenuItem>(menuItems);



        }



        public class MenuItem : BaseModel
        {

            public string Title { get; set; }

            public string Icon { get; set; } = "";

            public string Details { get; set; } = "";

            public Color BackColor { get; set; } = Color.White;

            public ICommand Command { get; set; }
        }
    }
}