namespace Physicist.Controls.GUIControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class NotifyProperty : INotifyPropertyChanged
    {    
        public event PropertyChangedEventHandler PropertyChanged;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "No new instances are created")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Follows INotifyPropertyChanged Pattern")]
        protected bool TrySetNotify<T>(ref T value, T newValue, [CallerMemberName] string propertyName = "")
        {
            var change = false;
            if (!value.Equals(newValue))
            {
                value = newValue;
                this.NotifyPropertyChanged(propertyName);
                change = true;
            }

            return change;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Follows INotifyPropertyChanged Pattern")]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
