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

            #region 生成文件规则
            //string filename = System.IO.Path.GetFileName(templateFilePath);//文件名  “Api.Controllers.txt”
            //string extension = System.IO.Path.GetExtension(templateFilePath);//扩展名 “.txt”
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(templateFilePath);// 没有扩展名的文件名 “Api.Controllers”

            string fileName = "";  //className + "." + "cs";
            if (fileNameWithoutExtension.Contains("Controllers"))
            {
                fileName= className + "Controller." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Application.Interfaces"))
            {
                fileName = "I"+className + "AppService." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Application.Services"))
            {
                fileName = className + "AppService." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Domain.Interfaces.Repository"))
            {
                fileName = "I" + className + "Repository." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Domain.Interfaces.Services"))
            {
                fileName = "I" + className + "Service." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Domain.Services"))
            {
                fileName = className + "Service." + "cs";
            }
            else if (fileNameWithoutExtension.Contains("Infra.Data.Repository"))
            {
                fileName = className + "Repository." + "cs";
            }

            #endregion

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
        /// 删除旧的压缩包文件
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
