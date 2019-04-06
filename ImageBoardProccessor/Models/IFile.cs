namespace ImageBoardProcessor.Models
{
    public interface IFile
    {
        string File_ext { get; set; }
        string File_url { get; set; }
        string filename { get; }
        int Id { get; set; }
    }
}