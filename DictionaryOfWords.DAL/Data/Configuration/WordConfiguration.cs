﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DictionaryOfWords.DAL.Data.Configuration
{
    class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            
            builder.HasOne(p => p.Language)
                .WithMany(t => t.Words)
                .HasForeignKey(p => p.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
