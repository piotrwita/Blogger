namespace Domain.Entities
{
    //Atrybut Table służy do konfigurowania nazwy tabeli w bazie danych
    [Table("Posts")]
    public class Post : AuditableEntitiy
    {
        //atrybut Key służy do wskazania klucza głównego w tabeli
        [Key]
        public int Id { get; set; }

        //wymagalność danych
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        #region Constructors

        public Post()
        {
        }

        public Post(int id, string title, string content)
        {
            Id = id;

            Title = 
                title ?? throw new ArgumentNullException(nameof(title));

            Content =
                content ?? throw new ArgumentNullException(nameof(content)); 
        }

        #endregion
    }
}
