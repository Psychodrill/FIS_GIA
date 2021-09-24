namespace Ege.Check.Common.Hash
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using JetBrains.Annotations;

    public class FioHasher : IFioHasher
    {
        [ThreadStatic] private static CultureInfo _ruCulture;
        [NotNull] private static readonly IDictionary<char, char> Replacements = new ConcurrentDictionary<char, char>();
        [NotNull]
        private readonly char[] _hex = new char[16]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
        };

        static FioHasher()
        {
            Replacements.Add('ё', 'е');
            Replacements.Add('й', 'и');
        }

        public string GetHash(string surname, string firstName, string patronymic)
        {
            var chars = new List<char>(GetLength(surname) + GetLength(firstName) + GetLength(patronymic));
            Add(chars, surname);
            Add(chars, firstName);
            Add(chars, patronymic);

            byte[] hash;
            using (var md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(chars.ToArray()));
            }
            var result = new StringBuilder(32);
            foreach (var hashByte in hash)
            {
                result.Append(_hex[hashByte >> 4]);
                result.Append(_hex[hashByte & 0xF]);
            }
            return result.ToString();
        }

        private void Add(List<char> chars, string str)
        {
            if (str == null)
            {
                return;
            }
            for (int i = 0; i < str.Length; ++i)
            {
                var @char = str[i];
                var lower = ToLower(@char);
                if (!IsValidChar(lower))
                {
                    continue;
                }
                chars.Add(ApplyReplace(lower));
            }
        }

        private int GetLength(string str)
        {
            return str != null ? str.Length : 0;
        }

        public bool AreEqual(string name1, string name2)
        {
            if (name1 == null || name2 == null)
            {
                return name1 == null && name2 == null;
            }
            return name1.SequenceEqual(name2, ReplacementCharEqualityComparer.Instance);
        }

        private char ToLower(char c)
        {
            return char.ToLower(c, _ruCulture ?? (_ruCulture = CultureInfo.CreateSpecificCulture("RU-ru")));
        }

        private bool IsValidChar(char lowerCaseChar)
        {
            return (lowerCaseChar >= 'a' && lowerCaseChar <= 'z') || (lowerCaseChar >= 'а' && lowerCaseChar <= 'я') ||
                   lowerCaseChar == 'ё';
        }

        private char ApplyReplace(char c)
        {
            char result;
            return Replacements.TryGetValue(c, out result)
                       ? result
                       : c;
        }

        public class ReplacementCharEqualityComparer : IEqualityComparer<char>
        {
            public static IEqualityComparer<char> Instance = new ReplacementCharEqualityComparer(); 

            public bool Equals(char x, char y)
            {
                char repl;
                return x == y || (Replacements.TryGetValue(x, out repl) && repl == y) || (Replacements.TryGetValue(y, out repl) && repl == x);
            }

            public int GetHashCode(char obj)
            {
                return 0;
            }
        }
    }
}