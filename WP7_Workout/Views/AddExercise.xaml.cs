using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WP7_Workout.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace WP7_Workout.Views
{
    public partial class AddExercise : PhoneApplicationPage
    {        
        public AddExercise()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);
            App.CurrentApp.DB.SubmitChanges();
        }

        private void exerciseNameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplicationBarIconButton saveButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            if (exerciseNameTxtBox.Text == "" || exerciseInstructionTxtBox.Text == "")
            {                
                saveButton.IsEnabled = false;
            }
            else
            {
                saveButton.IsEnabled = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Exercise newEx = new Exercise()
            {
                ExerciseInstruction = exerciseInstructionTxtBox.Text,
                ExerciseName = exerciseNameTxtBox.Text
            };
            App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(newEx);
            NavigationService.GoBack();
        }
    }
}