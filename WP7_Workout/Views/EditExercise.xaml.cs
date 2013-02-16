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
using Microsoft.Phone.Shell;
using WP7_Workout.Classes;

namespace WP7_Workout.Views
{
    public partial class EditExercise : PhoneApplicationPage
    {
        Exercise selectedExercise;

        public EditExercise()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.CurrentApp.DB.SubmitChanges();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            selectedExercise = (Exercise)(from exer in App.CurrentApp.DB.ExerciseTable
                          where exer.ExerciseID.ToString() == this.NavigationContext.QueryString["id"]
                          select exer).Single();

            exerciseNameTxtBox.Text = selectedExercise.ExerciseName;
            exerciseInstructionTxtBox.Text = selectedExercise.ExerciseInstruction;
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
            selectedExercise.ExerciseName = exerciseNameTxtBox.Text;
            selectedExercise.ExerciseInstruction = exerciseInstructionTxtBox.Text;
            NavigationService.GoBack();
        }
    }
}