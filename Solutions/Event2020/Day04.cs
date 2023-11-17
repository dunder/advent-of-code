using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 4: Passport Processing ---
    public class Day04
    {
        private readonly ITestOutputHelper output;

        public Day04(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static List<(string, string)> ParsePassportRow(string passportRow)
        {
            var keyValues = passportRow.Split(" ").ToList();

            return keyValues
                .Select(keyValue => keyValue.Split(":").ToList())
                .Select(keyValueStrings => (keyValueStrings[0], keyValueStrings[1]))
                .ToList();
        }

        private static List<(string, string)> ParsePassportRows(IEnumerable<string> passportRows)
        {
            return passportRows.Select(ParsePassportRow).SelectMany(rows => rows.ToList()).ToList();
        }

        private static List<List<string>> BatchByPassport(IList<string> input)
        {
            return input
                .Split(string.IsNullOrEmpty)
                .Select(batch => batch.ToList())
                .ToList();
        }

        private static List<List<(string, string)>> Parse(IList<string> input)
        {
            List<List<string>> passportRowsByPassport = BatchByPassport(input);

            return passportRowsByPassport.Select(ParsePassportRows).ToList();
        }

        private static bool IsValid(List<(string, string)> passportValues)
        {
            var passportFields = passportValues.ToDictionary(values => values.Item1, values => values.Item2);

            return HasRequiredFields(passportFields);
        }

        private static bool IsValid2(List<(string, string)> passportValues)
        {
            var passportFields = passportValues.ToDictionary(values => values.Item1, values => values.Item2);

            if (!HasRequiredFields(passportFields)) { return false; }
            if (!IsValidBirthYear(passportFields)) { return false; }
            if (!IsValidIssueYear(passportFields)) { return false; }
            if (!IsValidExpirationYear(passportFields)) { return false; }
            if (!IsValidHeight(passportFields)) { return false; }
            if (!IsValidHairColor(passportFields)) { return false; }
            if (!IsValidEyeColor(passportFields)) { return false; }
            if (!IsValidPassportId(passportFields)) { return false; }

            return true;
        }

        private static bool HasRequiredFields(Dictionary<string, string> passportFields)
        {
            var requiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            return requiredFields.All(passportFields.ContainsKey);
        }

        private static bool IsValidNumericRange(string value, int from, int to)
        {
            bool isNumeric = int.TryParse(value, out int number);

            if (!isNumeric)
            {
                return false;
            }

            return number >= from && number <= to;
        }

        private static bool IsValidBirthYear(Dictionary<string, string> passportFields)
        {
            return IsValidNumericRange(passportFields["byr"], 1920, 2002);
        }

        private static bool IsValidIssueYear(Dictionary<string, string> passportFields)
        {
            return IsValidNumericRange(passportFields["iyr"], 2010, 2020);
        }

        private static bool IsValidExpirationYear(Dictionary<string, string> passportFields)
        {
            return IsValidNumericRange(passportFields["eyr"], 2020, 2030);
        }

        private static bool IsValidHeight(Dictionary<string, string> passportFields)
        {
            string heightValue = passportFields["hgt"];

            var inchesMatch = new Regex(@"^(\d+)in").Match(heightValue);
            
            if (inchesMatch.Success)
            {
                var inches = int.Parse(inchesMatch.Groups[1].Value);
                return inches >= 59 && inches <= 76;
            }
            
            var centimeterMatch = new Regex(@"(\d+)cm").Match(heightValue);

            if (centimeterMatch.Success)
            {
                var centimeters = int.Parse(centimeterMatch.Groups[1].Value);
                return centimeters >= 150 && centimeters <= 193;
            }

            return false;
        }

        private static bool IsValidHairColor(Dictionary<string, string> passportFields)
        {
            return new Regex(@"#[0-9a-f]{6}").IsMatch(passportFields["hcl"]);
        }

        private static bool IsValidEyeColor(Dictionary<string, string> passportFields)
        {
            return new Regex(@"^amb|blu|brn|gry|grn|hzl|oth$").IsMatch(passportFields["ecl"]);
        }

        private static bool IsValidPassportId(Dictionary<string, string> passportFields)
        {
            return new Regex(@"^[0-9]{9}$").IsMatch(passportFields["pid"]);
        }

        private static int CountValidPasspords(IList<string> input)
        {
            return Parse(input).Count(IsValid);
        }

        private static int CountValidPasspords2(IList<string> input)
        {
            return Parse(input).Count(IsValid2);
        }

        public int FirstStar()
        {
            var input = ReadLineInput();

            return CountValidPasspords(input);
        }

        public int SecondStar()
        {
            var input = ReadLineInput();

            return CountValidPasspords2(input);
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(260, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(153, SecondStar());
        }

        [Fact]
        public void FirstStarExample()
        {
            var example = new List<string>()
            {
                "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
                "byr:1937 iyr:2017 cid:147 hgt:183cm",
                "",
                "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
                "hcl:#cfa07d byr:1929",
                "",
                "hcl:#ae17e1 iyr:2013",
                "eyr:2024",
                "ecl:brn pid:760753108 byr:1931",
                "hgt:179cm",
                "",
                "hcl:#cfa07d eyr:2025 pid:166559648",
                "iyr:2011 ecl:brn hgt:59in",
            };

            Assert.Equal(2, CountValidPasspords(example));
        }

        [Fact]
        public void SecondStarExampleInvalid()
        {
            var example = new List<string>()
            {
                "eyr:1972 cid:100",
                "hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926",
                "",
                "iyr:2019",
                "hcl:#602927 eyr:1967 hgt:170cm",
                "ecl:grn pid:012533040 byr:1946",
                "",
                "hcl:dab227 iyr:2012",
                "ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277",
                "",
                "hgt:59cm ecl:zzz",
                "eyr:2038 hcl:74454a iyr:2023",
                "pid:3556412378 byr:2007"
            };

            Assert.Equal(0, CountValidPasspords2(example));
        }

        [Fact]
        public void SecondStarExampleValid()
        {
            var example = new List<string>()
            {
                "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980",
                "hcl:#623a2f",
                "",
                "eyr:2029 ecl:blu cid:129 byr:1989",
                "iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm",
                "",
                "hcl:#888785",
                "hgt:164cm byr:2001 iyr:2015 cid:88",
                "pid:545766238 ecl:hzl",
                "eyr:2022",
                "",
                "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719"
            };

            Assert.Equal(4, CountValidPasspords2(example));
        }
    }
}
