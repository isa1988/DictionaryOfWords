using DictionaryOfWords.Core.DataBase;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.DAL.Data.Configuration;

namespace DictionaryOfWords.DAL.Data
{
    public class DbContextDictionaryOfWords : IdentityDbContext<User, Role, int>
    { 
        public DbContextDictionaryOfWords(DbContextOptions<DbContextDictionaryOfWords> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<WordTranslation> WordTranslations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new WordConfiguration());
            modelBuilder.ApplyConfiguration(new WordTranslationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
