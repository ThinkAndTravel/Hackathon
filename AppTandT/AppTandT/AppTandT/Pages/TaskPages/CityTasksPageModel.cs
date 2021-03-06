﻿using Acr.UserDialogs;
using AppTandT.BLL.Help;
using AppTandT.BLL.Services;
using AppTandT.Pages.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;

namespace AppTandT.Pages.TaskPages
{
    public class CityTasksPageModel : BasePageModel
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public CityTasksPageModel()
        {
            var list = new List<CityItem>();

            var city0 = new CityItem()
            {
                Name = "UNIT City",
                LogoUrl = "https://apply.unit.ua/assets/img/logo.png?v043"
            };
            var city1 = new CityItem()
            {
                Name = "Київ",
                LogoUrl = "http://www.studiokiev.co.nz/images/kiev_logo.png"
            };
            var city2 = new CityItem()
            {
                Name = "Paris",
                LogoUrl = "http://c7.alamy.com/comp/F0MN43/paris-vector-logo-design-template-eiffel-tower-drawn-in-a-simple-sketch-F0MN43.jpg"
            };
            var city3 = new CityItem()
            {   
                Name = "London",
                LogoUrl = "http://www.richardalanmoore.com/wp-content/uploads/2009/01/london_logo_black.gif"
            };
            var city4 = new CityItem()
            {
                Name = "Львів",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/1/13/%D0%9B%D0%BE%D0%B3%D0%BE%D1%82%D0%B8%D0%BF_%D0%9B%D1%8C%D0%B2%D0%BE%D0%B2%D0%B0_2007.png"
            };
            var city5 = new CityItem()
            {
                Name = "Kraków",
                LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b9/Krakow-seal.PNG/220px-Krakow-seal.PNG"
            };
            var city6 = new CityItem()
            {
                Name = "Warszawa",
                LogoUrl = "http://www.warszawa.pl/wp-content/themes/warszawapl/img/fblink.png"
            };
            var city7 = new CityItem()
            {
                Name = "New York",
                LogoUrl = "https://ksr-ugc.imgix.net/assets/015/948/830/9d0bc635f892278375dbead26628c49e_original.png?crop=faces&w=1552&h=873&fit=crop&v=1491489203&auto=format&q=92&s=b731a0c0aafde1872f33c2563c6c4997"
            };
            list.Add(city0);
            list.Add(city1);
            list.Add(city2);
            list.Add(city3);
            list.Add(city4);
            list.Add(city5);
            list.Add(city6);
            list.Add(city7);

            CityItems = new ObservableCollection<CityItem>(list);
            var tasks = new List<TaskItem>();
            tasks.Add(new TaskItem()
            {
                Title ="",
                LogoUrl= "https://cdn2.iconfinder.com/data/icons/freecns-cumulus/16/519660-164_QuestionMark-256.png",
                About = "Виберіть місто "
            });
            TaskItems = new ObservableCollection<TaskItem>(tasks);
            CitySelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>(async(arg) => 
            {
                SelectedCity = (CityItem)arg.SelectedItem;
                await Reload();
            });

            TaskSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
            {
                var pageModel = new TaskExecutionPageModel();
                pageModel.SetTaskId(((TaskItem)arg.SelectedItem).ID);
                var page = this.GetPageFromCache<TaskExecutionPageModel>(pageModel);
                var masterDetailPage =
                    this.GetPageFromCache<MainMasterDetailPageModel>();

                masterDetailPage.GetPageModel().SetDetail(page);
                SelectedTask = null;
            });
        }

        public CityItem SelectedCity { get; set; }
        public ICommand CitySelectedCommand { get; set; }

        public TaskItem SelectedTask { get; set; }
        public ICommand TaskSelectedCommand { get; set; }

        public ObservableCollection<CityItem> CityItems { get; set; }
        public ObservableCollection<TaskItem> TaskItems { get; set; }

        public async Task Reload()
        {
            try
            {
                var list = new List<TaskItem>();
                var selectedCity = SelectedCity;
                if (SelectedCity == null)
                    selectedCity = CityItems.First<CityItem>();
                // тут повинна бути загрузка списку завдань
                var downloadTasks = await TaskService.GetTasksForCityAsync(BLL.Sesion._id, SelectedCity.Name);
                foreach (var cur in downloadTasks)
                {
                    list.Add(
                        new TaskItem()
                        {
                            Title = cur.Title,
                            About = cur.About,
                            ID = cur._id,
                        });
                }
                if (SelectedCity.Name == "Київ")
                {
                    list.Add(new TaskItem()
                    {
                        Title = "Будинок з химерами",
                        LogoUrl = "http://bm.img.com.ua/berlin/storage/news/orig/7/e4/b0c8b53f913b6bec26e48ac0d3a28e47.jpg",
                        About = "Потрібно сфотографуватися з певною заданою фотографією химерою"
                    }
                    );
                    var task1 = new TaskItem()
                    {
                        Title = "Майдан Незалежності",
                        LogoUrl = "https://st2.depositphotos.com/1536490/11132/v/950/depositphotos_111323836-stock-illustration-maidan-nezalezhnosti-kiev.jpg",
                    };
                    list.Add(task1);

                }
                // Тут повинний бути список завдань для вибраного міста

                TaskItems = new ObservableCollection<TaskItem>(list);
                OnPropertyChanged("TaskItems");
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
    }

    public class TaskItem : BaseModel
    {
        public string LogoUrl { get; set; }

        public string About { get; set; }

        public string Title { get; set; }

        public string ID { get; set; }

        public ICommand Command { get; set; }
    }

    public class CityItem : BaseModel
    {
        public string Name { get; set; }

        public ICommand Command { get; set; }

        public string LogoUrl { get; set; }
    }

}

