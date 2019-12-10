﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.DAL.Data.Configuration
{
    class WordTranslationConfiguration : IEntityTypeConfiguration<WordTranslation>
    {
        public void Configure(EntityTypeBuilder<WordTranslation> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.WordTranslationValue)
                .WithMany(t => t.WordTranslations)
                .HasForeignKey(p => p.WordTranslationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.WordSource)
                .WithMany(t => t.WordSources)
                .HasForeignKey(p => p.WordSourceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.LanguageWord)
                .WithMany(t => t.WordTranslations)
                .HasForeignKey(p => p.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
