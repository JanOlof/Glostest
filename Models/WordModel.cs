namespace Glostest
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WordModel : DbContext
    {
        public WordModel()
            : base("name=WordModel")
        {
        }

        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Synonyms> Synonyms { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<WordGroup> WordGroup { get; set; }
        public virtual DbSet<WordGroupSynonym> WordGroupSynonym { get; set; }
        public virtual DbSet<WordTest> WordTest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Language>()
                .HasMany(e => e.Word)
                .WithRequired(e => e.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Word>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<Word>()
                .HasMany(e => e.Synonyms)
                .WithRequired(e => e.Word)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WordGroup>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<WordGroup>()
                .HasMany(e => e.WordGroupSynonym)
                .WithRequired(e => e.WordGroup)
                .WillCascadeOnDelete(false);
        }
    }
}
