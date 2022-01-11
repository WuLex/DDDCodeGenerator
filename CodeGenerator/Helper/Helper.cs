using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeGenerator.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 驼峰式
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCamelCase(string text)
        {
            TextInfo txtInfo = new CultureInfo("pt-br", false).TextInfo;
            return txtInfo.ToTitleCase(text).Replace("_", string.Empty);
        }

        /// <summary>
        /// 驼峰式首字母小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCamelCaseLoweFirst(string text)
        {
            TextInfo txtInfo = new CultureInfo("zh-CN", false).TextInfo;
            text = txtInfo.ToTitleCase(text).Replace("_", string.Empty);
            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFilePath"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        public static void CreateFiles(string templateFilePath, string nameSpace, string className)
        {
            StreamReader rd = new StreamReader(templateFilePath);
            string strContents = rd.ReadToEnd();
            rd.Close();

            Regex regexText = new Regex("#NAMESPACE#");
            strContents = regexText.Replace(strContents, ToCamelCase(nameSpace));
            regexText = new Regex("#CLASSNAME#");
            strContents = regexText.Replace(strContents, ToCamelCase(className));
            regexText = new Regex("#VARCLASSNAME#");
            strContents = regexText.Replace(strContents, ToCamelCaseLoweFirst(className));

            string pathToSave = Directory.GetCurrentDirectory() + @"\DownloadFiles\" +
                                Path.GetFileNameWithoutExtension(templateFilePath).Replace(".", @"\");

            string fileName = className + "." + "cs";

            var path = Path.Combine(pathToSave, fileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(strContents);
                    sw.Close();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesNameToGenerate"></param>
        /// <param name="downloadFilePath"></param>
        public static void DeleteOlderFiles(List<string> filesNameToGenerate, string downloadFilePath)
        {
            foreach (var templateFilePath in filesNameToGenerate)
            {
                var pathFileToDelete = downloadFilePath + @"\" + Path.GetFileNameWithoutExtension(templateFilePath).Replace(".", @"\");
                var directoryInfo = new DirectoryInfo(pathFileToDelete);

                if (directoryInfo.GetFiles().Count() > 0)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
        }
    }
}