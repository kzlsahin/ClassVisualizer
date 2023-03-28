using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassVisualizer
{
    internal class ComponontGeneratorFromType
    {
        protected HashSet<string> _typesSet = new HashSet<string>();
        protected Dictionary<string, string> _stack = new Dictionary<string, string>();
        public string GetHtmlComponent(Type T)
        {
            string markup = $@"<div class='class-box'>
<h4>{T.FullName}</h4>
<hr />
<div class='property-field'>{GetHtmlComponent(T.GetProperties())}</div>
<hr />
<div class='field-field'>{GetHtmlComponent(T.GetFields())}</div>
<hr />
<div class='method-field'></div>
</div>";
            //string pattern = @"(?<=-->Type:)\w+.*(?=<--)";
            //foreach (Match match in Regex.Matches(markup, pattern))
            //{
            //    string name = match.ToString();
            //    Console.WriteLine(name);
            //    string newMark;
            //    if (_stack.TryGetValue(name, out newMark))
            //    {
            //        Console.WriteLine("FOUND");
            //        markup = markup.Replace($"-->Type:{name}<--", newMark);
            //    }

            //}
            return markup;
        }
        public string GetHtmlComponent(PropertyInfo[] properties)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach(PropertyInfo prop in properties){
                sb.Append(GetHtmlComponent(prop));
            }
            return sb.ToString();
        }
        public string GetHtmlComponent(FieldInfo[] properties)
        {
            StringBuilder sb = new StringBuilder();

            foreach (FieldInfo field in properties)
            {
                sb.Append(GetHtmlComponent(field));
            }
            return sb.ToString();
        }

        string GetHtmlComponent(PropertyInfo propertyInfo)
        {
            string propTypeFullName = propertyInfo.PropertyType.FullName;
            if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType == typeof(string))
            {
                return $@"<div class='property-box'><span class='cs-access'>{{ {(propertyInfo.CanRead ? "get" : "_")}; {(propertyInfo.CanWrite? "set" : "-")}; }}</span>  <span class='cs-type'>{propertyInfo.PropertyType.Name}</span> {propertyInfo.Name}</div>";
            }
            string subType;
            if (_typesSet.Add(propTypeFullName))
            {
                subType = GetHtmlComponent(propertyInfo.PropertyType);
                _stack.Add(propTypeFullName, subType);
            }
            else
            {
                _stack.TryGetValue(propTypeFullName, out subType);
            }
            return $@"<details class='property-box'><summary><span class='cs-access'>{{ {(propertyInfo.CanRead ? "get" : "_")}; {(propertyInfo.CanWrite ? "set" : "-")}; }}</span>  <span class='cs-type'>{propertyInfo.PropertyType.Name}</span> {propertyInfo.Name}</summary>
{subType}</details>";
        }
        string GetHtmlComponent(FieldInfo fieldInfo)
        {
            string fieldTypeFullName = fieldInfo.FieldType.FullName;
            if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string))
            {
                return $@"<div class='property-box'> <span class='cs-access'>{GetFieldAccessibility(fieldInfo)}</span> {(fieldInfo.IsStatic ? "static" : " ")} <span class='cs-type'>{fieldInfo.FieldType.Name}</span> {fieldInfo.Name}</div>";
            }
            string subType;
            if (_typesSet.Add(fieldTypeFullName))
            {
                subType = GetHtmlComponent(fieldInfo.FieldType);
                _stack.Add(fieldTypeFullName, subType);
            }
            else
            {
                _stack.TryGetValue(fieldTypeFullName, out subType);
            }
            return $@"<details class='property-box'><summary><span class='cs-access'>{GetFieldAccessibility(fieldInfo)}</span> {(fieldInfo.IsStatic ? "static |" : " ")} <span class='cs-type'>{fieldInfo.FieldType.Name}</span> {fieldInfo.Name}</summary>{subType}</details>";
        }

        string GetFieldAccessibility(FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic)
            {
                return "public";
            }
            else if (fieldInfo.IsPrivate)
            {
                return "private";
            }
            else if (fieldInfo.IsFamily)
            {
                return "protected";
            }
            else if (fieldInfo.IsAssembly)
            {
                return "internal";
            }
            else if (fieldInfo.IsFamilyOrAssembly)
            {
                return "protected internal";
            }
            else if (fieldInfo.IsFamilyAndAssembly)
            {
                return "private protected";
            }
            else
            {
                return "----";
            }
        }
    }
}
