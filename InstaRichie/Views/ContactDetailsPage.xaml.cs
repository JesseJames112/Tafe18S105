using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StartFinance.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public ContactDetailsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating table
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<ContactDetails>();
            var query = conn.Table<ContactDetails>();
            TransactionList.ItemsSource = query.ToList();
        }


        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // checks if account name is null
                if (txtFirstName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Please enter First name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (txtLastName.Text.ToString() == "")
                {
                    MessageDialog variableerror = new MessageDialog("Please enter Last name", "Oops..!");
                }
                else if (txtCompanyName.Text.ToString() == "")
                {
                    MessageDialog variableerror = new MessageDialog("Please enter Company name", "Oops..!");
                }
                else if (txtMobilePhone.Text.ToString() == "")
                {
                    MessageDialog variableerror = new MessageDialog("Please enter Mobile Phone", "Oops..!");
                }
                else
                {   // Inserts the data
                    conn.Insert(new ContactDetails()
                    {
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        CompanyName = txtCompanyName.Text,
                        MobilePhone = txtMobilePhone.Text,

                    });
                    Results();
                }

            }
            catch (Exception ex)
            {   // Exception to display when amount is invalid or not numbers
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }   // Exception handling when SQLite contraints are violated
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Account Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }

            }



        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this Account will delete all transactions of this account", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                // checks if data is null else inserts
                try
                {
                    string FirstName = ((ContactDetails)TransactionList.SelectedItem).FirstName;
                    string LastName = ((ContactDetails)TransactionList.SelectedItem).LastName;

                    var querydel = conn.Query<ContactDetails>("DELETE FROM ContactDetails WHERE FirstName='" + FirstName + "' AND LastName='" + LastName + "'");
                    Results();
                }

                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog.ShowAsync();
                }
            }
            else
            {
                //
            }
        }
    }
   }

