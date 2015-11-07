namespace Physicist.Controls.GUIControls
{
    public static class GUIExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "No new instances are created")]
        public static bool TrySet<T>(this NotifyProperty notify, ref T value, T newValue)
        {
            var change = false;
            if (notify != null)
            {
                if (!value.Equals(newValue))
                {
                    value = newValue;
                    change = true;
                }
            }

            return change;
        }
    }
}
