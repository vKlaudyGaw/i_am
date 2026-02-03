using Google.Cloud.Firestore;
using System.Globalization;

namespace i_am.Converters
{
    public class DateTimeToTimestampConverter : IFirestoreConverter<DateTime>
    {
        public object ToFirestore(DateTime value) => Timestamp.FromDateTime(value.ToUniversalTime());

        public DateTime FromFirestore(object value)
        {
            if (value is Timestamp timestamp)
            {
                return timestamp.ToDateTime();
            }
            throw new ArgumentException("Invalid value");
        }
    }
    public class TimeSpanToStringConverter : IFirestoreConverter<TimeSpan>
    {
        public object ToFirestore(TimeSpan value)
        {
            return value.ToString(@"hh\:mm\:ss");
        }

        public TimeSpan FromFirestore(object value)
        {
            if (value is string stringValue)
            {
                if (TimeSpan.TryParse(stringValue, out var result))
                {
                    return result;
                }
            }
            return TimeSpan.Zero;
        }
    }
}