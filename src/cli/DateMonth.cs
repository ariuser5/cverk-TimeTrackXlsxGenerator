using System.Globalization;

struct DateMonth
{
	public const string Formats = 
		"""
		MM.yyyy, yyyy.MM, MM/yyyy, yyyy/MM, MM-yyyy, yyyy-MM, MM_yyyy, yyyy_MM,
		MM.yy, yy.MM, MM/yy, yy/MM, MM-yy, yy-MM, MM_yy, yy_MM
		""";

	public int Year { get; init; }
	public int Month { get; init; }

	public DateMonth(int year, int month)
	{
		this.Year = year;
		this.Month = month;
	}
	
	public string ToString(string format)
	{
		bool isValidFormat = ValidFormats.Contains(format);
		if (!isValidFormat) {
			throw new ArgumentException(
				$"Invalid format '{format}' for {typeof(DateMonth)} object."
			);
		}
		if (format == "MM.yyyy" || format == "MM/yyyy" || format == "MM-yyyy" || format == "MM_yyyy") {
			return $"{Month:D2}.{Year:D4}";
		}
		if (format == "yyyy.MM" || format == "yyyy/MM" || format == "yyyy-MM" || format == "yyyy_MM") {
			return $"{Year:D4}.{Month:D2}";
		}
		if (format == "MM.yy" || format == "MM/yy" || format == "MM-yy" || format == "MM_yy") {
			return $"{Month:D2}.{Year:D2}";
		}
		if (format == "yy.MM" || format == "yy/MM" || format == "yy-MM" || format == "yy_MM") {
			return $"{Year:D2}.{Month:D2}";
		}
		throw new ArgumentException(
			$"Invalid format '{format}' for {typeof(DateMonth)} object."
		);
	}
	
	public override string ToString()
	{
		return $"{Month:D2}/{Year:D4}";
	}
	
	public override bool Equals(object? obj)
	{
		return obj is DateMonth other && this == other;
	}
	
	public override int GetHashCode()
	{
		return HashCode.Combine(Year, Month);
	}
	
	

	private static string[] _validFormats = Formats.Split(",").Select(s => s.Trim()).ToArray();
	public static string[] ValidFormats => _validFormats;

	public static DateMonth Parse(string input)
	{
		bool isParsed = TryParse(input, out DateMonth result);
		if (!isParsed) {
			throw new FormatException(
				$"Invalid format '{input}' for {typeof(DateMonth)} object."
			);
		}
		return result;
	}
	
	public static bool TryParse(string input, out DateMonth result)
	{
		bool isParsed = DateTime.TryParseExact(
			s: input, 
			formats: ValidFormats, 
			provider: CultureInfo.InvariantCulture, 
			style: DateTimeStyles.None, 
			result: out DateTime dateTime);
			
		result = new DateMonth(dateTime.Year, dateTime.Month);
		return isParsed;
	}
	
	public static DateMonth Now()
	{
		DateTime now = DateTime.Now;
		return new DateMonth(now.Year, now.Month);
	}
	
	public static DateMonth FromDateTime(DateTime dateTime)
	{
		return new DateMonth(dateTime.Year, dateTime.Month);
	}
	
	public static DateMonth FromDateTimeOffset(DateTimeOffset dateTimeOffset)
	{
		return new DateMonth(dateTimeOffset.Year, dateTimeOffset.Month);
	}
	
	
	
	
	public static bool operator ==(DateMonth left, DateMonth right)
	{
		return left.Year == right.Year && left.Month == right.Month;
	}
	
	public static bool operator !=(DateMonth left, DateMonth right)
	{
		return !(left == right);
	}
	
	public static bool operator <(DateMonth left, DateMonth right)
	{
		return left.Year < right.Year || (left.Year == right.Year && left.Month < right.Month);
	}
	
	public static bool operator >(DateMonth left, DateMonth right)
	{
		return left.Year > right.Year || (left.Year == right.Year && left.Month > right.Month);
	}
	
	public static bool operator <=(DateMonth left, DateMonth right)
	{
		return left.Year <= right.Year || (left.Year == right.Year && left.Month <= right.Month);
	}
	
	public static bool operator >=(DateMonth left, DateMonth right)
	{
		return left.Year >= right.Year || (left.Year == right.Year && left.Month >= right.Month);
	}
	
	public static DateMonth operator +(DateMonth left, TimeSpan right)
	{
		DateTime dateTime = new DateTime(left.Year, left.Month, 1);
		dateTime += right;
		return new DateMonth(dateTime.Year, dateTime.Month);
	}
	
	public static DateMonth operator -(DateMonth left, TimeSpan right)
	{
		DateTime dateTime = new DateTime(left.Year, left.Month, 1);
		dateTime -= right;
		return new DateMonth(dateTime.Year, dateTime.Month);
	}
	
	public static TimeSpan operator -(DateMonth left, DateMonth right)
	{
		DateTime leftDateTime = new DateTime(left.Year, left.Month, 1);
		DateTime rightDateTime = new DateTime(right.Year, right.Month, 1);
		return leftDateTime - rightDateTime;
	}
}
