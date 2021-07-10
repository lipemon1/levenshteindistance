using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class OpenFilePanelExample : EditorWindow
{
    [MenuItem("XML Importer/Import and Export XML")]
    static void ImportXML()
    {
        string path = EditorUtility.OpenFilePanel("Select your xml file", "", "xml");
        if (path.Length != 0)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            List<string> stringsFoundInXML = new List<string>();

            foreach (XmlNode node in xmlDocument.DocumentElement)
                stringsFoundInXML.Add(node.InnerText);

            if(stringsFoundInXML?.Count > 0)
            {
                DuplicateListsResults duplicateListsResults = DuplicatedPhrases.Instance.SeparateStrings(stringsFoundInXML);
                SaveNewXMLList(duplicateListsResults);
            }
            else
            {
                Debug.LogError("No string found on this XML");
            }
        }
    }

    static void SaveNewXMLList(DuplicateListsResults duplicatesResult)
    {
        //Decalre a new XMLDocument object
        XmlDocument doc = new XmlDocument();

        //xml declaration is recommended, but not mandatory
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

        //create the root element
        XmlElement root = doc.DocumentElement;
        doc.InsertBefore(xmlDeclaration, root);

        //string.Empty makes cleaner code
        XmlElement mainBodyElement = doc.CreateElement(string.Empty, "Mainbody", string.Empty);
        doc.AppendChild(mainBodyElement);

        XmlElement notDuplicateElement = doc.CreateElement(string.Empty, "NotDuplicates", string.Empty);
        foreach (string singleString in duplicatesResult.NotDuplicateList)
        {
            XmlText newItem = doc.CreateTextNode(singleString);
            XmlElement newInnerElem = doc.CreateElement(string.Empty, "SingleString", string.Empty);
            newInnerElem.AppendChild(newItem);
            notDuplicateElement.AppendChild(newInnerElem);
        }

        XmlElement duplicateElement = doc.CreateElement(string.Empty, "Duplicates", string.Empty);
        foreach (string singleString in duplicatesResult.DuplicateList)
        {
            XmlText newItem = doc.CreateTextNode(singleString);
            XmlElement newInnerElem = doc.CreateElement(string.Empty, "DuplicateString", string.Empty);
            newInnerElem.AppendChild(newItem);
            duplicateElement.AppendChild(newInnerElem);
        }

        mainBodyElement.AppendChild(notDuplicateElement);
        mainBodyElement.AppendChild(duplicateElement);

        var path = EditorUtility.SaveFilePanel("Save your analyzed xml", "", "Analyzed XML.xml", "xml");
        if(path.Length != 0)
            doc.Save(path);
    }
}