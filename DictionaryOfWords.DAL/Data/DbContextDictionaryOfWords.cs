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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<WordTranslation> WordTranslations { get; set; }

        public DbContextDictionaryOfWords(DbContextOptions<DbContextDictionaryOfWords> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;uid=root;pwd=123456;database=DictionaryOfWords");
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
