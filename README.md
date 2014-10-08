DayStream
=========
Reasoning with days in C# using LINQ!
-----------

The DayStream class implements IEnumerable<DateTime> and can be used reason with days using LINQ and lambda expressions.

Best explained by some examples:

Count the number of wednesdays in a period:
```
var stream = new DayStream(new DateTime(2013, 2, 1), new DateTime(2013, 2, 28));
stream.Count(d => d.DayOfWeek == DayOfWeek.Wednesday);
```

Reason with "infinite" periods:
```
var stream = new DayStream(new DateTime(2013, 2, 1), DateTime.MaxValue);
stream.First(d => d.DayOfWeek == DayOfWeek.Sunday);
```

Reason with "infinite" periods back in history:
```
var stream = new DayStream(new DateTime(2013, 2, 1), DateTime.MinValue);
stream.First(d => d.DayOfWeek == DayOfWeek.Sunday);
```

Use all sorts of LINQ expressions (list al 3rd tuesdays in september for the next 100 years (dutch prinsjesdag) ):
```
new DayStream(DateTime.Today, DateTime.Today.AddYears(100))
	.Where( d => d.Month == 9 )
	.Where( d => d.DayOfWeek == DayOfWeek.Tuesday )
	.GroupBy( d => d.Year )
	.Select( g => g.Skip(2).First() );
```

Easy stream creation for a specific year
```
DayStream.ForYear(2014)
```