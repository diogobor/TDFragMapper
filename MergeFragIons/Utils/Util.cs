/**
 * Program:     ProteoCombiner - Integrating bottom-up & top-down proteomics data
 * Author:      Diogo Borges Lima
 * Created:     4/3/2019
 * Update by:   Diogo Borges Lima
 * Description: Util class
 */
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MergeFragIons.Utils
{
    public static class Util
    {
        public const double HYDROGENMASS = 1.00782503214;

        private const char CR = '\r';
        private const char LF = '\n';
        private const char NULL = (char)0;

        /// <summary>
        /// Method responsible for counting lines of a file
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long CountLines(StreamReader stream)
        {
            Ensure.ArgumentNotNull(stream, nameof(stream));

            var lineCount = 0L;

            char[] byteBuffer = new char[1024 * 1024];
            const int BytesAtTheTime = 4;
            var detectedEOL = NULL;
            var currentChar = NULL;

            int bytesRead;
            while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
            {
                var i = 0;
                for (; i <= bytesRead - BytesAtTheTime; i += BytesAtTheTime)
                {
                    currentChar = (char)byteBuffer[i];

                    if (detectedEOL != NULL)
                    {
                        if (currentChar == detectedEOL) { lineCount++; }

                        currentChar = (char)byteBuffer[i + 1];
                        if (currentChar == detectedEOL) { lineCount++; }

                        currentChar = (char)byteBuffer[i + 2];
                        if (currentChar == detectedEOL) { lineCount++; }

                        currentChar = (char)byteBuffer[i + 3];
                        if (currentChar == detectedEOL) { lineCount++; }
                    }
                    else
                    {
                        if (currentChar == LF || currentChar == CR)
                        {
                            detectedEOL = currentChar;
                            lineCount++;
                        }
                        i -= BytesAtTheTime - 1;
                    }
                }

                for (; i < bytesRead; i++)
                {
                    currentChar = (char)byteBuffer[i];

                    if (detectedEOL != NULL)
                    {
                        if (currentChar == detectedEOL) { lineCount++; }
                    }
                    else
                    {
                        if (currentChar == LF || currentChar == CR)
                        {
                            detectedEOL = currentChar;
                            lineCount++;
                        }
                    }
                }
            }

            if (currentChar != LF && currentChar != CR && currentChar != NULL)
            {
                lineCount++;
            }
            return lineCount;
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        /// <summary>
        /// Method responsible for checking whether is possible to read files inside of a specific folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool UserHasAccess(String folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds =
                  System.IO.Directory.GetAccessControl(folderPath);

                //Check if is a RAW Waters file
                if (folderPath.ToLower().Contains(".raw"))
                {
                    return false;
                }
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetWindowsVersion()
        {
            string platform = System.Environment.OSVersion.Platform.ToString();
            if (platform.ToLower().Contains("nt"))
            {
                platform = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", null).ToString();
            }
            else
            {
                platform = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion", "ProductName", null).ToString();
            }
            return platform;
        }

        public static string GetProcessorName()
        {
            string processorName = "?";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            try
            {
                foreach (ManagementObject share in searcher.Get())
                {
                    processorName = share["Name"].ToString();
                    break;
                }
            }
            catch (Exception) { }

            return processorName;
        }

        public static string GetRAMMemory()
        {
            string memory = "?";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            try
            {
                foreach (ManagementObject share in searcher.Get())
                {
                    double memoryInBytes = Convert.ToDouble(share["TotalVisibleMemorySize"].ToString());
                    memoryInBytes /= (1024 * 1024);
                    memory = Math.Round(memoryInBytes, 2).ToString() + " GB";
                    break;
                }
            }
            catch (Exception) { }

            return memory;
        }

        public static bool Is64Bits()
        {
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// Removes the enzime cut specifications from the peptide
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public static string CleanPeptide(string peptide, bool removeParenthesis)
        {
            string cleanedPeptide = Regex.Replace(peptide, @"^[A-Z|\-|\*]+\.", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\.[A-Z|\-|\*]+$", "");
            cleanedPeptide = cleanedPeptide.Replace("*", "");
            cleanedPeptide = cleanedPeptide.Replace("#", "");

            if (removeParenthesis)
            {
                cleanedPeptide = Regex.Replace(cleanedPeptide, @"\([0-9|\.|\+|\-| |a-z|A-Z]*\)", "");
            }

            return cleanedPeptide;
        }

        /// <summary>
        /// Returns the parts per million error of two mass spectral numbers (eg theoretical and measured, and the program will return the 
        /// )
        /// </summary>
        /// <param name="n1">measurement 1</param>
        /// <param name="n2">measurement 2</param>
        /// <returns></returns>
        public static double PPM(double n1, double n2)
        {
            return ((Math.Abs(n1 - n2) * 1000000) / n2);
        }

        public static decimal PPM(decimal n1, decimal n2)
        {
            return ((Math.Abs(n1 - n2) * 1000000) / n2);
        }

        public static decimal PPM(double n1, decimal n2)
        {
            return ((Math.Abs((decimal)n1 - n2) * 1000000) / n2);
        }

        /// <summary>
        /// Uses Cryptography to generate really random numbers
        /// </summary>
        /// <param name="maxNo"></param>
        /// <returns></returns>
        public static int GetRandomNumber(int maxNo)
        {
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];

            // Create a new instance of the RNGCryptoServiceProvider. 
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            // Convert the byte to an integer value to make the modulus operation easier.
            int rand = Convert.ToInt32(randomNumber[0]);

            // Return the random number mod the number
            // of sides.  The possible values are zero-
            // based, so we add one.
            if (maxNo == 0) { return 0; }
            else
            {
                return rand % maxNo;
            }
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static IEnumerable<IEnumerable<T>>
    GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<PeptideMQ>> CombinationsOfK<PeptideMQ>(PeptideMQ[] data, int k)
        {
            int size = data.Length;

            IEnumerable<IEnumerable<PeptideMQ>> Runner(IEnumerable<PeptideMQ> list, int n)
            {
                int skip = 1;
                foreach (var headList in list.Take(size - k + 1).Select(h => new PeptideMQ[] { h }))
                {
                    if (n == 1)
                        yield return headList;
                    else
                    {
                        foreach (var tailList in Runner(list.Skip(skip), n - 1))
                        {

                            yield return headList.Concat(tailList);
                        }
                        skip++;
                    }
                }
            }

            return Runner(data, k);
        }

        public static string CleanPeptide(string peptide)
        {
            string cleanedPeptide = Regex.Replace(peptide, @"^[A-Z|\-|\*]+\.", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\.[A-Z|\-|\*]+$", "");
            cleanedPeptide = cleanedPeptide.Replace("*", "");
            cleanedPeptide = cleanedPeptide.Replace("#", "");
            cleanedPeptide = cleanedPeptide.Replace(".", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"([\[\]'|0-9|\)|\+|\(|\-|\0|a-z]+)", "");
            cleanedPeptide = Regex.Replace(cleanedPeptide, @"\([0-9|\.|\+|\-| |a-z|A-Z]*\)", "");

            return cleanedPeptide;
        }

        /// <summary>
        /// Check whether MSFileReader (Thermo Program) is installed in pc, because the ParserRAW needs a msfilereader DLL
        /// </summary>
        /// <returns></returns>
        public static bool checkMSFileReaderInstalled()
        {
            #region Windows 7 or later
            //Windows RegistryKey
            RegistryKey regKey = Registry.LocalMachine;
            regKey = regKey.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            if (regKey == null)
            {
                return false;
            }
            //Get key vector for each entry
            string[] keys = regKey.GetSubKeyNames();
            if (keys != null && keys.Length > 0)
            {
                //Interates key vector to try to get DisplayName
                for (int i = 0; i < keys.Length; i++)
                {
                    //Open current key
                    RegistryKey k = regKey.OpenSubKey(keys[i]);
                    try
                    {
                        //Get DisplayName
                        String appName = k.GetValue("DisplayName").ToString();

                        if (appName != null && appName.Length > 0 && appName.Contains("Thermo MSFileReader"))
                        {
                            return true;
                        }
                    }
                    catch (Exception) { }
                }
            }
            #endregion

            #region Windows Vista
            //Windows RegistryKey
            regKey = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            if (regKey == null)
            {
                return false;
            }
            //Get key vector for each entry
            keys = regKey.GetSubKeyNames();
            if (keys != null && keys.Length > 0)
            {
                //Interates key vector to try to get DisplayName
                for (int i = 0; i < keys.Length; i++)
                {
                    //Open current key
                    RegistryKey k = regKey.OpenSubKey(keys[i]);
                    try
                    {
                        //Get DisplayName
                        String appName = k.GetValue("DisplayName").ToString();

                        if (appName != null && appName.Length > 0 && appName.Contains("Thermo MSFileReader"))
                        {
                            return true;
                        }
                    }
                    catch (Exception) { }
                }
            }
            #endregion

            return false;
        }

        public static void openSpectrumViewerFromProteinCoverage()
        {

        }

    }

    public static class Ensure
    {
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
