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
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WP7_Workout.Classes
{
    [Table]
    public class Exercise : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _exerciseID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ExerciseID 
        {
            get 
            {
                return _exerciseID; 
            }
            set
            {
                if (_exerciseID != value)
                {
                    NotifyPropertyChanging("ExerciseID");
                    _exerciseID = value;
                    NotifyPropertyChanged("ExerciseID");
                }
            }
                    
        }

        private string _exerciseName;
        [Column]
        public string ExerciseName 
        {
            get
            {
                return _exerciseName;
            }
            set
            {
                if (_exerciseName != value)
                {
                    NotifyPropertyChanging("ExerciseName");
                    _exerciseName = value;
                    NotifyPropertyChanged("ExerciseName");
                }
            }
        }

        private string _exerciseInstruction;
        [Column]
        public string ExerciseInstruction 
        {
            get
            {
                return _exerciseInstruction;
            }
            set
            {
                if (_exerciseInstruction != value)
                {
                    NotifyPropertyChanging("ExerciseInstruction");
                    _exerciseInstruction = value;
                    NotifyPropertyChanged("ExerciseInstruction");
                }
            }
        }

        [Column(IsVersion = true)]
        private Binary version;
        

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }
}
