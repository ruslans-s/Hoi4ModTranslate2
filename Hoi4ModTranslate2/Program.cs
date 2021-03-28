using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hoi4ModTranslate2
{
    
    class Program
    {
      

        static void Main(string[] args)
        {
            
            

            List<string> fileName = new List<string>();
            DirectoryInfo dir = new DirectoryInfo("data");
            foreach (FileInfo file in dir.GetFiles())
            {
                fileName.Add(file.Name);
            }

            pullStringsFromFile(fileName);


            
            Console.WriteLine("Нажми кнопку");
            Console.ReadKey(true);
            fileName.Clear();
            foreach (FileInfo file in dir.GetFiles())
            {
                fileName.Add(file.Name);
            }

            recordingFromTranslatedFilesToFinal(fileName);

            Console.WriteLine("Ценок");
            Console.ReadKey(true);
        }

        
        //достаем строки и собираем out
        static void pullStringsFromFile(List<string> fileName)
        {
            string lineToEdit;
            List<string> textListFotOutFile = new List<string>();
            textListFotOutFile.Clear();
            //Цикл по файлам
            for (int j = 0; j < fileName.Count; j++)
            { 
            StreamReader sr = new StreamReader("data/" + fileName[j]);
            string line;
                //Цикл по файлу
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                lineToEdit = Regex.Replace(line, "^.+?:..", "");
                lineToEdit = Regex.Replace(lineToEdit, "\"", "");
                textListFotOutFile.Add(lineToEdit);
                 Console.Write("\r"+"Файл: "+j+"Строка: "+ textListFotOutFile.Count);
            }
            sr.Close();
               
                //запись в out
            fileName[j]= Regex.Replace(fileName[j], "yml$", "txt");
            StreamWriter sw = new StreamWriter("out/" + fileName[j]);
           for (int i = 1; i < textListFotOutFile.Count; i++)
           {
                    sw.WriteLine(textListFotOutFile[i]);
            }
          
            sw.Close();
                textListFotOutFile.Clear();
            }
        }
        static void chekError(List<string> fileName)
        {
            //  / n  n /  /п /N
        }
        static List<string> getTranslatedString(string fileName)
        {

            fileName = Regex.Replace(fileName, "yml$", "txt");
            StreamReader sr = new StreamReader("translated/" + fileName);

            List<string> dataList = new List<string>();

            //Цикл по переведенным файлам
            while (!sr.EndOfStream)
            {
                dataList.Add(sr.ReadLine());
              
            }
            sr.Close();
            return dataList;
        }

        static void recordingFromTranslatedFilesToFinal(List<string> fileName)
        {
            string lineToEdit;
           
            List<string> textListFotOutFile = new List<string>();
            List<string> dataList = new List<string>();
            string line;

            textListFotOutFile.Clear();
            //Цикл по файлам
            for (int j = 0; j < fileName.Count; j++)
            {

                /* fileName[j] = Regex.Replace(fileName[j], "yml$", "txt");
                 StreamReader sr = new StreamReader("translated/" + fileName[j]);

                 //Цикл по переведенным файлам
                 while (!sr.EndOfStream)
                 {
                     dataList.Add(sr.ReadLine());
                     Console.Write("\r" + "Файл: " + j + "Строка: " + textListFotOutFile.Count);
                 }
                 sr.Close();
                */

                dataList = getTranslatedString(fileName[j]);

                StreamReader sr = new StreamReader("data/" + fileName[j]);

                //Цикл по исходникам
                int k = 0;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    //Если превая строка индикатор языка пропускаем 
                    if(line.IndexOf("l_english:") > -1) continue;
                    

                    lineToEdit = Regex.Replace(line, ".\".+", " \"" + dataList[k] + "\"");
                    textListFotOutFile.Add(lineToEdit);
                    Console.Write("\r" + "Файл: " + j + "Строка: " + textListFotOutFile.Count);
                    k++;
                }
                sr.Close();


                //запись в out
                fileName[j] = Regex.Replace(fileName[j], "_english", "_russian");
                StreamWriter sw = new StreamWriter("finalOut/" + fileName[j]);
                sw.WriteLine("l_russian:");
                for (int i = 1; i < textListFotOutFile.Count; i++)
                {
                    sw.WriteLine(textListFotOutFile[i]);
                }

                sw.Close();

                dataList.Clear();
                textListFotOutFile.Clear();
            }
        }


    }
}
