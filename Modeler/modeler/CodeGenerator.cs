namespace Modeler.modeler
{
    interface ICodeGenerator
    {
        void Generate(string baseFileName);
        string GetCodeFileName(string modelFileName);
    }
}
