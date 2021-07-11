# Searching for similar strings

## What is this about
In this project, I tried from a list of strings. Separate this list into two new lists. One with the unique strings and the other with the possible copies that exist.

For this use the string comparison method called Levenshtein distance (Wikipedia Link [LINK AQUI]). Also, I created some other variables to help me find possible duplicates with higher quality.

All the code is in C# inside a Unity project in version 2020.1 but the UnityPackage with the necessary for the project you can use even in versions before this one.

Or, you can download this UnityPackage with the important stuff [LINK PARA O UNITY PACKAGE]

## How to use
You can use the result in 3 ways. Calling through a **ScriptableObject**, importing an **XML file**, or **via script** calling the created method.

### To use scriptable

Access this file:

![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image2.PNG?raw=true "San Juan Mountains")

Now inside the "First List" field fill your string list. Then to perform the calculation click on "Compare Now" inside the context menu.

![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image3.PNG?raw=true "San Juan Mountains")

To see the result check the variable called "Duplicate Lists Result" as in the image below:

![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image5.PNG?raw=true "San Juan Mountains")

### Now to do the same process using the XML file
In the top menu of Unity click on "XML Importer" and then on "Import and Export XML". It will ask you for an XML file. Notes to follow this standard:

![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image6.PNG?raw=true "San Juan Mountains")

``  

      <?xml version="1.0" encoding="UTF-8"?>

      <phrases>

        <note>Ramon is the new black</note>

        <note>Thayane is the new black</note>

        <note>Hajime is the new black</note>

        <note>Madalena is the new black</note>

        <note>Jujuba is the new black</note>

        <note>Juujba is the new black</note>

      </phrases>
  ``

After choosing it the code will make the comparisons and ask a new location to save the result XML with the two lists.

### The third way to do this is by calling it by script. 
So you have to call the method "DuplicatedPhrases.Instance.SeparateStrings([YOUR STRING LIST - List<string>])"

The return of this method is a "DuplicateListsResults". It's a simple struct with 2 List<string>.

public struct DuplicateListsResults
{
    public List<string> NotDuplicateList;
    public List<string> DuplicateList;
}

Those 2 Lists contain the result of the comparison.

In any of those 3 ways, you can check all the comparisons made accessing the ScriptableObject. 
  
![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image2.PNG?raw=true "San Juan Mountains")

Inside it, you can open the variable called "Duplicates Result" and click on any one of the comparisons.

Inside the "Config Result" you can check all the parameters calculated and inside the "Results" you can see why it was marked like a duplicate if it is the case.

![](https://github.com/lipemon1/levenshteindistance/blob/main/Images/image8.PNG?raw=true "San Juan Mountains")

## Links used
### Unity Editor
https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
https://docs.unity3d.com/ScriptReference/EditorUtility.SaveFilePanel.html

### XML Files
https://stackoverflow.com/questions/8401280/read-a-xml-from-a-string-and-get-some-fields-problems-reading-xml
https://qawithexperts.com/article/c-sharp/create-xml-document-using-c-console-application-example/212
https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.loadxml?view=net-5.0
https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.load?view=net-5.0#System_Xml_XmlDocument_Load_System_String_

### Levenshtein Distance
https://en.wikipedia.org/wiki/Levenshtein_distance
https://social.technet.microsoft.com/wiki/contents/articles/26805.c-calculating-percentage-similarity-of-2-strings.aspx
https://stackoverflow.com/questions/6973972/remove-duplicate-in-a-string
https://www.dotnetperls.com/duplicates
https://www.codegrepper.com/code-examples/csharp/c%23+find+duplicates+in+list+of+strings
https://stackoverflow.com/questions/2344320/comparing-strings-with-tolerance
http://mihkeltt.blogspot.com/2009/04/dameraulevenshtein-distance.html
https://docs.microsoft.com/en-us/dotnet/csharp/how-to/compare-strings
