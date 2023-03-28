using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClassVisualizer
{
    public class ClassVisualizer
    {
        /// <summary>
        /// This will open a browser page and list the properties of the type in a frame as collapsable components.
        /// </summary>
        /// <param name="type">Type of the class that is intended to be visualized</param>
        public static void Visualize(Type type)
        {
            ComponontGeneratorFromType generator = new ComponontGeneratorFromType();
            string markup = $@"<!DOCTYPE html>
<style>
body {{font-family: 'Segoe UI'}}
.class-box {{ width:550px; border:solid 3px black; background-color:white;}}
.property-box {{5width:90%; margin-bottom:0.5em; margin-left: 4%; border:dashed 1px gray; cursor: pointer; padding:0.2em; }}
h4 {{ margin: auto; width: 96%; text-align: center;}}
.cs-type {{color:red;}}
.cs-access {{color:blue;}}
</style>
<Body>
{generator.GetHtmlComponent(type)}
</Body>";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClassVisualizer", "deneme.html");
            Console.WriteLine(Path.GetDirectoryName(path));
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {

                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            File.WriteAllText(path, markup);

            Process proc = Process.Start(path);
            proc.WaitForExit();
            Console.ReadLine();
        }

    }
    class Deneme
    {
        public double ID { get; set; }
        public string url;
        public string getName()
        {
            return "name";
        }
        public string Name()
        {
            return "Name";
        }
    }

    class MajorDeneme
    {
        public Deneme MyDeneme { get; set; }
        public string Name;
    }
}
