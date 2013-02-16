using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace WP7_Workout.Classes
{
    public class DBDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/DataStorage.sdf";

        public DBDataContext() : base("isostore:/DataStorage.sdf")
        {
        }

        public Table<Exercise> ExerciseTable;
        public Table<Workout> WorkoutTable;
        public Table<Routine> RoutineTable;
    }
}
