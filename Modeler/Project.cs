using System.IO;
using System.Xml.Serialization;

namespace Modeler
{
    public class Project
    {
        // имя проекта
        public string ProjectName = "";
        // имя файла проекта
        public string FileName = "";
        // путь к файлу проекта(mdd)
        public string FilePath = "";
        // путь к папки с моделями
        public string FileModelPath = "";
        //путь к папке с кодом
        public string FileCodePath = "";

        public void Init(string fileName)
        {
            FileName = fileName;
            ProjectName = Path.GetFileNameWithoutExtension(FileName);
            FilePath = Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName);
            FileModelPath = FilePath + "\\model";
            FileCodePath = FilePath + "\\code";
        }

        public static Project Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(Project));
            var reader = new StreamReader(fileName);
            var pr = serializer.Deserialize(reader) as Project;
            reader.Close();
            if (pr == null)
            {
                return null;
            }
            pr.FileCodePath = Path.GetDirectoryName(fileName) + "\\" + pr.ProjectName + "\\code";
            pr.FileModelPath = Path.GetDirectoryName(fileName) + "\\" + pr.ProjectName + "\\model";
            pr.FileName = fileName;
            pr.FilePath = Path.GetDirectoryName(fileName);
            return pr;
        }
        public void CreatePaths()
        {
            Directory.CreateDirectory(FilePath);
            Directory.CreateDirectory(FileModelPath);
            Directory.CreateDirectory(FileCodePath);
        }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(Project));
            var writer = new StreamWriter(FileName);
            serializer.Serialize(writer, this);
            writer.Close();
        }
    }
}
