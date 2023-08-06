using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercises
{
    public static class GroupByQuerySyntax
    {
        //Coding Exercise 1
        /*
         Using LINQ's query syntax, implement the GroupByFirstDigit method, 
        which given a collection of numbers will group them by the first digit, 
        and return a collection of strings with information about each group.

        For example, for numbers {1, 11, 44, 4, 62, 672, 921} the result shall be:
            "FirstDigit: 1, numbers: 1,11",
            "FirstDigit: 4, numbers: 44,4",
            "FirstDigit: 6, numbers: 62,672",
            "FirstDigit: 9, numbers: 921"
         */
        public static IEnumerable<string> GroupByFirstDigit(IEnumerable<int> numbers)
        {
            //TODO your code goes here

            var res1 = from number in numbers
                group number by number.ToString().ToCharArray().ElementAt(0)
                into g
                select $"FirstDigit: {g.Key}, numbers: {string.Join(",", g.Select(x => x).ToList()) }";

            var res2 = from number in numbers
                group number by number.ToString()[0]
                into g
                let values = string.Join(",", g.Select(x => x))
                select $"FirstDigit: {g.Key}, numbers: {values}";

            return res2;
        }

        //Coding Exercise 2
        /*
        Using LINQ's query syntax, implement the GroupByDayOfWeek method, 
        which given a collection of dates will return a Dictionary<DayOfWeek, DateTime>,
        where the day of the week will be the key, and the latest date with this 
        day of the week will be the value.

        For example, for the following dates:
            *new DateTime(2021, 10, 17) (Sunday)
            *new DateTime(2021, 10, 10) (Sunday)
            *new DateTime(2021, 10, 24) (Sunday)
            *new DateTime(2021, 10, 11) (Monday)
            *new DateTime(2021, 10, 4) (Monday)
        
        ...the result will be the following dictionary:        
            *[DayOfWeek.Sunday] = new DateTime(2021, 10, 24)
            *[DayOfWeek.Monday] = new DateTime(2021, 10, 11)
        
        Please note that those dates have been selected as values because for 
        given days of the week they are the latest dates.
         */
        public static Dictionary<DayOfWeek, DateTime> GroupByDayOfWeek(
            IEnumerable<DateTime> dates)
        {
            //TODO your code goes here

            return (from date in dates
                    group date by date.DayOfWeek
                    into g 
                    select g
                ).ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x).Max());

        }

        //Refactoring challenge
        //TODO implement this method
        public static IEnumerable<string> GetOwnersWithMultipleHouses_Refactored(
            IEnumerable<House> houses)
        {
            //TODO your code goes here

            var res1 = (from house in houses
                    group house by house.OwnerId
                    into g
                    select g)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var houses = g.Select(x => x).ToList();

                        return houses.Count() > 1 ?
                            $"Owner with ID {g.Key} " +
                            $"owns houses: " +
                            $"{string.Join(", ", houses)}"
                                : string.Empty;
                    })
                .Where(x => x.Value != String.Empty)
                .Select(x => x.Value).ToList();


            var res2 = from house in houses
                group house by house.OwnerId
                into g
                where g.Count() > 1
                select $"Owner with ID {g.Key} " +
                       $"owns houses: " +
                       $"{string.Join(", ", g.Select(h => h))}";

            return res2;
        }

        //do not modify this method
        public static IEnumerable<string>
            GetOwnersWithMultipleHouses(
                IEnumerable<House> houses)
        {
            var ownersHouses = new Dictionary<int, List<House>>();
            foreach (var house in houses)
            {
                if (!ownersHouses.ContainsKey(house.OwnerId))
                {
                    ownersHouses[house.OwnerId] = new List<House>();
                }
                ownersHouses[house.OwnerId].Add(house);
            }

            var result = new List<string>();
            foreach (var keyValue in ownersHouses)
            {
                if (keyValue.Value.Count > 1)
                {
                    result.Add(
                        $"Owner with ID {keyValue.Key} " +
                        $"owns houses: " +
                        $"{string.Join(", ", keyValue.Value)}");
                }
            }
            return result;
        }

        public class House
        {
            public int OwnerId { get; }
            public string Address { get; }

            public House(int ownerId, string address)
            {
                OwnerId = ownerId;
                Address = address;
            }

            public override string ToString()
            {
                return $"{Address}";
            }
        }
    }
}
