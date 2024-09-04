using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesLibraryApp.Data.Entities;

public class EntityBase : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public string Type { get; set; }
    public override string ToString() => $"Title: {Title}, Year: {Year}, Director: {Director}, Type: {Type}";
}