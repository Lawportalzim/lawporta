using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web;
using System.Web.Hosting;
using Kay.BLL;
using MailBee.SmtpMail;
using MailBee.Mime;
using System.Linq;

namespace Kay.Global
{
    public class Utilities
    {
        #region Update file references

        // Copy files from other folders and update HTML
        public static String FixFileReferences(String Html, String FolderPath)
        {
            // Images (src)
            String[] Images = FindMatches(Html, "\\ssrc=(\"|\')(?<path>[^\"\']+)(\"|\')", "path");
            if (Images != null)
            {
                foreach (String OldFilename in Images)
                {
                    if (!OldFilename.StartsWith(FolderPath) && !OldFilename.StartsWith("http://"))
                    {
                        String NewFilename = Utilities.MoveFile(OldFilename, FolderPath);
                        // Html = Regex.Replace(Html, OldFilename, NewFilename);
                        Html = Html.Replace(OldFilename, NewFilename);
                    }
                }
            }

            // Files (href)
            String[] Links = FindMatches(Html, "\\shref=(\"|\')(?<path>[^\"\']+)(\"|\')", "path");
            if (Links != null)
            {
                foreach (String OldFilename in Links)
                {
                    if (!OldFilename.StartsWith(FolderPath) && !OldFilename.StartsWith("http://"))
                    {
                        String NewFilename = Utilities.MoveFile(OldFilename, FolderPath);
                        // Html = Regex.Replace(Html, OldFilename, NewFilename);
                        Html = Html.Replace(OldFilename, NewFilename);
                    }
                }
            }

            // Done
            return Html;
        }

        #endregion

        #region Misc

        // Return a selected value from the appSettings section of the web.config file
        public static String GetAppSettingOption(String OptionName)
        {
            String option = ConfigurationManager.AppSettings[OptionName];
            if (String.IsNullOrEmpty(option))
            {
                throw new ApplicationException(OptionName + " missing from appSettings in web.config");
            }
            else
            {
                return option;
            }
        }

        // Random code
        private static Random random = new Random();
        public static string GenerateRandomCode(int length)
        {
            //Initiate objects & vars    
            String randomString = "";
            int randNumber;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < length; i++)
            {
                int _next = random.Next(1, 3);
                if (_next == 1)
                    randNumber = random.Next(97, 123); //char {a-z}
                else if (_next == 2)
                    randNumber = random.Next(65, 91); //char {A-Z}
                else
                    randNumber = random.Next(48, 58); //int {0-9}

                //append random char or digit to random string
                randomString = randomString + (char)randNumber;
            }
            //return the random string
            return randomString;
        }

        // Create a random password
        public static String GetRandomPasswordUsingGUID(Int32 length)
        {
            return GenerateRandomCode(length);
        }

        // Clear the cache
        public static Boolean ClearCache()
        {
            // Clear the cache
            try
            {
                ArrayList keyList = new ArrayList();
                IDictionaryEnumerator CacheEnum = HttpContext.Current.Cache.GetEnumerator();
                while (CacheEnum.MoveNext()) keyList.Add(CacheEnum.Key.ToString());
                foreach (string key in keyList) HttpContext.Current.Cache.Remove(key);
                return true;
            }
            catch (Exception err)
            {
                KayEventHistory.Error("Clear Cache Error", err);
                return false;
            }
        }

        #endregion

        #region File paths

        // Make an absolute path
        public static String MapPath(String Path)
        {
            // Nothing to do
            if (IsAbsolutePath(Path)) return Path;

            // Use stored path
            return FixAbsolutePath(Config.ApplicationPath + Path);
        }

        // Fix an absolute path
        public static String FixAbsolutePath(String Path)
        {
            Path = Regex.Replace(Path, "/", "\\");
            return Regex.Replace(Path, @"[\\]+", "\\");
        }

        // Check if a path is absolute
        public static bool IsAbsolutePath(String Path)
        {
            return Regex.IsMatch(Path, @"^[a-zA-Z]");
        }

        // Clean file paths (remove non-alphanumeric characters)
        public static String CleanFilePath(String Path, String FileName)
        {
            String SafeFileName = Regex.Replace(System.IO.Path.GetFileNameWithoutExtension(FileName), @"[^a-zA-Z0-9]+", "_");
            String Extension = System.IO.Path.GetExtension(FileName);
            return System.IO.Path.Combine(Path, SafeFileName + Extension);
        }

        #endregion

        #region File reading/writing

        // Write data to a file
        // Create file if it does not exist
        public static void WriteToFile(String FilePath, String Content, Boolean CreateNew)
        {
            // Check pacth
            String AbsoluteFilePath = IsAbsolutePath(FilePath) ? FilePath : MapPath(FilePath);

            // Create a new file
            Utilities.CreateDirectory(Path.GetDirectoryName(AbsoluteFilePath) + @"\");
            if (CreateNew && File.Exists(AbsoluteFilePath)) File.Delete(AbsoluteFilePath);

            // Write to file
            StreamWriter sw = File.AppendText(AbsoluteFilePath);
            sw.WriteLine(Content);
            sw.Close();
        }

        // Read a file to a string
        public static String ReadFromFile(String FilePath)
        {
            // Check pacth
            string AbsoluteFilePath = IsAbsolutePath(FilePath) ? FilePath : MapPath(FilePath);

            // Ignore
            if (!File.Exists(AbsoluteFilePath)) return String.Empty;

            // Open file and read
            StreamReader sr = File.OpenText(AbsoluteFilePath);
            string Contents = sr.ReadToEnd();
            sr.Close();

            // Return the string
            return Contents;
        }

        // Read last line
        public static String ReadLastLineFromFile(String FilePath)
        {
            // Check pacth
            String AbsoluteFilePath = IsAbsolutePath(FilePath) ? FilePath : MapPath(FilePath);

            // Ignore if file does not exists
            if (!File.Exists(AbsoluteFilePath)) return String.Empty;

            // Open file for reading
            StreamReader sr = File.OpenText(AbsoluteFilePath);

            // Loop until last line
            String Contents = String.Empty;
            String Line = String.Empty;
            while ((Line = sr.ReadLine()) != null) Contents = Line;

            // Close object
            sr.Close();

            // Return the string
            return Contents;
        }

        // Read all content from a folder
        public static String ReadFolderFiles(String[] Folders, String Extension)
        {
            // Build a string
            StringBuilder Builder = new StringBuilder();

            // Loop through folders
            foreach (String Folder in Folders)
            {
                // Loop through files
                if (Directory.Exists(Utilities.MapPath(Folder)))
                {
                    String[] Files = Directory.GetFiles(Utilities.MapPath(Folder));
                    foreach (String File in Files)
                    {
                        if (File.EndsWith("." + Extension)) Builder.AppendLine(Utilities.ReadFromFile(File));
                    }
                }
            }

            // Done
            return Builder.ToString();
        }

        #endregion

        #region File/Folder IO

        // Create all directories in a path
        public static void CreateDirectory(String DirectoryPath)
        {
            // Root path
            if (!IsAbsolutePath(DirectoryPath)) DirectoryPath = MapPath(DirectoryPath);

            // Start path
            String PathBuilder = Path.GetPathRoot(DirectoryPath);

            // Loop through folders
            String[] Folders = Path.GetDirectoryName(DirectoryPath).Split('\\');
            for (int i = 1; i < Folders.Length; i++)
            {
                // Add folder to path
                String Folder = Folders[i];
                PathBuilder += Folder + "\\";

                // Check if folder exists
                if (!Directory.Exists(PathBuilder)) Directory.CreateDirectory(PathBuilder);
            }
        }

        // Delete a file
        public static Boolean DeleteFile(String Path)
        {
            // Make path absolute
            Path = MapPath(Path);

            // Check if file exists
            if (!File.Exists(Path)) return false;

            // Try deleting file
            try
            {
                File.Delete(Path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete a folder
        public static Boolean DeleteFolder(String Path)
        {
            // Make path absolute
            Path = MapPath(Path);

            // Check if folder exists
            if (!Directory.Exists(Path)) return false;

            // Try deleting folder
            try
            {
                Directory.Delete(Path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Move a file
        public static String MoveFile(String SourcePath, String DestinationPath)
        {
            // Ignore (source file does not exist)
            if (!File.Exists(MapPath(SourcePath))) return SourcePath;

            String DestinationFolder = Path.GetDirectoryName(DestinationPath) + @"\\";
            String DestinationFileName = Path.GetFileName(DestinationPath);

            // Create destination directroy
            Utilities.CreateDirectory(DestinationFolder);

            // Get a unique destination file name
            if (String.IsNullOrEmpty(DestinationFileName))
            {
                DestinationPath = GetUniqueFilePath(DestinationFolder + @"\" + Path.GetFileName(SourcePath));
            }
            else
            {
                DeleteFile(DestinationPath);
            }

            // Move the file
            File.Move(MapPath(SourcePath), MapPath(DestinationPath));

            // Return the new file path
            return DestinationPath;
        }

        // Creates a unique filename
        public static String GetUniqueFilePath(String RelativePath)
        {
            // Check path
            String AbsolutePath = Utilities.MapPath(RelativePath);

            // Get parts
            String newPath = Path.GetDirectoryName(RelativePath) + "\\" + GetUniqueFileName(AbsolutePath);
            return newPath.Replace('\\', '/');
        }
        public static String GetUniqueFileName(String FolderPath)
        {
            // Check path
            String AbsolutePath = IsAbsolutePath(FolderPath) ? FolderPath : Utilities.MapPath(FolderPath);

            // Get Parts
            String directory = Path.GetDirectoryName(AbsolutePath);
            String fileName = Regex.Replace(Path.GetFileNameWithoutExtension(AbsolutePath), @"[^a-zA-Z0-9]+", "-");
            fileName = Regex.Replace(fileName, @"[\-]+$", "");
            String extension = Path.GetExtension(AbsolutePath);

            // Rename file
            String returnPath = directory + "\\" + fileName + extension;
            int fileIndex = 1;
            while (System.IO.File.Exists(returnPath))
            {
                // New filename
                if (Regex.IsMatch(fileName, @"-[0-9]+$")) fileName = Regex.Replace(fileName, @"-[0-9]+$", "-" + (fileIndex++));
                else fileName += "-" + fileIndex.ToString();

                // New path
                returnPath = directory + "\\" + fileName + extension;
            }

            // Return new filename
            return Path.GetFileName(returnPath);
        }

        #endregion

        #region Encryption

        // Convert a string to a series of Hex characters
        public static string ConvertToHexString(string Original)
        {
            string Hex = String.Empty;
            byte[] encodedChars = System.Text.Encoding.Unicode.GetBytes(Original);
            for (int i = 0; i < encodedChars.Length; i++)
            {
                Hex += encodedChars[i].ToString("X2"); //.PadLeft(2, '0');
            }
            return Hex;
        }

        // Convert series of Hex characters to a string
        public static string ConvertToString(string HexString)
        {
            string Original;
            int Discarded;
            Original = System.Text.Encoding.Unicode.GetString(HexEncoding.GetBytes(HexString, out Discarded));
            return Original;
        }

        // Encrypt or decrypt a string (uses global password)
        public static string EncryptText(string Data)
        {
            return EncryptText(Data, "");
        }
        public static string EncryptText(string Data, string Salt)
        {
            // URL encode string to preserve foriegn characters and NON-ANSI characters
            Data = System.Web.HttpUtility.UrlEncode(Data);

            // Encrypt string
            RC4Engine RC4 = new RC4Engine();
            RC4.EncryptionKey = Config.Rc4Password + Salt;
            RC4.InClearText = Data;
            RC4.Encrypt();
            string Encrypted = RC4.CryptedText;

            // Convert to HEX
            return Utilities.ConvertToHexString(Encrypted);
        }
        public static string DecryptText(string Data)
        {
            return DecryptText(Data, "");
        }
        public static string DecryptText(string Data, string Salt)
        {
            // Convert to string
            Data = Utilities.ConvertToString(Data);

            // Decrypt string
            RC4Engine RC4 = new RC4Engine();
            RC4.EncryptionKey = Config.Rc4Password + Salt;
            RC4.CryptedText = Data;
            RC4.Decrypt();
            string Decrypted = RC4.InClearText;

            // URL decode
            return System.Web.HttpUtility.UrlDecode(Decrypted);
        }

        // Encrypt text, return byte array
        public static byte[] Encrypt(string Data, string Salt)
        {
            // URL encode string to preserve foriegn characters and NON-ANSI characters e.g. Tugba Dasdemir
            Data = System.Web.HttpUtility.UrlEncode(Data);

            // Encrypt the string
            string Encrypted = EncryptText(Data, Salt);

            // Convert to byte array
            byte[] EncodedData = System.Text.Encoding.UTF8.GetBytes(Encrypted);

            // Return bytes
            return EncodedData;
        }

        // Decrypt data, return string
        public static string Decrypt(byte[] Data, string Salt)
        {
            // Convert byte array to string
            string DecodedData = System.Text.Encoding.UTF8.GetString(Data);

            // Decrypt the string
            string Decrypted = DecryptText(DecodedData, Salt);

            // URL decode string to preserve foriegn characters and NON-ANSI characters e.g. Tugba Dasdemir
            Decrypted = System.Web.HttpUtility.UrlDecode(Decrypted);

            // Return string
            return Decrypted;
        }

        #endregion

        #region Strings

        // Create URL path
        public static String GenerateUrlPath(Object Part1)
        {
            return GenerateUrlPath(Part1, String.Empty, String.Empty, String.Empty, String.Empty);
        }
        public static String GenerateUrlPath(Object Part1, Object Part2)
        {
            return GenerateUrlPath(Part1, Part2, String.Empty, String.Empty, String.Empty);
        }
        public static String GenerateUrlPath(Object Part1, Object Part2, Object Part3)
        {
            return GenerateUrlPath(Part1, Part2, Part3, String.Empty, String.Empty);
        }
        public static String GenerateUrlPath(Object Part1, Object Part2, Object Part3, Object Part4)
        {
            return GenerateUrlPath(Part1, Part2, Part3, Part4, String.Empty);
        }
        public static String GenerateUrlPath(Object Part1, Object Part2, Object Part3, Object Part4, Object Part5)
        {
            StringBuilder urlpath = new StringBuilder();

            if (!String.IsNullOrEmpty(Part1.ToString()))
            {
                urlpath.Append("/");
                urlpath.Append(SanitiseForUrlPath(Part1.ToString()));
            }
            if (!String.IsNullOrEmpty(Part2.ToString()))
            {
                urlpath.Append("/");
                urlpath.Append(SanitiseForUrlPath(Part2.ToString()));
            }
            if (!String.IsNullOrEmpty(Part3.ToString()))
            {
                urlpath.Append("/");
                urlpath.Append(SanitiseForUrlPath(Part3.ToString()));
            }
            if (!String.IsNullOrEmpty(Part4.ToString()))
            {
                urlpath.Append("/");
                urlpath.Append(SanitiseForUrlPath(Part4.ToString()));
            }
            if (!String.IsNullOrEmpty(Part5.ToString()))
            {
                urlpath.Append("/");
                urlpath.Append(SanitiseForUrlPath(Part5.ToString()));
            }

            // done
            return urlpath.ToString();
        }
        public static String SanitiseForUrlPath(String Text)
        {
            String sanitary = Text.ToLower();
            
            // remove "tame" characters
            sanitary = Regex.Replace(sanitary, @"[\'\""\.]+", "");

            // replace invalid characters with dashes
            sanitary = Regex.Replace(sanitary, @"[^a-z0-9]+", "-");

            // replace consecutive dashes
            sanitary = Regex.Replace(sanitary, @"[\-]+", "-");

            // remove trailing dash
            sanitary = Regex.Replace(sanitary, @"\-$", "");

            // remove leading dash
            sanitary = Regex.Replace(sanitary, @"^\-", "");

            // done
            return sanitary;
        }

        // Truncate
        public static String Truncate(String Text, int WordCount)
        {
            // remove html
            Text = StripHtml(Text);

            // limit to number of words
            String[] words = Text.Split(' ');
            StringBuilder truncated = new StringBuilder();
            int counter = 0;
            while (counter < WordCount && counter < words.Length)
            {
                truncated.Append(words[counter]);
                truncated.Append(" ");
                counter++;
            }

            // done
            if (counter < words.Length) truncated.Append("...");
            return HtmlSafe(truncated.ToString());
        }
        //Remove unwanted chars
        public static string RemoveNums(string Text)
        {
            Text = Text.Replace("800x600", "");
            return Text;
        }
        // HTML safe
        public static String HtmlSafe(String Text)
        {
            return HtmlSafe(Text, 0);
        }
        public static String HtmlSafe(String Text, Int32 Length)
        {
            if (Length > 0 && Text.Length > Length)
            {
                Text = Text.Substring(0, Length) + "...";
            }
            return HttpUtility.HtmlEncode(Text);
        }
        public static string HighlightKeyWords(string Text, string keywords, string cssClass, bool fullMatch)
        {
            Text = StripHtml(Text);
            if (Text == String.Empty || keywords == String.Empty || cssClass == String.Empty)
                return Text;
            var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (!fullMatch)
                return words.Select(word => word.Trim()).Aggregate(Text,
                             (current, pattern) =>
                             Regex.Replace(current,
                                             pattern,
                                               string.Format("<span style=\"background-color:{0}\">{1}</span>",
                                               cssClass,
                                               "$0"),
                                               RegexOptions.IgnoreCase));
            return words.Select(word => "\\b" + word.Trim() + "\\b")
                        .Aggregate(Text, (current, pattern) =>
                                  Regex.Replace(current,
                                  pattern,
                                    string.Format("<span style=\"background-color:{0}\">{1}</span>",
                                    cssClass,
                                    "$0"),
                                    RegexOptions.IgnoreCase));

        }
        public static string TruncateByKeyWords(string source, string searchWord, int sentPrepend = 1, int sentAppend = 1, bool onlyShowFirst = true, string viewMoreTag = "", bool alwaysShowViewMoreTag = false, string startTruncTag = "", bool returnSourceIfKeywordNotFound = false, string returnNotFound = "")
        {
            //http://chrisbitting.com/2014/02/06/search-and-truncate-trim-paragraph-by-sentence-c-not-word-or-character/
            //going to be the final string
            string truncated = "";

            //parse source sentences
            string[] sents = Regex.Split(source, @"(?<=[.?!;])\s+(?=\p{Lu})");

            //create some search start & end holders
            int i = 0;
            int ssent = -1;
            int esent = 0;

            //find start / end
            foreach (string sent in sents)
            {
                //search using regex for word boundaries \b
                if (Regex.IsMatch(sent, "\\b" + searchWord + "\\b", RegexOptions.IgnoreCase))
                {
                    if (ssent == -1)
                    {
                        ssent = i;
                    }
                    else
                    {
                        esent = i;
                    }
                }

                i++;
            }

            //make final string:

            if (esent == 0 || onlyShowFirst == true) esent = ssent;

            i = 0;

            foreach (string sent in sents)
            {
                if (i == ssent - sentPrepend || i == ssent || i == esent + sentAppend || (i >= ssent - sentPrepend && i <= esent + sentAppend))
                {
                    truncated = truncated + sent + " ";
                }

                i++;
            }

            //add view more

            if (esent + sentAppend + 1 < sents.Count() || alwaysShowViewMoreTag == true)
            {
                truncated = truncated + viewMoreTag;
            }

            //add beginning tag
            if (ssent - sentPrepend > 0)
            {
                truncated = startTruncTag + truncated;
            }

            //check if anything was even found:            
            if (ssent == -1)
            {
                if (returnSourceIfKeywordNotFound)
                { truncated = Truncate(RemoveNums(source), 15); }
                else
                {
                    truncated = returnNotFound;
                }
            }

            //and now return the final string - do a trim and remove double spaces.
            //did i ever mention how much i despise double spaces?
            return truncated.Trim().Replace("  ", " ");
        }
        // JavaScript safe
        public static String JsSafe(String Text)
        {
            return JsSafe(Text, 0);
        }
        public static String JsSafe(String Text, Int32 Length)
        {
            if (Length > 0 && Text.Length > Length)
            {
                Text = Text.Substring(0, Length) + "...";
            }
            return Text.Replace(@"'", @"\'").Replace(Environment.NewLine, "\\n");
        }

        // JSON safe
        public static String JsonSafe(String Text)
        {
            // remove line breaks
            Text = Text.Replace(Environment.NewLine, "\r\n");

            // remove quotes
            Text = Text.Replace("\"", "\\\"");

            // done
            return Text;
        }

        // CSV safe
        public static String CsvSafe(String Text)
        {
            return CsvSafe(Text, 0);
        }
        public static String CsvSafe(String Text, Int32 Length)
        {
            if (Length > 0 && Text.Length > Length)
            {
                Text = Text.Substring(0, Length) + "...";
            }
            return "\"" + Text.Replace("\"", "\"\"") + "\"";
        }

        // URL safe
        public static String UrlSafe(String Text)
        {
            return UrlSafe(Text, 0);
        }
        public static String UrlSafe(String Text, Int32 Length)
        {
            if (Length > 0 && Text.Length > Length)
            {
                Text = Text.Substring(0, Length) + "...";
            }
            return HttpUtility.UrlEncode(Text);
        }

        // Strip HTML markup
        public static String StripHtml(String Html)
        {
            return StripHtml(Html, 0);
        }
        public static String StripHtml(String Html, Int32 Length)
        {
            Html = Regex.Replace(Html, @"<\/?[^>]+>", String.Empty);
            Html = Regex.Replace(Html, @"^[\s|\r|\n]+", String.Empty);
            Html = Regex.Replace(Html, @"&[^;]{3,4};", " ");
            if (Length > 0 && Html.Length > Length)
            {
                Html = Html.Substring(0, Length) + "...";
            }
            return Html;
        }

        // Remove QueryString variables from a URL
        public static String RemoveQueryStringVariable(String Url, String QueryStringVars)
        {
            // Create array of vars
            string[] Vars = QueryStringVars.Split(',');

            // Get URL parts
            string Path, Qs;
            Path = Url.Split('?')[0];
            try { Qs = Url.Split('?')[1]; }
            catch { Qs = String.Empty; }

            // No query string
            if (Qs == String.Empty) return Path;

            // Get query string pairs
            string[] Pairs = Qs.Split('&');

            // Loop through pairs and create new query string
            string NewQs = String.Empty;
            for (int i = 0; i < Pairs.Length; i++)
            {
                // Get name-value 
                string[] NameValue = Pairs[i].Split('=');

                // Loop through removable vars and check if item should be removed
                bool Remove = false;
                for (int j = 0; j < Vars.Length; j++)
                {
                    // Check name
                    if (NameValue[0].ToLower() == Vars[j].ToLower()) Remove = true;
                }
                if (!Remove) NewQs += Pairs[i] + "&";
            }

            // Remove trailing ampersand
            if (NewQs != String.Empty) NewQs = "?" + NewQs.Substring(0, NewQs.Length - 1);

            // Return new URL
            return Path + NewQs;
        }

        // Add a QueryString variable to a URL replacing variables of the same name
        public static String AddQueryStringVariable(String Url, String QueryStringPairs)
        {
            // Common variables
            char[] Separator = new char[1];
            string Path;
            string QueryString;
            string NewQueryString = "";

            // Split url into path and querystring
            if (Url.IndexOf("?") > -1)
            {
                Separator[0] = '?';
                string[] UrlParts = Url.Split(Separator, 100);
                Path = UrlParts[0];
                QueryString = UrlParts[1];
            }
            else
            {
                Path = Url;
                QueryString = "";
            }

            // Split QueryStrings into pairs
            Separator[0] = '&';
            string[] NewQueryStringPairs = QueryStringPairs.Split(Separator);
            string[] OldQueryStringPairs = QueryString.Split(Separator);

            // Loop through old querystring
            for (int i = 0; i < OldQueryStringPairs.Length; i++)
            {
                // Get name/value pairs
                Separator[0] = '=';
                string[] OldNameValuePair = OldQueryStringPairs[i].Split(Separator);

                // Loop through new querystring looking for same name
                bool PairExists = false;
                for (int j = 0; j < NewQueryStringPairs.Length; j++)
                {
                    // Get name/value pairs
                    string[] NewNameValuePair = NewQueryStringPairs[j].Split(Separator);

                    if (NewNameValuePair[0] == OldNameValuePair[0])
                    {
                        PairExists = true;
                    }
                }

                // Add non-existent pair to new QueryString
                if (!PairExists)
                {
                    NewQueryString += OldQueryStringPairs[i] + "&";
                }
            }

            // Add new querystring
            NewQueryString += QueryStringPairs;

            // Remove leading ampersand (&)
            if (NewQueryString.IndexOf("&") == 0)
            {
                NewQueryString = NewQueryString.Substring(1, NewQueryString.Length - 1);
            }

            // Return new Url
            return Path + "?" + NewQueryString;
        }

        // Find strings matching a pattern
        public static String[] FindMatches(String Content, String Pattern, String Identifier)
        {
            // Temporary array to hold results
            ArrayList array = new ArrayList();

            // Check content
            Regex expression = new Regex(Pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = expression.Matches(Content);

            // Loop through all the matches
            foreach (Match m in matches)
            {
                // Loop through all the captures of the Identifier
                foreach (Capture c in m.Groups[Identifier].Captures)
                {
                    array.Add(c.Value);
                }
            }

            if (array.Count > 0)
            {
                // Matches exist
                String[] Matches = new String[array.Count];
                for (Int32 i = 0; i < array.Count; i++)
                {
                    Matches[i] = array[i].ToString();
                }
                return Matches;
            }
            else
            {
                // No matches
                return null;
            }
        }

        // Find image paths in a string
        public static ArrayList FindImages(String Html)
        {
            // Return a collection
            ArrayList Images = new ArrayList();
            String[] Patterns = new string[] { "\\ssrc=\"(?<path>[^\"]+)\"", "\\ssrc=\'(?<path>[^\']+)\'", "\\ssrc=(?<path>[^\'\"][^\\s]+)" };
            foreach (String pattern in Patterns)
            {
                // Find image references in the HTML
                Regex expression = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = expression.Matches(Html);
                foreach (Match m in matches)
                {
                    foreach (Capture c in m.Groups["path"].Captures)
                    {
                        String Image = c.Value;
                        if (!String.IsNullOrEmpty(Image) && !Image.StartsWith("http://"))
                        {
                            Boolean Exists = false;
                            foreach (Object img in Images)
                            {
                                if ((String)img == Image)
                                {
                                    Exists = true;
                                    break;
                                }
                            }
                            if (!Exists) Images.Add(Image);
                        }
                    }
                }
            }

            // Done
            return Images;
        }

        // Update content so that all paths use the domain name
        public static String UpdatePathsForExternal(String Html)
        {
            // Create absolute paths
            return Regex.Replace(Html, "(src|action|background|href|url\\(/|url\\('/)=('|\")/", @"$1=$2http://" + Config.DomainName + @"/");
        }

        // RFC822 date
        public static string GetRFC822Date(DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');
            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'));
        }

        #endregion

        #region Web requests

        // Request content
        public static String RequestBack(String Url)
        {
            return RequestBack(String.Empty, Url);
        }

        // POST variables and get response
        public static String RequestBack(String Query, String Url)
        {
            // Create the request
            HttpWebRequest PdtRequest = (HttpWebRequest)WebRequest.Create(Url);

            if (!String.IsNullOrEmpty(Query))
            {
                // Set values for the request back
                PdtRequest.Method = "POST";
                PdtRequest.ContentType = "application/x-www-form-urlencoded";
                PdtRequest.ContentLength = Query.Length;

                // Add query to the request back
                StreamWriter OutStream = new StreamWriter(PdtRequest.GetRequestStream(), System.Text.Encoding.ASCII);
                OutStream.Write(Query);
                OutStream.Close();
            }
            else
            {
                // Set verb
                PdtRequest.Method = "GET";
            }

            // Send request and get the response
            StreamReader InStream = new StreamReader(PdtRequest.GetResponse().GetResponseStream());
            String ResponseText = InStream.ReadToEnd();
            InStream.Close();

            // Return response string
            return ResponseText;
        }

        // Send XML and get response as XML
        public static XmlDocument XmlRequestBack(String Query, String Url)
        {
            // Create the request
            HttpWebRequest XmlRequest = (HttpWebRequest)WebRequest.Create(Url);

            // Set values for the request back
            XmlRequest.Method = "POST";
            XmlRequest.ContentLength = Query.Length;
            XmlRequest.ContentType = "text/xml";

            // Add query to the request back
            StreamWriter OutStream = new StreamWriter(XmlRequest.GetRequestStream());
            OutStream.Write(Query);
            OutStream.Close();

            // Web response (XML)
            HttpWebResponse XmlResponse = (HttpWebResponse)XmlRequest.GetResponse();
            XmlDocument responseXML = new XmlDocument();
            responseXML.Load(XmlResponse.GetResponseStream());
            XmlResponse.Close();

            // Return response string
            return responseXML;
        }

        #endregion

        #region Validation

        // Check for a valid email address
        public static Boolean ValidEmailAddress(String Text)
        {
            Regex Strict = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3} \.[0-9]{1,3}\.[0-9]{1,3}\.)" +
                @"|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return Strict.IsMatch(Text);
        }

        #endregion
        

        #region Search

        // Get search filter
        public static string GetSearchFilter(string Fields, string Keywords)
        {
            return GetSearchFilter(Fields, Keywords, false, "AND");
        }
        public static string GetMySqlSearchFilter(string Fields, string Keywords)
        {
            return GetSearchFilter(Fields, Keywords, false, "AND", true);
        }
        public static string GetSearchFilter(string Fields, string Keywords, bool MatchWholeWord, string DefaultOperator)
        {
            return GetSearchFilter(Fields, Keywords, MatchWholeWord, DefaultOperator, false);
        }
        public static string GetSearchFilter(string Fields, string Keywords, bool MatchWholeWord, string DefaultOperator, bool MySql)
        {
            // Ignore
            if (Keywords == "" || Keywords == null) return "";

            // Variables
            char[] separator = new char[1];
            string[] fields;
            string[] keywords;
            string filter = "";
            string matchWholeWord = MatchWholeWord ? " [^a-zA-Z] " : "";
            string defaultOperator = DefaultOperator;

            // Create array of fields and keywords
            separator[0] = ',';
            fields = Fields.Split(separator, 100);
            separator[0] = ' ';
            keywords = Keywords.Split(separator, 100);

            // Build filter
            for (int i = 0; i < keywords.Length; i++)
            {
                // Ignore *built-in* operators on alternate indexes
                if (!((keywords[i].ToUpper() == "AND" || keywords[i].ToUpper() == "OR") && ((i + 1) % 2 == 0 && i < keywords.Length - 1)))
                {
                    // Variable for current keyword
                    string keyword = keywords[i];

                    // Check for double quotes => search for string
                    if (keyword.StartsWith("\""))
                    {
                        // Remove double quote from keyword
                        keyword = keyword.Substring(1, keyword.Length - 1);

                        // Get all keywords until next double quote
                        while (!keyword.EndsWith("\"") && i < (keywords.Length - 1))
                        {
                            keyword += " " + keywords[++i];
                        }
                        if (keyword.EndsWith("\"")) keyword = keyword.Substring(0, keyword.Length - 1);

                        // Force matchWholeWord for this string (and remove trailing space)
                        keyword = matchWholeWord + keyword.TrimEnd() + matchWholeWord;
                    }

                    // Loop through fields
                    string filterChunk = "";
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (MySql) filterChunk += fields[j] + " LIKE '%" + matchWholeWord + keyword.Replace("'", "''") + matchWholeWord + "%'";
                        else filterChunk += "' '+" + fields[j] + "+' ' LIKE '%" + matchWholeWord + keyword.Replace("'", "''") + matchWholeWord + "%'";
                        if (j < fields.Length - 1) filterChunk += " OR ";
                    }
                    filter += " (" + filterChunk + ") " + defaultOperator + " ";
                }
                else
                {
                    // Remove current trailing operator
                    if (filter != String.Empty) filter = filter.Substring(0, filter.Length - defaultOperator.Length - 1);

                    // Add operator to filter i.e. AND/OR
                    filter += keywords[i].ToUpper() + " ";
                }
            }

            // Remove trailing operator
            if (filter != String.Empty) filter = filter.Substring(0, (filter.Length - defaultOperator.Length - 2));

            // Return filter
            return filter;
        }

        #endregion

        #region Enum

        // Create enum list item
        public static ListItem EnumListItem(Enum en)
        {
            Type type = en.GetType();
            int value = (int)type.GetField(en.ToString()).GetValue(en);
            return new ListItem(Utilities.GetEnumDescription(en), (value).ToString());
        }

        // Get description attribute from enum
        public static string GetEnumDescription(Type type, string text)
        {
            // Get info
            MemberInfo[] memInfo = type.GetMember(text);
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0) return ((DescriptionAttribute)attrs[0]).Description;
            }

            // Return enum string
            return text;
        }
        public static string GetEnumDescription(Enum en)
        {
            // Get type
            Type type = en.GetType();

            // Get info
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0) return ((DescriptionAttribute)attrs[0]).Description;
            }

            // Return enum string
            return en.ToString();
        }

        #endregion

        #region Download

        // Force download dialogue
        public static void DownloadFile(string FilePath)
        {
            SendFile(FilePath, true);
        }
        public static void SendFile(string FilePath, Boolean ForceDownload)
        {
            HttpResponse Response = HttpContext.Current.Response;
            if (!String.IsNullOrEmpty(FilePath))
            {
                String filePath = IsAbsolutePath(FilePath) ? FilePath : Utilities.MapPath(FilePath);
                String fileName = System.IO.Path.GetFileName(filePath);

                String contentType;
                switch (Path.GetExtension(fileName).ToLower())
                {
                    case ".jpg":
                    case ".jpeg": contentType = "image/jpeg"; break;
                    case ".tif":
                    case ".tiff": contentType = "image/tiff"; break;
                    case ".gif": contentType = "image/gif"; break;
                    default: contentType = "application/octet-stream"; break;
                }

                if (ForceDownload)
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "Attachment; Filename=" + fileName);
                }
                else
                {
                    Response.ContentType = contentType;
                }
                Response.TransmitFile(filePath);
                Response.End();
            }
        }
        public static void DownloadFileAsStream(string FilePath)
        {
            System.IO.Stream iStream = null;
            HttpResponse Response = HttpContext.Current.Response;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file to download including its path.
            string filepath = IsAbsolutePath(FilePath) ? FilePath : Utilities.MapPath(FilePath);

            // Identify the file name.
            string filename = System.IO.Path.GetFileName(filepath);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);


                // Total bytes to read:
                dataToRead = iStream.Length;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                Response.Close();
            }

        }
        public static void DownloadBytes(byte[] Buffer, string Filename)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", "Attachment; Filename=\"" + Filename + "\"");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(Buffer);
            HttpContext.Current.Response.End();
        }

        #endregion

        #region XML

        // Convert string to XML
        public static XmlDocument StringToXmlDocument(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }
        public static XmlNode StringToXmlNode(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }

        // Prettify XML document
        public static String PrettifyXml(XmlDocument document)
        {
            return PrettifyXml(document.DocumentElement);
        }
        public static String PrettifyXml(XmlNode node)
        {
            // Prettify output
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = " ";
            settings.NewLineChars = Environment.NewLine;
            settings.ConformanceLevel = ConformanceLevel.Document;

            // Create document
            MemoryStream ms = new MemoryStream();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(ms, settings);
            node.WriteTo(writer);
            writer.Flush();
            writer.Close();
            settings = null;
            ms.Position = 0;

            // Read to string
            String xml = String.Empty;
            StreamReader sr = new StreamReader(ms);
            xml = sr.ReadToEnd();
            sr.Close();
            sr = null;
            ms.Close();
            ms = null;

            // Done
            return xml;
        }

        #endregion
    }
}
