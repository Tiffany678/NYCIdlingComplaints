﻿using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SeleniumUtilities.Base;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.DevTools.V112.Network;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace SeleniumUtilities.Utils
{
    public static class RegistrationUtilities
    {
        public static readonly Random random = new Random();
        //  private static readonly int SLEEPTIMER = 1000;


        //This method will generate random Email address
        public static string GenerateEmail(this string firstName, string lastName, string domain)
        {
            string email = $"{firstName.ToLower()}.{lastName.ToLower()}@{domain}";
            return email;
        }

        //This method will generate random string without giving specific length
        public static string GenerateRandomString()
        {
            const string acceptedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //  var random = new Random(Guid.NewGuid().GetHashCode());
            string str = string.Empty;
            int length = random.Next(1, 20);
            for (int i = 0; i < length; i++)
            {
                str += acceptedChars[random.Next(0, acceptedChars.Length)];
            }
            return str;
        }

        //This method will generate a random number within the specified range
        public static int GenerateRandomNumberWithRange(this int minValue, int maxValue)
        {
            Random random = new Random();
            return random.Next(minValue, maxValue + 1);
        }

        //This method will generate a series random numbers
        public static string GenerateSeriesNumbers()
        {
            int length = random.Next(1, 20);

            string seriesRandomNumbers = "";

            for (int i = 0; i < length; i++)
            {
                int pickedNumber = random.Next(0, 9);
                seriesRandomNumbers += pickedNumber.ToString();
            }

            return seriesRandomNumbers;
        }

        //This method will generate regitimate password
        public static string GenerateQualifiedPassword()
        {
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialCharacters = "!@#$%^&*()";

            string allCharacters = uppercaseLetters + lowercaseLetters + numbers + specialCharacters;

            int Passwordlength = random.Next(8, 20);
            char[] password = new char[Passwordlength];
            password[0] = uppercaseLetters[random.Next(uppercaseLetters.Length)];
            password[1] = lowercaseLetters[random.Next(lowercaseLetters.Length)];
            password[2] = numbers[random.Next(numbers.Length)];
            password[3] = specialCharacters[random.Next(specialCharacters.Length)];

            for (int i = 4; i < password.Length; i++)
            {
                password[i] = allCharacters[random.Next(allCharacters.Length)];
            }

            password = password.OrderBy(x => random.Next()).ToArray();
            return new string(password);
        }


        /*This method will retrive the data from the file.
            The targetRowIndex and targetColumnIndex are starting from 0;*/
        public static string RetrieveRecordValue(this string filePath, int targetRowIndex, int targetColumnIndex)
        {
            try {
                string[] lines = File.ReadAllLines(filePath);

                if (targetRowIndex >= 0 && targetRowIndex < lines.Length)
                {
                    string row = lines[targetRowIndex];

                    string[] columns = row.Split(' ');

                    if (targetColumnIndex >= 0 && targetColumnIndex <= columns.Length - 1)
                    {
                        string targetValue = columns[targetColumnIndex];

                        return targetValue;

                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Target column index is out of range.");
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException("Target row index is out of range.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("An index out of range error occurred " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred " + ex.Message);
            }

            return "";
        }



        public static void ReplaceRecordValue(this string filePath, int targetRowIndex, int targetColumnIndex, string newValue)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] columns = lines[targetRowIndex].Split(' ');
                lines[targetRowIndex] = lines[targetRowIndex].Replace(columns[targetColumnIndex], newValue);
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot replace into File");
                Console.WriteLine(ex.ToString());
            }
        }

        public static void WriteIntoFile(this string filePath, string email, string password, string securityAnswer)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(email + " " + password + " " + securityAnswer);
                    Console.WriteLine("Accessed the file");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while appending the text to the file: {ex.Message}");
                Console.WriteLine("Cannot Write into File");
            }
        }
    }
}
